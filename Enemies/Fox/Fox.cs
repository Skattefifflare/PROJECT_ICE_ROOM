using Godot;
using System;
using System.Collections.Generic;


public partial class Fox : CreatureClass
{
    public override void _Ready() {
        base._Ready();
        hp = 100;
        speed = 60;



        AddStates(new Dictionary<string, Action>() {
            {"run_towards_player", RunTowardsPlayer}
        });
    }


    protected override void StateMachine() {
        PlayerClass player = (PlayerClass)GetNode("%player");
        Vector2 pos_diff = player.Position - Position;

        if (hp <= 0) {
            CallState("death");
        }
        else if (Math.Abs(pos_diff.Length()) > 40) {
            CallState("run_towards_player");
        }
        else {
            CallState("idle");
        }           
    }

    private void RunTowardsPlayer() {  
        is_busy = false;
        if (sprite_player.Animation != "walk") {
            sprite_player.Play("walk");
        }

        PlayerClass player = (PlayerClass)GetNode("%player");
        Vector2 pos_diff = player.Position - Position;
        Vector2 direction = pos_diff.Normalized();

        if (direction.X > 0) {
            sprite_player.FlipH =true;
        }
        else {
            sprite_player.FlipH =false;
        }

        Velocity = direction * speed;                  
    }
}
