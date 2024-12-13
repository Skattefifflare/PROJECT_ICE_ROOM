using Godot;
using System;

public partial class SpearBody : Bone2D
{
    double offsetmultiplier = 0.5;
    double distancemax = 20;
    public override void _Process(double delta)
    {
        Vector2 GlobalMousePosition = GetGlobalMousePosition();
        Transform2D GlobalTransform = GetGlobalTransform();
        Vector2 GlobalPosition = GlobalTransform.Origin;
        double distance = Math.Sqrt(Math.Pow(GlobalMousePosition.X-GlobalPosition.X, 2) + Math.Pow(GlobalMousePosition.Y-GlobalPosition.Y, 2));
        Vector2 offset = new((float)Math.Abs(distance * offsetmultiplier),0);
        if (distance * offsetmultiplier >= distancemax)
        {
            offset = new Vector2((float)(distancemax), 0);
        }
        this.Position = offset;
        LookAt(GlobalMousePosition);
        base._Process(delta);
    }
}
