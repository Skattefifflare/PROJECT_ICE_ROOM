using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;
using Project_Ice_Room.Player;


public partial class Fox : Creature
{
    State RunToPlayer;
    private Vector2 player_distance;

    public override void _Ready() {
        base._Ready();

        RunToPlayer = new(
            () => Math.Abs(player_distance.Length()) >= 40,
            () => true,
            () => RunToPlayerM(),
            () => { return; },
            "walk",
            true
        );

        SH.SetStates(new List<State> { RunToPlayer, Idle });
    }

    private void RunToPlayerM() {  
        DIRECTION = player_distance.Normalized();

        Velocity = DIRECTION * SPEED;                  
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
    }

    public override void _Process(double delta) {
        base._Process(delta);
        Player player = (Player)GetNode("%player");
        player_distance = player.Position - Position;
    }
}
