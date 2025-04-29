using Godot;
using System;
using Project_Ice_Room.Scriptbin;
using Project_Ice_Room.Creatures;

public partial class Healthbar : Sprite2D
{
    int initial_hp;
    CreatureRework parent;
    Vector2 initial_scale;
    public override void _Ready() {
        base._Ready();
        parent = (CreatureRework)GetParent();
        initial_hp = parent.hp;
        initial_scale = Scale;
    }
    public override void _Process(double delta) {
        base._Process(delta);

        Scale = new Vector2(initial_scale.X * parent.hp/initial_hp, Scale.Y);
    }

}
