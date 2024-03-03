using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	private double _immortalTime;

	public void StartProgress(double time)
	{
		var progressBar = GetNode<ProgressBar>("ProgressBar");
		progressBar.Value = time;
		progressBar.MaxValue = time;
		progressBar.Visible = true;
		_immortalTime = time;

	}
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		
		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		var message = GetNode<Label>("Message");
		message.Text = "Dodge the Creeps!";
		message.Show();

		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}

	private void OnProgressTimerTimeout()
	{
		var progressBar = GetNode<ProgressBar>("ProgressBar");
		if (_immortalTime >= 0)
		{
			var timer = GetNode<Timer>("ProgressTimer");
			_immortalTime -= timer.WaitTime;
			progressBar.Value = _immortalTime;

		}
		else
		{
			progressBar.Visible = false;
		}
	}
	
	
}
