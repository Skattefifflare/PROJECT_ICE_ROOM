using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room;
public partial class NewPlayerWeapon : Weapon {

    Node2D parent;
    AnimationPlayer animation_player;

    public override void _Ready() {
        base._Ready();
        parent = (Node2D)GetParent();
        animation_player = (AnimationPlayer)FindChild("animation_player");
    }

    public override void _Process(double delta) {
        base._Process(delta);
        Rotate();
        CheckInput();
    }

    private void Rotate() {
        parent.Rotation = Mathf.Atan2(GetGlobalMousePosition().Y - parent.GlobalPosition.Y, GetGlobalMousePosition().X - parent.GlobalPosition.X);       
    }
    private void CheckInput() {
        if (Input.IsActionJustPressed("left_click")) {
            animation_player.Play("attack");
            
        }
    }
}



