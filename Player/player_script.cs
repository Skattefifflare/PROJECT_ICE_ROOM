using Godot;
using Project_Ice_Room.Scriptbin;
using System;

public partial class player_script : KillableThing
{
	public float speed = 100.0f;


	// the player needs to communicate with its weapon.
	int spear_dmg = 40;
	Area2D dmg_box;

    public override void _Ready() {
		hp = 100; // hp must be declared before base. yes it is dumb but i dont wanna learn dependency injection
        base._Ready();
		hitbox = (Area2D)FindChild("hitbox");

		dmg_box = (Area2D)FindChild("damagebox");
    }


    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
            velocity.Y = direction.Y * speed;
        }
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
        }

		Velocity = velocity;
		MoveAndSlide();
	}


	private void Attack() {
		gm.PlayerAttacksEnemy(dmg_box, spear_dmg);
		// attack animation here
		gm.PlayerStopsAttack();
	}
}
