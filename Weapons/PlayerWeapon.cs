using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Weapons;
public partial class PlayerWeapon : Weapon {

    Node2D parent;
    Node2D grandparent;
    AnimationPlayer animation_player;
    public Marker2D left_hold;
    public Marker2D right_hold;

    public override void _Ready() {
        base._Ready();
        parent = (Node2D)GetParent();
        grandparent = (Node2D)parent.GetParent();
        animation_player = (AnimationPlayer)FindChild("animation_player");
        left_hold = (Marker2D)FindChild("left_hold");
        right_hold = (Marker2D)FindChild("right_hold");
    }

    public override void _Process(double delta) {
        base._Process(delta);
        Rotate();
        CheckInput();
    }

    private void Rotate() {
        parent.Rotation = Mathf.Atan2(GetGlobalMousePosition().Y - grandparent.GlobalPosition.Y, GetGlobalMousePosition().X - grandparent.GlobalPosition.X);
        if (grandparent.Scale.X < 0) parent.Rotation *= -1;
        else parent.Rotation += Mathf.Pi;
        //GD.Print("p---" + parent.Scale.X);
        //GD.Print("gp--" + grandparent.Scale.X);
    }

    private void CheckInput() {
        if (Input.IsActionJustPressed("left_click")) {
            animation_player.Play("attack");
            
        }
    }
}