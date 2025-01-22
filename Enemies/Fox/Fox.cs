using Godot;
using Project_Ice_Room.Player;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;


public partial class Fox : Creature
{
    State RunToPlayer;
    State KnockBack;

    private int prev_hp;

    private Vector2 player_distance;

    public override void _Ready() {
        base._Ready();

        prev_hp = HP;

        RunToPlayer = new(
            () => Math.Abs(player_distance.Length()) >= 40,
            () => true,
            () => RunToPlayerM(),
            () => { return; },
            "walk",
            true
        );
        KnockBack = new(
            () => prev_hp != HP,
            () => KnockBackM(),
            () => true,
            () => { return; },
            true,
            "idle"
        );

        SH.SetStates(new List<State> { RunToPlayer, Idle });
    }

    private void RunToPlayerM() {  
        DIRECTION = player_distance.Normalized();

        Velocity = DIRECTION * SPEED;                  
    }
    private void KnockBackM() {

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
