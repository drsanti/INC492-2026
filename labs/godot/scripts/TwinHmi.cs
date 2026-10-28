using System;
using System.Collections.Generic;
using Godot;

namespace godot1;

/// <summary>
/// Industrial-style operator HMI overlay.
/// </summary>
public partial class TwinHmi : CanvasLayer
{
	public event Action<string, Dictionary<string, object>>? CommandRequested;

	private static readonly Color BgPanel = new("1B2330");
	private static readonly Color BgCard = new("243041");
	private static readonly Color BgInput = new("141B26");
	private static readonly Color Accent = new("3DBE9F");
	private static readonly Color AccentWarm = new("E6A23C");
	private static readonly Color TextPrimary = new("E8EEF6");
	private static readonly Color TextMuted = new("8B9BB0");
	private static readonly Color Ok = new("3DDC97");
	private static readonly Color Alarm = new("F07178");
	private static readonly Color Border = new("334155");

	private Label _statusDot = null!;
	private Label _statusText = null!;
	private Label _modeBadge = null!;
	private Label _l1Value = null!;
	private Label _l2Value = null!;
	private Label _valveValue = null!;
	private Label _spValue = null!;
	private Label _eventText = null!;
	private LineEdit _valveEdit = null!;
	private LineEdit _spEdit = null!;

