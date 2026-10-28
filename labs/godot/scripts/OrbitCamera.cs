using Godot;

namespace godot1;

/// <summary>
/// Orbit / pan / zoom camera for inspecting the twin.
/// Left-drag: orbit · Middle or Right-drag: pan · Wheel: zoom
/// </summary>
public partial class OrbitCamera : Node3D
{
	[Export] public float Distance { get; set; } = 7.5f;
	[Export] public float MinDistance { get; set; } = 2.5f;
	[Export] public float MaxDistance { get; set; } = 18f;
	[Export] public float OrbitSensitivity { get; set; } = 0.35f;
	[Export] public float PanSensitivity { get; set; } = 0.0025f;
	[Export] public float ZoomSensitivity { get; set; } = 0.85f;

	private Camera3D _camera = null!;
	private float _yaw = 40f;
	private float _pitch = -28f;
	private bool _orbiting;
	private bool _panning;

	public override void _Ready()
	{
		Position = new Vector3(0f, 1.0f, 0f);

		_camera = new Camera3D
		{
			Name = "TwinCamera",
			Fov = 42
		};
		AddChild(_camera);
		_camera.Current = true;
		ApplyTransform();
	}

	public override void _Input(InputEvent @event)
	{
		// Ignore camera gestures when the pointer is over the HMI panel.
		if (IsPointerOverGui())
		{
			_orbiting = false;
			_panning = false;
			return;
		}

		if (@event is InputEventMouseButton mouseButton)
		{
			switch (mouseButton.ButtonIndex)
			{
				case MouseButton.Left:
					_orbiting = mouseButton.Pressed;
					GetViewport().SetInputAsHandled();
					break;
				case MouseButton.Middle:
				case MouseButton.Right:
					_panning = mouseButton.Pressed;
					GetViewport().SetInputAsHandled();
					break;
				case MouseButton.WheelUp when mouseButton.Pressed:
					Zoom(-ZoomSensitivity);
					GetViewport().SetInputAsHandled();
					break;
				case MouseButton.WheelDown when mouseButton.Pressed:
					Zoom(ZoomSensitivity);
					GetViewport().SetInputAsHandled();
					break;
			}
		}
		else if (@event is InputEventMouseMotion motion)
		{
			if (_orbiting)
			{
				_yaw -= motion.Relative.X * OrbitSensitivity;
				_pitch -= motion.Relative.Y * OrbitSensitivity;
				_pitch = Mathf.Clamp(_pitch, -80f, -5f);
				ApplyTransform();
				GetViewport().SetInputAsHandled();
			}
			else if (_panning)
			{
				var right = _camera.GlobalTransform.Basis.X;
				var up = _camera.GlobalTransform.Basis.Y;
				// Scale lightly with distance so pan stays controllable when zoomed out.
				var panScale = PanSensitivity * Mathf.Sqrt(Distance);
				Position -= right * motion.Relative.X * panScale;
				Position += up * motion.Relative.Y * panScale;
				ApplyTransform();
				GetViewport().SetInputAsHandled();
			}
		}
		else if (@event is InputEventKey key && key.Pressed && !key.Echo)
		{
			// Keyboard zoom fallback (trackpads / no wheel).
			if (key.Keycode == Key.Equal || key.Keycode == Key.KpAdd)
			{
				Zoom(-ZoomSensitivity);
				GetViewport().SetInputAsHandled();
			}
			else if (key.Keycode == Key.Minus || key.Keycode == Key.KpSubtract)
			{
				Zoom(ZoomSensitivity);
				GetViewport().SetInputAsHandled();
			}
		}
	}

	private void Zoom(float delta)
	{
		Distance = Mathf.Clamp(Distance + delta, MinDistance, MaxDistance);
		ApplyTransform();
	}

	private bool IsPointerOverGui()
	{
		var hovered = GetViewport().GuiGetHoveredControl();
		return hovered is not null;
	}

	private void ApplyTransform()
	{
		var yawRad = Mathf.DegToRad(_yaw);
		var pitchRad = Mathf.DegToRad(_pitch);
		var offset = new Vector3(
			Mathf.Cos(pitchRad) * Mathf.Sin(yawRad),
			-Mathf.Sin(pitchRad),
			Mathf.Cos(pitchRad) * Mathf.Cos(yawRad)
		) * Distance;

		_camera.Position = offset;
		_camera.LookAt(GlobalPosition, Vector3.Up);
	}
}
