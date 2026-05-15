using System.Collections.Generic;
using CombatLab.Core.Events;
using Godot;
using CombatLab.Core.Utils;
using Vector2 = Godot.Vector2;

namespace CombatLab.Presentation.UI.MainMenu;
public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	
	[ExportGroup("Buttons")]
	[Export] public Button StartGameBtn;
	[Export] public Button ExitGameBtn;
	[Export] public Button SettingsBtn;
	
	[ExportGroup("Options")]
	[Export] public float TweenAmount = 1.1f;
	[Export] public float Duration = 0.1f;
	
	private List<Button> _buttons;
	private readonly Dictionary<Button, Tween> _tweens = new();
	
	public override void _Ready()
	{
		_buttons = new List<Button>
		{
			StartGameBtn,
			ExitGameBtn,
			SettingsBtn
		};

		if (_buttons.Count == 0)
		{
			GameLogger.Error("No buttons added");
			return;
		}

		foreach (var btn in _buttons)
		{
			if(btn == null)
				continue;

			var capturedButton = btn;
			
			capturedButton.MouseEntered += () => OnButtonHover(capturedButton, true);
			capturedButton.MouseExited += () => OnButtonHover(capturedButton, false);
		}

		StartGameBtn.Pressed  += EventBus.PublishUIStartGameClicked;
		SettingsBtn.Pressed += EventBus.PublishUISettingsClicked;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		StartGameBtn.Pressed  -= EventBus.PublishUIStartGameClicked;
		SettingsBtn.Pressed -= EventBus.PublishUISettingsClicked;
	}

	private void OnButtonHover(Button btn, bool isHovered)
	{
		btn.PivotOffset = btn.Size / 2;

		if (_tweens.TryGetValue(btn, out var existingTween))
		{
			existingTween.Kill();
		}
		
		var targetScale = isHovered 
			? Vector2.One * TweenAmount 
			: Vector2.One;
		
		var tween = CreateTween();

		_tweens[btn] = tween;
		
		tween.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
		tween.TweenProperty(btn, "scale", targetScale, Duration);
	}
}
