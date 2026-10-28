using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

namespace godotsimple;

/// <summary>
/// Rotates this mesh from MQTT topic <c>twin/rotate</c>, smoothly via quaternion slerp.
/// Expected JSON examples:
///   {"y": 45}
///   {"x": 0, "y": 45, "z": 0}
///   {"yaw": 90}
///   45
/// </summary>
public partial class BoxScript : MeshInstance3D
{
	[Export] public string BrokerHost { get; set; } = "localhost";
	[Export] public int BrokerPort { get; set; } = 1883;
	[Export] public string RotateTopic { get; set; } = "twin/rotate";

	/// <summary>Higher = reaches the target faster. ~4–10 feels natural.</summary>
	[Export] public float SlerpSpeed { get; set; } = 4f;

	private SimpleMqttClient _mqtt = null!;
	private Vector3 _targetDegrees;
	private Quaternion _targetRotation = Quaternion.Identity;

	public override void _Ready()
	{
		_targetDegrees = RotationDegrees;
		_targetRotation = Quaternion;

		_mqtt = new SimpleMqttClient
		{
			Name = "SimpleMqttClient",
			BrokerHost = BrokerHost,
			BrokerPort = BrokerPort
		};
		AddChild(_mqtt);

		_mqtt.ConnectionChanged += (ok, detail) =>
			GD.Print(ok
				? $"[box] MQTT {detail} — subscribed to {RotateTopic}"
				: $"[box] MQTT {detail}");

		_mqtt.MessageReceived += OnMqttMessage;
		_ = ConnectAsync();
	}

	public override void _Process(double delta)
	{
		var angle = Quaternion.AngleTo(_targetRotation);
		if (angle < 0.001f)
		{
			Quaternion = _targetRotation;
			return;
		}

		// Frame-rate independent ease toward target.
		var t = 1f - Mathf.Exp(-SlerpSpeed * (float)delta);
		Quaternion = Quaternion.Slerp(_targetRotation, t);
	}

	private async Task ConnectAsync()
	{
		try
		{
			await _mqtt.ConnectAsync(RotateTopic);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"[box] MQTT connect failed: {ex.Message}");
			GD.PrintErr("[box] Start Lab 01 broker (docker), then restart this scene.");
		}
	}

	private void OnMqttMessage(string topic, string payload)
	{
		if (topic != RotateTopic)
			return;
		ApplyRotatePayload(payload);
	}

	private void ApplyRotatePayload(string text)
	{
		try
		{
			using var doc = JsonDocument.Parse(text);
			var root = doc.RootElement;
			var degrees = _targetDegrees;

			if (root.ValueKind == JsonValueKind.Number)
			{
				degrees.Y = root.GetSingle();
			}
			else if (root.ValueKind == JsonValueKind.Object)
			{
				if (TryGetFloat(root, "x", out var x)) degrees.X = x;
				if (TryGetFloat(root, "y", out var y)) degrees.Y = y;
				if (TryGetFloat(root, "z", out var z)) degrees.Z = z;
				if (TryGetFloat(root, "yaw", out var yaw)) degrees.Y = yaw;
			}
			else
			{
				GD.PrintErr($"[box] twin/rotate: unsupported JSON kind {root.ValueKind}");
				return;
			}

			_targetDegrees = degrees;
			_targetRotation = Quaternion.FromEuler(degrees * Mathf.Pi / 180f);
			GD.Print($"[box] rotate target → ({degrees.X:0.#}, {degrees.Y:0.#}, {degrees.Z:0.#})");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"[box] twin/rotate bad payload: {ex.Message} | raw={text}");
		}
	}

	private static bool TryGetFloat(JsonElement obj, string name, out float value)
	{
		value = 0f;
		if (!obj.TryGetProperty(name, out var el))
			return false;

		switch (el.ValueKind)
		{
			case JsonValueKind.Number:
				value = el.GetSingle();
				return true;
			case JsonValueKind.String when float.TryParse(el.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed):
				value = parsed;
				return true;
			default:
				return false;
		}
	}
}
