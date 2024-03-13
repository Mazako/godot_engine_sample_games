using Godot;
using System;

public partial class MainMenu : Control
{
	private void OnButtonTitlePlayPressed()
	{
		GetTree().ChangeSceneToFile("./Level.tscn");
	}
}
