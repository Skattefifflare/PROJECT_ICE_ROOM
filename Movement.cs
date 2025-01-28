using Godot;
using System;

public partial class Movement : CharacterBody2D
{
	private Vector2 new_velocity;
	private int speed = 420/8;
	private bool facing_right = true;

	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("Sprite_Sheet");
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		GetDirection();
		Velocity = new_velocity;
		MoveAndSlide();

		if (Velocity.X != 0 || Velocity.Y != 0)
		{
			Filip();
			_animatedSprite.Play("RUNNIN");
		}
		else 
		{
			_animatedSprite.Play("standim stimll");
		}
	}

	//Check if sprite should be facing left or right and scale sprite accordingly
	private void Filip()
	{
		if (Velocity.X > 0 && !facing_right)
		{
			Scale = new Vector2(-1, Scale.Y);
			facing_right = true;
		}
		else if (Velocity.X < 0 && facing_right)
		{
			Scale = new Vector2(-1, Scale.Y);
			facing_right = false;
		}
	}

	private void GetDirection()
	{
		new_velocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		new_velocity *= speed;
	}
}
