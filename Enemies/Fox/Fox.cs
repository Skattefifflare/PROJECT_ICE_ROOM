using Godot;
using System;
using System.Collections.Generic;


public partial class Fox : CreatureClass
{
    public override void _Ready() {
        base._Ready();
        hp = 50;


        AddStates(new Dictionary<string, Action>() {
            {"run_towards_player", RunTowardsPlayer}
        });
    }


    protected override void StateMachine() {
        PlayerClass player = (PlayerClass)GetNode("%player");
        Vector2 pos_diff = player.Position - Position;
        if (pos_diff.Length() > 40) {
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
        Velocity = direction * 50;                  
    }
}
