using Godot;
using System;

public partial class Main : Node
{
	[Export] public PackedScene MobScene { get; set; }
	[Export] public PackedScene PowerBallScene { get; set; }

	private int _score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	
	private void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Timer>("PowerBallTimer").Stop();
		
		GetTree().CallGroup("powerBalls", Node.MethodName.QueueFree);
		
		GetNode<HUD>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	public void NewGame()
	{
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");

		GetNode<Timer>("StartTimer").Start();
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		GetNode<AudioStreamPlayer>("Music").Play();
		
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Timer>("PowerBallTimer").Start();
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	private void OnMobTimerTimeout()
	{
		var mob = MobScene.Instantiate<Mob>();

		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		mob.Position = mobSpawnLocation.Position;

		direction += (float)GD.RandRange(-Mathf.Pi / 4, Math.PI / 4);
		mob.Rotation = direction;

		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
		
		AddChild(mob);
	}

	private void OnPowerBallTimerTimeout()
	{
		var ball = PowerBallScene.Instantiate<PowerBall>();

		ball.Position = new Vector2((float)GD.RandRange(0, GetViewport().GetVisibleRect().Size.X),
			(float)GD.RandRange(0, GetViewport().GetVisibleRect().Size.Y));
		
		AddChild(ball);
	}

	private void OnKillMob()
	{
		GetNode<AudioStreamPlayer>("MobDeathSound").Play();
	}

	private void OnImmortalModeStart(double time)
	{
		GetTree().CallGroup("powerBalls", Node.MethodName.QueueFree);
		var timer = GetNode<Timer>("ImmortalModeTimer");
		var player = GetNode<AudioStreamPlayer>("ImmortalModeSound");

		timer.WaitTime = time;
		player.Play();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<Timer>("PowerBallTimer").Stop();
		timer.Start();
		GetNode<HUD>("HUD").StartProgress(time);
	}

	private void OnImmortalModeTimeout()
	{
		GetNode<AudioStreamPlayer>("ImmortalModeSound").Stop();
		GetNode<Timer>("PowerBallTimer").Start();
		GetNode<AudioStreamPlayer>("Music").Play();

	}

}
