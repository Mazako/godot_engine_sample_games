using Godot;
using System;

public partial class GameOver : Control
{
	private void OnButtonPressed()
	{
		GetTree().ChangeSceneToFile("./MainMenu.tscn");
	}

}


