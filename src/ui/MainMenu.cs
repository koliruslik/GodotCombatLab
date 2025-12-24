using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] public PackedScene Opt1Scene;
	[Export] public PackedScene Opt2Scene;
	[Export] public PackedScene Opt3Scene;
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
}
