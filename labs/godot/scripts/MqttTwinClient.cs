using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace godot1;

/// <summary>
/// MQTT client for the digital twin. Marshals callbacks onto the Godot main thread.
/// </summary>
public partial class MqttTwinClient : Node
{
	[Export] public string BrokerHost { get; set; } = "localhost";
	[Export] public int BrokerPort { get; set; } = 1883;

	public bool MqttConnected { get; private set; }

	public event Action<JsonElement>? TelemetryReceived;
	public event Action<JsonElement>? EventReceived;
	public event Action<JsonElement>? StatusReceived;
	public event Action<bool, string>? ConnectionChanged;

	private IMqttClient? _client;
	private readonly ConcurrentQueue<Action> _mainThread = new();
	private int _cmdSeq;

	public override void _Process(double delta)
	{
		while (_mainThread.TryDequeue(out var action))
		{
			action();
		}
	}

	public async Task ConnectAsync()
	{
		var factory = new MqttFactory();
		_client = factory.CreateMqttClient();

		_client.ConnectedAsync += async _ =>
		{
			MqttConnected = true;
			Enqueue(() => ConnectionChanged?.Invoke(true, "connected"));

			await _client.SubscribeAsync(new MqttTopicFilterBuilder()
				.WithTopic("twin/telemetry/#")
				.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
				.Build());
			await _client.SubscribeAsync(new MqttTopicFilterBuilder()
				.WithTopic("twin/event")
				.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
				.Build());
			await _client.SubscribeAsync(new MqttTopicFilterBuilder()
				.WithTopic("twin/status/#")
				.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
				.Build());
		};

		_client.DisconnectedAsync += _ =>
		{
			MqttConnected = false;
			Enqueue(() => ConnectionChanged?.Invoke(false, "disconnected"));
			return Task.CompletedTask;
		};

		_client.ApplicationMessageReceivedAsync += e =>
		{
			var topic = e.ApplicationMessage.Topic;
			var text = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
			try
			{
				using var doc = JsonDocument.Parse(text);
				var root = doc.RootElement.Clone();
				Enqueue(() =>
				{
					if (topic.StartsWith("twin/telemetry/"))
						TelemetryReceived?.Invoke(root);
					else if (topic == "twin/event")
						EventReceived?.Invoke(root);
					else if (topic.StartsWith("twin/status/"))
						StatusReceived?.Invoke(root);
				});
			}
			catch (Exception ex)
			{
				GD.PrintErr($"[mqtt] bad JSON on {topic}: {ex.Message}");
			}

			return Task.CompletedTask;
		};

		var options = new MqttClientOptionsBuilder()
			.WithTcpServer(BrokerHost, BrokerPort)
			.WithClientId($"inc492-godot-{Guid.NewGuid():N}"[..22])
			.WithCleanSession()
			.Build();

		await _client.ConnectAsync(options);
	}

	public async Task DisconnectAsync()
	{
		if (_client is null)
			return;
		if (_client.IsConnected)
			await _client.DisconnectAsync();
		_client.Dispose();
		_client = null;
		MqttConnected = false;
	}

	public async Task PublishCommandAsync(string action, object paramsObj, string commandId)
	{
		if (_client is null || !_client.IsConnected)
			throw new InvalidOperationException("MQTT not connected");

		_cmdSeq++;
		var payload = new
		{
			schema_version = "1.0",
			source = "hmi/godot",
			timestamp_utc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
			seq = _cmdSeq,
			payload = new
			{
				command_id = commandId,
				action,
				@params = paramsObj,
				requested_by = "operator"
			}
		};

		// anonymous type cannot use "params" as property name — serialize dictionary instead
		var root = new Dictionary<string, object?>
		{
			["schema_version"] = "1.0",
			["source"] = "hmi/godot",
			["timestamp_utc"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
			["seq"] = _cmdSeq,
			["payload"] = new Dictionary<string, object?>
			{
				["command_id"] = commandId,
				["action"] = action,
				["params"] = paramsObj,
				["requested_by"] = "operator"
			}
		};
		_ = payload;

		var json = JsonSerializer.Serialize(root);
		var message = new MqttApplicationMessageBuilder()
			.WithTopic("twin/command/controller")
			.WithPayload(json)
			.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
			.Build();

		await _client.PublishAsync(message);
	}

	private void Enqueue(Action action) => _mainThread.Enqueue(action);

	public override void _ExitTree()
	{
		try
		{
			DisconnectAsync().GetAwaiter().GetResult();
		}
		catch
		{
			// ignore shutdown errors
		}
	}
}
