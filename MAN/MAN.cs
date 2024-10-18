using Godot;
using System;

public partial class MAN : CharacterBody2D
{
	private Vector2 newVelocity;
	private int speed = 300;

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		GetDirection();
		Velocity = newVelocity;
		MoveAndSlide();
	}

	private void GetDirection()
	{
		newVelocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		newVelocity *= speed;
	}
}
