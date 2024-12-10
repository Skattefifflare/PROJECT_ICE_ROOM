using Godot;
using Project_Ice_Room.Scriptbin;
using System;

public partial class movement : KillableThing
{
	public const float Speed = 100.0f;

	public Area2D hitbox;


    public override void _Ready() {
        base._Ready();
		hitbox = (Area2D)FindChild("hitbox");

		dmgh = new DamageHandler(hitbox, 100, ref taken_dmg_flag, ref death_flag);
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
	}
}
