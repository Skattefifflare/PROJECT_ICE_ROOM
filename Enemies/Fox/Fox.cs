using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;
using Project_Ice_Room.Player;


public partial class Fox : Creature
{
    State RunToPlayerState;


    public override void _Ready() {
        base._Ready();

       

    }

    private void RunToPlayer() {  
        Player player = (Player)GetNode("%player");
        Vector2 pos_diff = player.Position - Position;
        DIRECTION = pos_diff.Normalized();

        Velocity = DIRECTION * SPEED;                  
    }
}
