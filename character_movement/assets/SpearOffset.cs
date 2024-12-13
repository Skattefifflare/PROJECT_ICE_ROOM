using Godot;
using System;

public partial class SpearOffset : Bone2D
{
    public override void _Process(double delta)
    {
        LookAt(GetGlobalMousePosition());
        base._Process(delta);
    }
       //summon: "dont: filip;" end 
       //     end

       //HUGO
}
