using Godot;
using Project_Ice_Room.Scripts;
using System;
using System.Collections.Generic;

public partial class Player : Creature
{
    private float knockback_timer = 0;

    Dictionary<string, Flag> player_flags;

    public override void _Ready() {
        base._Ready();        
    }

    public override void FlagMan() {
        

        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");


        if (hp <= 0) {
            flags["dead"].Set(true);
        }
        if (knockback_timer >= 0.5f) {
            flags["damaged"].Set(false);
            knockback_timer = 0;
            flags["idle"].Set(true);
        }
        if (direction != Vector2.Zero) {
            flags["walk"].Set(true);
            flags["idle"].Set(false);
        }
        else {
            flags["walk"].Set(false);
            flags["idle"].Set(true);
        }
    }

	public override void ActionMan() {
        if (flags["dead"].is_hoisted) {
            Velocity = new(Mathf.MoveToward(Velocity.X, 0, (float)GetPhysicsProcessDeltaTime() * speed), Mathf.MoveToward(Velocity.Y, 0, (float)GetPhysicsProcessDeltaTime() * speed));

            if (flags["dead"].just_hoisted) {
                sprite_player.Play("dead");
            }
        }
        else {
            if (flags["damaged"].is_hoisted) {
                flags["idle"].Set(false);
                knockback_timer += (float)GetPhysicsProcessDeltaTime();
                KnockBack();

                if (flags["damaged"].just_hoisted) {
                    hp -= 50;
                    GD.Print(hp);                  
                }
            }
            else {
                if (flags["walk"].is_hoisted) {
                    Walk();
                    if (flags["walk"].just_hoisted) {
                        sprite_player.Play("walk");
                    }
                }
                
                else if (flags["idle"].just_hoisted) {
                    Idle();
                }
            }
        }
        var label = (Label)FindChild("Label");
        label.Text = "";
        foreach (var flag in flags) {
            flag.Value.Update();
            label.Text = label.Text + flag.Key + " : " + flag.Value.is_hoisted + "\n";
        }
    }

    private void Walk() {
        Velocity = direction.Normalized() * speed;
    }
    private void Idle() {
        Velocity = Vector2.Zero;
        sprite_player.Play("idle");
    }
    private void Attack() {

    }
    private void KnockBack() {
        Velocity =  -(enemy_hitbox.GlobalPosition-this.GlobalPosition).Normalized() * 30; //* (1 - (knockback_timer * knockback_timer))
    }


    public override void _PhysicsProcess(double delta)
	{		
		MoveAndSlide();
	}
}
