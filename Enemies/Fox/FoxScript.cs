using Godot;
using System;

public partial class FoxScript : CharacterBody2D
{
	public const float Speed = 300.0f;

	private AnimatedSprite2D _spriteplayer;
	EnemyAI fox_AI;
    bool is_attacking = false;

    public override void _Ready() {
        base._Ready();
		fox_AI = new EnemyAI(this);

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
    }
    void AnimationHandler() {
        if (Velocity.X > 0) {
            _spriteplayer.FlipH = true;
        }
        if (Velocity.X < 0) {
            _spriteplayer.FlipH = false;
        }
        if (fox_AI.sees_player) {
            if (!is_attacking) {
                is_attacking = true;
                _spriteplayer.Play("fox_attack");
            }
            if (!_spriteplayer.IsPlaying()) {
                is_attacking= false;
            }
        }
        else if (Velocity != Vector2.Zero) {
            _spriteplayer.Play("fox_walk");
        }
        else {
            _spriteplayer.Play("fox_idle");
        }
    }
}


