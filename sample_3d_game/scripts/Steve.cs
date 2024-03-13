using System;
using Godot;

public partial class Steve : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 5;
	
	[Export]
	public int RotationSpeed { get; set; } = 6;
	private Vector3 _velocity = new Vector3(0, 0, 0);

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		var mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_left"))
		{
			_velocity.X = 0;
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			_velocity.X = Speed;
			mesh.RotateZ(Mathf.DegToRad(-RotationSpeed));
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			_velocity.X = -Speed;
			mesh.RotateZ(Mathf.DegToRad(RotationSpeed));
		}
		else
		{
			_velocity.X = (float) Mathf.Lerp(_velocity.X, 0, 0.1);
		}


		if (Input.IsActionPressed("ui_up") && Input.IsActionPressed("ui_down"))
		{
			_velocity.Z = 0;
		}
		else if (Input.IsActionPressed("ui_up"))
		{
			_velocity.Z = -Speed;
			mesh.RotateX(Mathf.DegToRad(-RotationSpeed));
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			_velocity.Z = Speed;
			mesh.RotateX(Mathf.DegToRad(RotationSpeed));
		}
		else
		{
			_velocity.Z = (float)Mathf.Lerp(_velocity.Z, 0, 0.1);
		}

		Velocity = _velocity;
		MoveAndSlide();
	}
	private void OnEnemyBodyEntered(Node3D body)
	{
		if (body == this)
		{
			GetTree().ChangeSceneToFile("./GameOver.tscn");
		}
		
	}

}
