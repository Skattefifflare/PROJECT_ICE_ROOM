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
        CallState("run_towards_player");       
    }

    private void RunTowardsPlayer() {
        sprite_player.Play("walk");
        GD.Print(sprite_player.Animation.ToString());
        var player = (PlayerClass)GetNode("%player");
        
        Vector2 direction = player.Position-Position;
        direction = direction.Normalized();

        Velocity = direction * 50;
    }
}
