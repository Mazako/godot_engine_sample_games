using System;
using Godot;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitByMobEventHandler();

	[Signal]
	public delegate void KillMobEventHandler();

	[Signal]
	public delegate void ImmortalModeEventHandler(double time);

	[Export] public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;
	private bool _immortal = false;

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			Mathf.Clamp(Position.X, 0, ScreenSize.X),
			Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = _immortal ? "walk_immortal" : "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = _immortal ? "up_immortal" : "up";
			animatedSprite2D.FlipV = velocity.Y > 0;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (!_immortal)
		{
			Hide();
			EmitSignal(SignalName.HitByMob);

			GetNode<CollisionShape2D>("CollisionShape2D")
				.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
		else
		{
			EmitSignal(SignalName.KillMob);
			body.QueueFree();
		}
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void OnAreaEntered(Area2D area)
	{
		Console.WriteLine("START");
		area.QueueFree();
		_immortal = true;
		var timer = GetNode<Timer>("ImmortalTimer");
		EmitSignal(SignalName.ImmortalMode, timer.WaitTime);
		timer.Start();
	}

	private void OnImmortalTimerTimeout()
	{
		Console.WriteLine("KONIEC");
		_immortal = false;
	}
}
