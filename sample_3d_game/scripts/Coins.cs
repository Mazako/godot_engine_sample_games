using Godot;
using System;

public partial class Coins : Node3D
{
	[Signal]
	public delegate void StartCoinsEventHandler(int count);
	public override void _Ready()
	{
		EmitSignal("StartCoins", GetNode<Node3D>("CoinHolder").GetChildCount());
	}

}
