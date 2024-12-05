using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AI : CharacterBody2D
{
	public const float SPEED = 300.0f;


	CollisionShape2D VIEW_FIELD;
	AnimatedSprite2D SPRITE_PLAYER;

	Dictionary<string, CollisionPolygon2D> HITBOX_DICT;


    public override void _Ready() {
        base._Ready();	
        VIEW_FIELD = (CollisionShape2D)FindChild("view_field");
		SPRITE_PLAYER = (AnimatedSprite2D)FindChild("sprite_player");

		HITBOX_DICT = new Dictionary<string, CollisionPolygon2D>();
		foreach (var hitbox in GetChildren().OfType<CollisionPolygon2D>().Where(child => child.Name.ToString().StartsWith("hitbox_"))) {
			hitbox.Disabled = true;
			HITBOX_DICT[hitbox.Name.ToString().Substring(7)] = hitbox;
		}

    }


    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * SPEED;
            velocity.Y = direction.Y * SPEED;
        }
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, SPEED);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, SPEED);
        }
		Velocity = velocity;
		MoveAndSlide();
	}


	void DecideState() {

	}



	void SetStateTo(string state) {
		SPRITE_PLAYER.Play(state);
		HITBOX_DICT[state].Disabled = false;
	}
}
