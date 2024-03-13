using Godot;
using System;

public partial class YouWin : Control
{
	private void OnButtonPressed()
	{
		GetTree().ChangeSceneToFile("./Level.tscn");
	}

}