	public override void _Ready()
	{
		var root = new Control
		{
			Name = "HmiRoot",
			AnchorRight = 1,
			AnchorBottom = 1,
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		AddChild(root);

		var shell = new PanelContainer
		{
			MouseFilter = Control.MouseFilterEnum.Stop,
			CustomMinimumSize = new Vector2(360, 0)
		};
		shell.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
		shell.Position = new Vector2(20, 20);
		shell.AddThemeStyleboxOverride("panel", MakePanel(BgPanel, 12, 1));
		root.AddChild(shell);

		var outer = new MarginContainer();
		SetMargins(outer, 16);
		shell.AddChild(outer);

		var column = new VBoxContainer();
		column.AddThemeConstantOverride("separation", 14);
		outer.AddChild(column);

		BuildHeader(column);
		BuildMetricGrid(column);
		BuildEventCard(column);
		BuildControls(column);
		BuildFooter(column);
	}

	private void BuildHeader(VBoxContainer column)
	{
		var title = new Label { Text = "COUPLED-TANK TWIN" };
		title.AddThemeFontSizeOverride("font_size", 13);
		title.AddThemeColorOverride("font_color", TextMuted);
		column.AddChild(title);

		var subtitle = new Label { Text = "INC492  ·  Operator HMI" };
		subtitle.AddThemeFontSizeOverride("font_size", 18);
		subtitle.AddThemeColorOverride("font_color", TextPrimary);
		column.AddChild(subtitle);

		var statusRow = new HBoxContainer();
		statusRow.AddThemeConstantOverride("separation", 10);
		column.AddChild(statusRow);

		_statusDot = new Label { Text = "●" };
		_statusDot.AddThemeFontSizeOverride("font_size", 14);
		_statusDot.AddThemeColorOverride("font_color", AccentWarm);
		statusRow.AddChild(_statusDot);

		_statusText = new Label { Text = "Broker connecting…" };
		_statusText.AddThemeFontSizeOverride("font_size", 14);
		_statusText.AddThemeColorOverride("font_color", TextPrimary);
		_statusText.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		statusRow.AddChild(_statusText);

		_modeBadge = new Label { Text = " — " };
		_modeBadge.AddThemeFontSizeOverride("font_size", 12);
		_modeBadge.AddThemeColorOverride("font_color", BgPanel);
		_modeBadge.AddThemeStyleboxOverride("normal", MakeBadge(AccentWarm));
		statusRow.AddChild(_modeBadge);
	}

	private void BuildMetricGrid(VBoxContainer column)
	{
		var section = SectionLabel("PROCESS VALUES");
		column.AddChild(section);

		var grid = new GridContainer { Columns = 2 };
		grid.AddThemeConstantOverride("h_separation", 10);
		grid.AddThemeConstantOverride("v_separation", 10);
		column.AddChild(grid);

		_l1Value = AddMetricCard(grid, "TANK 1 LEVEL", "—", "m");
		_l2Value = AddMetricCard(grid, "TANK 2 LEVEL", "—", "m");
		_valveValue = AddMetricCard(grid, "INLET VALVE", "—", "%");
		_spValue = AddMetricCard(grid, "SETPOINT", "—", "m");
	}

	private void BuildEventCard(VBoxContainer column)
	{
		column.AddChild(SectionLabel("LAST EVENT"));

		var card = new PanelContainer();
		card.AddThemeStyleboxOverride("panel", MakePanel(BgCard, 8, 1));
		column.AddChild(card);

		var margin = new MarginContainer();
		SetMargins(margin, 10);
		card.AddChild(margin);

		_eventText = new Label
		{
			Text = "Waiting for commands…",
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		_eventText.AddThemeFontSizeOverride("font_size", 13);
		_eventText.AddThemeColorOverride("font_color", TextMuted);
		_eventText.CustomMinimumSize = new Vector2(300, 48);
		margin.AddChild(_eventText);
	}

	private void BuildControls(VBoxContainer column)
	{
		column.AddChild(SectionLabel("SUPERVISORY COMMANDS"));

		var card = new PanelContainer();
		card.AddThemeStyleboxOverride("panel", MakePanel(BgCard, 8, 1));
		column.AddChild(card);

		var margin = new MarginContainer();
		SetMargins(margin, 12);
		card.AddChild(margin);

		var box = new VBoxContainer();
		box.AddThemeConstantOverride("separation", 10);
		margin.AddChild(box);

		box.AddChild(FieldLabel("Valve position (%)"));
		_valveEdit = MakeInput("70");
		box.AddChild(_valveEdit);
		var setValve = MakePrimaryButton("Apply valve (manual mode)");
		setValve.Pressed += () =>
		{
			if (float.TryParse(_valveEdit.Text, out var v))
				CommandRequested?.Invoke("set_valve", new Dictionary<string, object> { ["valve_pct"] = v });
		};
		box.AddChild(setValve);

		box.AddChild(FieldLabel("Tank 2 setpoint (m)"));
		_spEdit = MakeInput("1.2");
		box.AddChild(_spEdit);
		var setSp = MakePrimaryButton("Apply setpoint");
		setSp.Pressed += () =>
		{
			if (float.TryParse(_spEdit.Text, out var sp))
				CommandRequested?.Invoke("set_setpoint", new Dictionary<string, object> { ["level_m"] = sp });
		};
		box.AddChild(setSp);

		var modeRow = new HBoxContainer();
		modeRow.AddThemeConstantOverride("separation", 8);
		box.AddChild(modeRow);

		var manual = MakeSecondaryButton("MANUAL");
		manual.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		manual.Pressed += () =>
			CommandRequested?.Invoke("set_mode", new Dictionary<string, object> { ["mode"] = "manual" });
		modeRow.AddChild(manual);

		var auto = MakeAccentButton("AUTO");
		auto.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		auto.Pressed += () =>
			CommandRequested?.Invoke("set_mode", new Dictionary<string, object> { ["mode"] = "auto" });
		modeRow.AddChild(auto);
	}

	private void BuildFooter(VBoxContainer column)
	{
		var note = new Label
		{
			Text = "Python owns process state · Godot sends commands only\n" +
			       "Camera: LMB orbit · MMB/RMB pan · Wheel or +/- zoom"
		};
		note.AddThemeFontSizeOverride("font_size", 11);
		note.AddThemeColorOverride("font_color", TextMuted);
		note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
		column.AddChild(note);
	}

	public void SetConnection(bool ok, string detail)
	{
		_statusDot.AddThemeColorOverride("font_color", ok ? Ok : Alarm);
		_statusText.Text = ok ? $"Broker online · {detail}" : $"Broker offline · {detail}";
		_statusText.AddThemeColorOverride("font_color", ok ? TextPrimary : Alarm);
	}

	public void SetTelemetry(float l1, float l2, float valve, string mode, float sp, bool stale)
	{
		_l1Value.Text = $"{l1:F3}";
		_l2Value.Text = $"{l2:F3}";
		_valveValue.Text = $"{valve:F1}";
		_spValue.Text = $"{sp:F3}";

		var modeText = string.IsNullOrWhiteSpace(mode) ? "—" : mode.ToUpperInvariant();
		_modeBadge.Text = $"  {modeText}  ";
		_modeBadge.AddThemeStyleboxOverride(
			"normal",
			MakeBadge(modeText == "AUTO" ? Accent : AccentWarm));

		if (stale)
		{
			_statusDot.AddThemeColorOverride("font_color", AccentWarm);
			_statusText.Text = "Broker online · DATA STALE";
			_statusText.AddThemeColorOverride("font_color", AccentWarm);
		}
	}

	public void SetEvent(string text)
	{
		_eventText.Text = text;
		_eventText.AddThemeColorOverride("font_color", TextPrimary);
	}

	private Label AddMetricCard(GridContainer grid, string caption, string value, string unit)
	{
		var card = new PanelContainer();
		card.CustomMinimumSize = new Vector2(150, 78);
		card.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		card.AddThemeStyleboxOverride("panel", MakePanel(BgCard, 8, 1));
		grid.AddChild(card);

		var margin = new MarginContainer();
		SetMargins(margin, 10);
		card.AddChild(margin);

		var box = new VBoxContainer();
		box.AddThemeConstantOverride("separation", 4);
		margin.AddChild(box);

		var cap = new Label { Text = caption };
		cap.AddThemeFontSizeOverride("font_size", 11);
		cap.AddThemeColorOverride("font_color", TextMuted);
		box.AddChild(cap);

		var row = new HBoxContainer();
		row.AddThemeConstantOverride("separation", 6);
		box.AddChild(row);

		var valueLabel = new Label { Text = value };
		valueLabel.AddThemeFontSizeOverride("font_size", 24);
		valueLabel.AddThemeColorOverride("font_color", TextPrimary);
		row.AddChild(valueLabel);

		var unitLabel = new Label { Text = unit };
		unitLabel.AddThemeFontSizeOverride("font_size", 12);
		unitLabel.AddThemeColorOverride("font_color", Accent);
		unitLabel.VerticalAlignment = VerticalAlignment.Bottom;
		row.AddChild(unitLabel);

		return valueLabel;
	}

	private static Label SectionLabel(string text)
	{
		var label = new Label { Text = text };
		label.AddThemeFontSizeOverride("font_size", 11);
		label.AddThemeColorOverride("font_color", Accent);
		return label;
	}

	private static Label FieldLabel(string text)
	{
		var label = new Label { Text = text };
		label.AddThemeFontSizeOverride("font_size", 12);
		label.AddThemeColorOverride("font_color", TextMuted);
		return label;
	}

	private static LineEdit MakeInput(string text)
	{
		var edit = new LineEdit { Text = text };
		edit.AddThemeStyleboxOverride("normal", MakePanel(BgInput, 6, 1));
		edit.AddThemeStyleboxOverride("focus", MakePanel(BgInput, 6, 1, Accent));
		edit.AddThemeColorOverride("font_color", TextPrimary);
		edit.AddThemeColorOverride("font_placeholder_color", TextMuted);
		edit.CustomMinimumSize = new Vector2(0, 34);
		return edit;
	}

	private static Button MakePrimaryButton(string text)
	{
		var button = new Button { Text = text };
		button.CustomMinimumSize = new Vector2(0, 36);
		button.AddThemeStyleboxOverride("normal", MakePanel(Accent, 6, 0));
		button.AddThemeStyleboxOverride("hover", MakePanel(new Color(Accent.R, Accent.G, Accent.B, 0.85f), 6, 0));
		button.AddThemeStyleboxOverride("pressed", MakePanel(new Color(Accent.R * 0.85f, Accent.G * 0.85f, Accent.B * 0.85f), 6, 0));
		button.AddThemeColorOverride("font_color", BgPanel);
		button.AddThemeColorOverride("font_hover_color", BgPanel);
		button.AddThemeColorOverride("font_pressed_color", BgPanel);
		return button;
	}

	private static Button MakeSecondaryButton(string text)
	{
		var button = new Button { Text = text };
		button.CustomMinimumSize = new Vector2(0, 34);
		button.AddThemeStyleboxOverride("normal", MakePanel(BgInput, 6, 1));
		button.AddThemeStyleboxOverride("hover", MakePanel(BgInput, 6, 1, Accent));
		button.AddThemeStyleboxOverride("pressed", MakePanel(Border, 6, 1));
		button.AddThemeColorOverride("font_color", TextPrimary);
		return button;
	}

	private static Button MakeAccentButton(string text)
	{
		var button = new Button { Text = text };
		button.CustomMinimumSize = new Vector2(0, 34);
		button.AddThemeStyleboxOverride("normal", MakePanel(AccentWarm, 6, 0));
		button.AddThemeStyleboxOverride("hover", MakePanel(new Color(AccentWarm.R, AccentWarm.G, AccentWarm.B, 0.9f), 6, 0));
		button.AddThemeStyleboxOverride("pressed", MakePanel(new Color(AccentWarm.R * 0.9f, AccentWarm.G * 0.9f, AccentWarm.B * 0.9f), 6, 0));
		button.AddThemeColorOverride("font_color", BgPanel);
		button.AddThemeColorOverride("font_hover_color", BgPanel);
		button.AddThemeColorOverride("font_pressed_color", BgPanel);
		return button;
	}

	private static StyleBoxFlat MakePanel(Color bg, int radius, int borderWidth, Color? borderColor = null)
	{
		return new StyleBoxFlat
		{
			BgColor = bg,
			CornerRadiusTopLeft = radius,
			CornerRadiusTopRight = radius,
			CornerRadiusBottomLeft = radius,
			CornerRadiusBottomRight = radius,
			BorderWidthLeft = borderWidth,
			BorderWidthTop = borderWidth,
			BorderWidthRight = borderWidth,
			BorderWidthBottom = borderWidth,
			BorderColor = borderColor ?? Border,
			ContentMarginLeft = 8,
			ContentMarginRight = 8,
			ContentMarginTop = 6,
			ContentMarginBottom = 6
		};
	}

	private static StyleBoxFlat MakeBadge(Color bg)
	{
		var box = MakePanel(bg, 10, 0);
		box.ContentMarginLeft = 8;
		box.ContentMarginRight = 8;
		box.ContentMarginTop = 3;
		box.ContentMarginBottom = 3;
		return box;
	}

	private static void SetMargins(MarginContainer margin, int value)
	{
		margin.AddThemeConstantOverride("margin_left", value);
		margin.AddThemeConstantOverride("margin_right", value);
		margin.AddThemeConstantOverride("margin_top", value);
		margin.AddThemeConstantOverride("margin_bottom", value);
	}
}
