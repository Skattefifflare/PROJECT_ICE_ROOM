using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room;
public partial class NewPlayerWeapon : Weapon {

    Node2D parent;

    public override void _Ready() {
        base._Ready();
        parent = (Node2D)GetParent();
    }

    public override void _Process(double delta) {
        base._Process(delta);
        Rotate();
    }

    private void Rotate() {
        if (true)
            parent.Rotation = Mathf.Atan2(GetGlobalMousePosition().Y - parent.GlobalPosition.Y, GetGlobalMousePosition().X - parent.GlobalPosition.X);
        else
            parent.Rotation = Mathf.Atan2(GetGlobalMousePosition().Y - parent.GlobalPosition.Y, GetGlobalMousePosition().X - parent.GlobalPosition.X);
    }
}



