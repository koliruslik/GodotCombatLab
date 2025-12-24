using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	[ExportGroup("Scenes")]
	[Export] public PackedScene Opt1Scene;
	[Export] public PackedScene Opt2Scene;
	[Export] public PackedScene Opt3Scene;
	
	[ExportGroup("Buttons")]
	[Export] public Button[] Buttons;
	
	[ExportGroup("Options")]
	[Export] public float TweenAmount = 1.1f;
	[Export] public float Duration = 0.1f;
	
	
	public override void _Ready()
	{
		if (Buttons == null) return;
		foreach (var btn in Buttons)
		{
			
			
			btn.MouseEntered += () => OnButtonHover(btn, true);
			btn.MouseExited += () => OnButtonHover(btn, false);
		}
	}

	private void OnButtonHover(Button btn, bool isHovered)
	{
		btn.PivotOffset = btn.Size / 2;
		if (!GodotObject.IsInstanceValid(btn)) return;

		var targetScale = isHovered ? Vector2.One * TweenAmount : Vector2.One;
		
		var tween = CreateTween();

		tween.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(btn, "scale", targetScale, Duration);
	}

	

	private void OnOptionOneBtnPressed()
	{
		if(Opt1Scene != null)
			GetTree().ChangeSceneToPacked(Opt1Scene);
	}

	private void OnOptionTwoBtnPressed()
	{
		if(Opt2Scene != null)
			GetTree().ChangeSceneToPacked(Opt2Scene);
	}

	private void OnOptionThreeBtnPressed()
	{
		if (Opt3Scene != null)
			GetTree().ChangeSceneToPacked(Opt3Scene);
	}

	private void OnExitBtnPressed()
	{
		GetTree().Quit();
	}
}
