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
        sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
        nav_agent = (NavigationAgent2D)FindChild("nav_agent");
    }
    public override void _Process(double delta) {
        base._Process(delta);
        
    }
}