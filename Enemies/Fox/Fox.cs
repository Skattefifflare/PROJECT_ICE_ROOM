using Godot;
using Project_Ice_Room.Player;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;
using Project_Ice_Room.Enemies;


public partial class Fox : Enemy
{
    State RunToPlayer;
    State KnockBack;
    State Bite;

    private bool knockbackflag = false;
    public override void _Ready() {
        base._Ready();
        KnockBack = new(
            () => knockbackflag,
            () => knockbackflag == false,
            () => { return; },
            () => KnockBackM(),
            () => { return; },

            "idle",
            true
        );

        RunToPlayer = new(
            () => Math.Abs(player_distance.Length()) >= 40,
            () => Math.Abs(player_distance.Length()) <= 40,

            () => { return; },
            () => RunToPlayerM(),
            () => { direction = Vector2.Zero; },

            "walk",
            true
        );

        Bite = new (
            () => Math.Abs(player_distance.Length()) <= 20,
            () => sprite_done,
            
            () => whap.MakeDangerous(),
            () => { return; },
            () => whap.MakeHarmLess(),

            "attack",
            true
        );

        sh.SetStates(new List<State> { Idle });
        combat_states = new List<State> {Bite, RunToPlayer, Idle };
    }

    private void RunToPlayerM() {  
        direction = player_distance.Normalized();

        Velocity = direction * speed;                  
    }
    private void KnockBackM() {

    }
}
