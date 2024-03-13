using System;
using Godot;

public partial class ScoreLabel : Label
{
	private int _coins;
	private int _totalCoins;

	public override void _Ready()
	{
		Text = _coins.ToString();
	}

	private void OnCoinCollected()
	{
		_coins++;
		_Ready();
		if (_coins == _totalCoins)
		{
			GetNode<Timer>("Timer").Start();
		}
	}

	private void OnStartCoins(long count)
	{
		_totalCoins = (int)count;
		Console.WriteLine(_totalCoins);

	}

	private void OnTimerTimeout()
	{
		GetTree().ChangeSceneToFile("./YouWin.tscn");
	}

}


