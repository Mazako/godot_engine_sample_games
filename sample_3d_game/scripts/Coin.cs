using Godot;
using System;

public partial class Coin : Area3D
{
	[Signal]
	public delegate void CoinCollectedEventHandler();
	public override void _PhysicsProcess(double delta)
	{
		RotateY(Mathf.DegToRad(3));
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if (body.Name == "Steve")
		{
			GetNode<Timer>("Timer").Start();
			GetNode<AnimationPlayer>("AnimationPlayer").Play("bounce");
		}
	}

	private void OnTimerTimeout()
	{
		EmitSignal("CoinCollected");
		QueueFree();
	}

}
