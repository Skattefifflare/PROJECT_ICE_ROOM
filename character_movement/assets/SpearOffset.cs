using Godot;
using System;

public partial class SpearOffset : Bone2D
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        LookAt(GetGlobalMousePosition());

        if (Input.IsActionPressed("ui_attack"))
        {

        }
    }
       //summon: "dont: filip;" end 
       //     end

       //HUGO
}
