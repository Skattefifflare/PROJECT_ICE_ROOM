using Godot;
using Project_Ice_Room.Scriptbin;
using Project_Ice_Room.Creatures;

namespace Project_Ice_Room.Enemies;
public partial class EnemyRework : CreatureRework {

    protected NavigationAgent2D nav_agent;
    protected Node2D player;
    protected Vector2 player_distance;
    protected AnimatedSprite2D sprite_player;
    protected Weapon weapon;
    public override void _Ready() {
        base._Ready();
    }
    public override void _Process(double delta) {
        base._Process(delta);
    }
}