using Godot;
using Project_Ice_Room.Scriptbin;


namespace Project_Ice_Room.Enemies;
public partial class Enemy : Creature {
    protected Node2D player;
    protected Vector2 player_distance;
    protected Area2D backoff;

    protected NavigationAgent2D nav_agent;

    public override void _Ready() {
        base._Ready();
        player = (Node2D)GetNode("%player");
        nav_agent = (NavigationAgent2D)GetNode("nav_agent");      
    }
    public override void _Process(double delta) {
        base._Process(delta);
        player_distance = player.Position - Position;
    }
}
