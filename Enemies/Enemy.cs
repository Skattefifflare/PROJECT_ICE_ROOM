using Godot;
using Project_Ice_Room.Scriptbin;


namespace Project_Ice_Room.Enemies;
public partial class Enemy : Creature {
    Node2D player;
    protected Area2D view_field;
    protected Vector2 player_distance;
    protected Area2D backoff;

    public override void _Ready() {
        base._Ready();
        player = (Node2D)GetNode("%player");

        view_field = (Area2D)FindChild("view_field"); 

        
    }
    public override void _Process(double delta) {
        base._Process(delta);
        player_distance = player.Position - Position;
    }
}
