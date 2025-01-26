using Godot;
using System;

public partial class Weapon : Sprite2D
{
    [Export]
    public int dmg = 1;

    Area2D dmg_box;
    public bool is_dangerous = false;


    public override void _Ready() {
        base._Ready();

        dmg_box = (Area2D)FindChild("dmg_box");
        is_dangerous = false;
    }
    public void MakeDangerous() {
        is_dangerous = true;
    }
    public void MakeHarmless() {
        is_dangerous = false;
    }
}
