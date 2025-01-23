using Godot;
using Project_Ice_Room.Scripts;
using System;

public partial class Player : Creature
{
    private float knockback_timer = 0;

    public override void FlagMan() {
        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (hp <= 0) {
            dead_flag.Update(true);
        }
        if (knockback_timer >= 1) {
            damaged_flag.Update(false);
            knockback_timer = 0;
        }
        if (direction != Vector2.Zero) {
            walk_flag.Update(true);
            idle_flag.Update(false);
        }
        else {
            walk_flag.Update(false);
            idle_flag.Update(true);
        }
    }

	public override void ActionMan() {
        if (dead_flag.just_hoisted) {
            sprite_player.Play("dead");
        }
        if (dead_flag.is_hoisted) {
            Velocity = new(Mathf.MoveToward(Velocity.X, 0, (float)GetPhysicsProcessDeltaTime()*speed), Mathf.MoveToward(Velocity.Y, 0, (float)GetPhysicsProcessDeltaTime()*speed));
            return;
        }
        if (damaged_flag.just_hoisted) {
            hp -= 50;
            GD.Print(hp);
        }
        if (damaged_flag.is_hoisted) {
            knockback_timer += (float)GetPhysicsProcessDeltaTime();
            //KnockBack();
            return;
        }
        if (walk_flag.is_hoisted) {
            Walk();
        }
        else if (idle_flag.just_hoisted) {
            Idle();
        }
    }

    private void Walk() {
        Velocity = direction.Normalized() * speed;
    }
    private void Idle() {
        Velocity = Vector2.Zero;
    }
    private void Attack() {

    }
    private void KnockBack() {
        Velocity -=  (enemy_hitbox.GlobalPosition-this.GlobalPosition) * (1 - knockback_timer);
    }


    public override void _PhysicsProcess(double delta)
	{		
		MoveAndSlide();
	}
}
