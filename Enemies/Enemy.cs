using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Project_Ice_Room.Scriptbin;
using Project_Ice_Room.Player;

namespace Project_Ice_Room.Enemies;
public partial class Enemy : Creature {
    Node2D player;

    protected Area2D view_field;

    internal List<State> combat_states;

    protected Vector2 player_distance;

    public override void _Ready() {
        base._Ready();
        player = (Node2D)GetNode("%player");

        view_field = (Area2D)FindChild("view_field"); 
        view_field.AreaEntered += (a) => sh.SetStates(combat_states);
    }
    public override void _Process(double delta) {
        base._Process(delta);
        player_distance = player.Position - Position;

    }
}
