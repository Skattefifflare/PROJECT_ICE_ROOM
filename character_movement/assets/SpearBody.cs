using Godot;
using System;

public partial class SpearBody : Bone2D
{
    public override void _Process(double delta)
    {
        LookAt(GetGlobalMousePosition());
        base._Process(delta);
    }
}
