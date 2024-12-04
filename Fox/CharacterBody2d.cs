using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private AnimatedSprite2D _spriteplayer;


    public override void _Ready() {
        base._Ready();

		_spriteplayer = GetNode<AnimatedSprite2D>("FoxAnimator");
    }


    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }

		Velocity = velocity;
		MoveAndSlide();

		AnimationHandler();

        void AnimationHandler() {


            if (velocity == Vector2.Zero) {
				_spriteplayer.Play("fox_idle");
			}
			else {
				_spriteplayer.Play("fox_walk");
			}

		}
    }
}


public partial class EnemyAI : Node {

	bool sees_player = false;


	EnemyAI() { 
		
		
	}



	void Update() {

	}

	void LookForPlayer() {
		if (!sees_player) {
			try {
				var field_of_view = GetNode<Area2D>("FieldOfView");
				field_of_view.BodyEntered += (what_entered) => sees_player = true;
            }
			catch { 
				
			}
		}
	}
	void WalkTowardsPlayer() {
		if (sees_player) {

		}
	}
	



}
