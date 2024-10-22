using Godot;
using System;

public partial class Dash : CharacterBody2D
{
	private Vector2 dashVelocity;
	private int dashSpeed = 10;

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Input.IsActionPressed("ui_dash") && Velocity.X != 0 || Velocity.Y != 0)
		{
			dashVelocity = Velocity * dashSpeed;
			Velocity = dashVelocity;
		}
	}
}
