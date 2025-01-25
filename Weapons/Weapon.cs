using Godot;
using System;

public partial class Weapon : Sprite2D
{
    [Export]
    public int dmg = 1;

    Area2D dmg_box;


    public override void _Ready() {
        base._Ready();

        dmg_box = (Area2D)FindChild("dmg_box");
        dmg_box.Monitorable = false;
    }
    public void MakeDangerous() {
        dmg_box.Monitorable = true;
    }
    public void MakeHarmless() {
        dmg_box.Monitorable = false;
    }

}
