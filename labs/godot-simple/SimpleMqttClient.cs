using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace godotsimple;

/// <summary>
/// Thin MQTT helper: connect, subscribe, raise messages on the Godot main thread.
/// </summary>
public partial class SimpleMqttClient : Node
{
	[Export] public string BrokerHost { get; set; } = "localhost";
	[Export] public int BrokerPort { get; set; } = 1883;

	public bool IsMqttConnected { get; private set; }

	/// <summary>topic, payload text</summary>
	public event Action<string, string>? MessageReceived;
	public event Action<bool, string>? ConnectionChanged;

	private IMqttClient? _client;
	private readonly ConcurrentQueue<Action> _mainThread = new();
	private string[] _topics = Array.Empty<string>();

	public override void _Process(double delta)
	{
		while (_mainThread.TryDequeue(out var action))
			action();
	}

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

	public async Task ConnectAsync(params string[] topics)
	{
		_topics = topics ?? Array.Empty<string>();

		var factory = new MqttFactory();
		_client = factory.CreateMqttClient();

		_client.ConnectedAsync += async _ =>
		{
			IsMqttConnected = true;
			Enqueue(() => ConnectionChanged?.Invoke(true, "connected"));

			foreach (var topic in _topics)
			{
				await _client.SubscribeAsync(new MqttTopicFilterBuilder()
					.WithTopic(topic)
					.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
					.Build());
			}
		};

		_client.DisconnectedAsync += _ =>
		{
			IsMqttConnected = false;
			Enqueue(() => ConnectionChanged?.Invoke(false, "disconnected"));
			return Task.CompletedTask;
		};

		_client.ApplicationMessageReceivedAsync += e =>
		{
			var topic = e.ApplicationMessage.Topic;
			var text = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
			Enqueue(() => MessageReceived?.Invoke(topic, text));
			return Task.CompletedTask;
		};

		var options = new MqttClientOptionsBuilder()
			.WithTcpServer(BrokerHost, BrokerPort)
			.WithClientId($"inc492-simple-{Guid.NewGuid():N}"[..24])
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
		IsMqttConnected = false;
	}

	private void Enqueue(Action action) => _mainThread.Enqueue(action);
}
