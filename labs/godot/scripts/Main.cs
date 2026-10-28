using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

namespace godot1;

/// <summary>
/// Lab 03 entry: builds twin world + HMI and connects MQTT.
/// </summary>
public partial class Main : Node3D
{
	private MqttTwinClient _mqtt = null!;
	private TwinWorld _world = null!;
	private TwinHmi _hmi = null!;

	private float _level1 = 0.4f;
	private float _level2 = 0.3f;
	private float _valve = 40f;
	private float _setpoint = 0.8f;
	private string _mode = "manual";
	private bool _stale = true;
	private ulong _lastTelemetryMs;

	public override void _Ready()
	{
		_world = new TwinWorld { Name = "TwinWorld" };
		AddChild(_world);

		_mqtt = new MqttTwinClient { Name = "MqttTwinClient" };
		AddChild(_mqtt);

		_hmi = new TwinHmi { Name = "TwinHmi" };
		AddChild(_hmi);

		_mqtt.TelemetryReceived += OnTelemetry;
		_mqtt.EventReceived += OnEvent;
		_mqtt.StatusReceived += OnStatus;
		_mqtt.ConnectionChanged += (ok, detail) => _hmi.SetConnection(ok, detail);
		_hmi.CommandRequested += OnCommandRequested;

		_ = ConnectMqtt();
	}

	public override void _Process(double delta)
	{
		var now = Time.GetTicksMsec();
		if (_lastTelemetryMs > 0 && now - _lastTelemetryMs > 2000)
		{
			if (!_stale)
			{
				_stale = true;
				_world.ApplyTelemetry(_level1, _level2, _valve, _setpoint, true);
				_hmi.SetTelemetry(_level1, _level2, _valve, _mode, _setpoint, true);
			}
		}
	}

	private async Task ConnectMqtt()
	{
		try
		{
			await _mqtt.ConnectAsync();
			GD.Print("[twin] MQTT connected");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"[twin] MQTT connect failed: {ex.Message}");
			_hmi.SetConnection(false, ex.Message);
			_hmi.SetEvent("Start Lab 01 + Lab 02, then restart this scene.");
		}
	}

	private void OnTelemetry(JsonElement root)
	{
		if (!root.TryGetProperty("payload", out var payload))
			return;

		_level1 = GetFloat(payload, "level1_m", _level1);
		_level2 = GetFloat(payload, "level2_m", _level2);
		_valve = GetFloat(payload, "valve_pct", _valve);
		_setpoint = GetFloat(payload, "setpoint_m", _setpoint);
		if (payload.TryGetProperty("controller_mode", out var modeEl))
			_mode = modeEl.GetString() ?? _mode;

		_stale = false;
		_lastTelemetryMs = Time.GetTicksMsec();
		_world.ApplyTelemetry(_level1, _level2, _valve, _setpoint, false);
		_hmi.SetTelemetry(_level1, _level2, _valve, _mode, _setpoint, false);
	}

	private void OnEvent(JsonElement root)
	{
		if (!root.TryGetProperty("payload", out var payload))
			return;
		var result = payload.TryGetProperty("result", out var r) ? r.GetString() : "?";
		var reason = payload.TryGetProperty("reason", out var reasonEl) ? reasonEl.GetString() : "";
		var id = payload.TryGetProperty("command_id", out var idEl) ? idEl.GetString() : "";
		_hmi.SetEvent($"{result}: {reason} ({id})");
	}

	private void OnStatus(JsonElement root)
	{
		if (!root.TryGetProperty("payload", out var payload))
			return;
		if (payload.TryGetProperty("state", out var state) && state.GetString() == "offline")
		{
			_stale = true;
			_world.ApplyTelemetry(_level1, _level2, _valve, _setpoint, true);
			_hmi.SetTelemetry(_level1, _level2, _valve, _mode, _setpoint, true);
			_hmi.SetEvent("simulator status: offline");
		}
	}

	private void OnCommandRequested(string action, Dictionary<string, object> parameters)
	{
		var id = $"cmd-godot-{DateTime.UtcNow:HHmmss}";
		_ = Publish(action, parameters, id);
	}

	private async Task Publish(string action, Dictionary<string, object> parameters, string id)
	{
		try
		{
			await _mqtt.PublishCommandAsync(action, parameters, id);
			_hmi.SetEvent($"sent {action} ({id})");
		}
		catch (Exception ex)
		{
			_hmi.SetEvent($"send failed: {ex.Message}");
		}
	}

	private static float GetFloat(JsonElement obj, string name, float fallback)
	{
		if (!obj.TryGetProperty(name, out var el))
			return fallback;
		return el.ValueKind switch
		{
			JsonValueKind.Number => el.GetSingle(),
			JsonValueKind.String when float.TryParse(el.GetString(), out var v) => v,
			_ => fallback
		};
	}
}
