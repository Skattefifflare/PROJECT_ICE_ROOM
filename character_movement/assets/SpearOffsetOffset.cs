using Godot;
using System;

public partial class SpearOffsetOffset : Bone2D
{
    public override void _Process(double delta)
    {
        base._Process(delta);

        LookAt(GetGlobalMousePosition());
    }
}
