using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

namespace godotsimple;

/// <summary>
/// Smoothly applies position / rotation / scale from MQTT topic <c>twin/transform</c>.
/// Expected JSON (all fields optional; omitted values keep the current target):
/// <code>
/// {
///   "position": {"x": 1, "y": 0.5, "z": 2},
///   "rotation": {"x": 0, "y": 45, "z": 0},
///   "scale":    {"x": 1.2, "y": 1.2, "z": 1.2}
/// }
/// </code>
/// Arrays also work: <c>"position": [1, 0.5, 2]</c>.
/// Rotation is Euler degrees. Aliases: <c>pos</c>/<c>rot</c>/<c>orientation</c>.
/// </summary>
public partial class BoxTransform : MeshInstance3D
{
	[Export] public string BrokerHost { get; set; } = "localhost";
	[Export] public int BrokerPort { get; set; } = 1883;
	[Export] public string TransformTopic { get; set; } = "twin/transform";

	/// <summary>Higher = reaches the target faster. ~4–10 feels natural.</summary>
	[Export] public float SmoothSpeed { get; set; } = 6f;

	private SimpleMqttClient _mqtt = null!;

	private Vector3 _targetPosition;
	private Vector3 _targetDegrees;
	private Quaternion _targetRotation = Quaternion.Identity;
	private Vector3 _targetScale = Vector3.One;

	public override void _Ready()
	{
		_targetPosition = Position;
		_targetDegrees = RotationDegrees;
		_targetRotation = Quaternion;
		_targetScale = Scale;

		_mqtt = new SimpleMqttClient
		{
			Name = "SimpleMqttClient",
			BrokerHost = BrokerHost,
			BrokerPort = BrokerPort
		};
		AddChild(_mqtt);

		_mqtt.ConnectionChanged += (ok, detail) =>
			GD.Print(ok
				? $"[transform] MQTT {detail} — subscribed to {TransformTopic}"
				: $"[transform] MQTT {detail}");

		_mqtt.MessageReceived += OnMqttMessage;
		_ = ConnectAsync();
	}

	public override void _Process(double delta)
	{
		var t = 1f - Mathf.Exp(-SmoothSpeed * (float)delta);

		Position = Position.Lerp(_targetPosition, t);
		Scale = Scale.Lerp(_targetScale, t);

		if (Quaternion.AngleTo(_targetRotation) < 0.001f)
			Quaternion = _targetRotation;
		else
			Quaternion = Quaternion.Slerp(_targetRotation, t);

		// Snap leftovers when very close (avoids endless micro-motion).
		if (Position.DistanceTo(_targetPosition) < 0.0005f)
			Position = _targetPosition;
		if (Scale.DistanceTo(_targetScale) < 0.0005f)
			Scale = _targetScale;
	}

	private async Task ConnectAsync()
	{
		try
		{
			await _mqtt.ConnectAsync(TransformTopic);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"[transform] MQTT connect failed: {ex.Message}");
			GD.PrintErr("[transform] Start Lab 01 broker (docker), then restart this scene.");
		}
	}

	private void OnMqttMessage(string topic, string payload)
	{
		if (topic != TransformTopic)
			return;
		ApplyTransformPayload(payload);
	}

	private void ApplyTransformPayload(string text)
	{
		try
		{
			using var doc = JsonDocument.Parse(text);
			var root = doc.RootElement;
			if (root.ValueKind != JsonValueKind.Object)
			{
				GD.PrintErr("[transform] twin/transform expects a JSON object");
				return;
			}

			if (TryGetVector3(root, out var pos, "position", "pos"))
				_targetPosition = pos;

			if (TryGetVector3(root, out var rot, "rotation", "rot", "orientation"))
			{
				_targetDegrees = rot;
				_targetRotation = Quaternion.FromEuler(rot * Mathf.Pi / 180f);
			}

			if (TryGetVector3(root, out var scale, "scale"))
				_targetScale = scale;

			GD.Print(
				$"[transform] target pos=({_targetPosition.X:0.###}, {_targetPosition.Y:0.###}, {_targetPosition.Z:0.###}) " +
				$"rot=({_targetDegrees.X:0.#}, {_targetDegrees.Y:0.#}, {_targetDegrees.Z:0.#}) " +
				$"scale=({_targetScale.X:0.###}, {_targetScale.Y:0.###}, {_targetScale.Z:0.###})");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"[transform] twin/transform bad payload: {ex.Message} | raw={text}");
		}
	}

	private static bool TryGetVector3(JsonElement root, out Vector3 value, params string[] names)
	{
		value = Vector3.Zero;
		foreach (var name in names)
		{
			if (!root.TryGetProperty(name, out var el))
				continue;

			if (el.ValueKind == JsonValueKind.Object)
			{
				var v = Vector3.Zero;
				if (TryGetFloat(el, "x", out var x)) v.X = x;
				if (TryGetFloat(el, "y", out var y)) v.Y = y;
				if (TryGetFloat(el, "z", out var z)) v.Z = z;
				value = v;
				return true;
			}

			if (el.ValueKind == JsonValueKind.Array && el.GetArrayLength() >= 3)
			{
				value = new Vector3(
					el[0].GetSingle(),
					el[1].GetSingle(),
					el[2].GetSingle());
				return true;
			}
		}

		return false;
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
