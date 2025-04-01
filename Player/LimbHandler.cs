using Godot;
using System;

[Tool]
public partial class LimbHandler : Node2D
{
    Node2D frontarmtarget;
    Node2D backarmtarget;
    Node2D frontlegtarget;
    Node2D backlegtarget;

    Node2D local_frontarmtarget;
    Node2D local_backarmtarget;
    Node2D local_frontlegtarget;
    Node2D local_backlegtarget;
    public override void _Ready()
	{
        frontarmtarget = (Node2D)FindChild("frontarmtarget");
        backarmtarget = (Node2D)FindChild("backarmtarget");
        frontlegtarget = (Node2D)FindChild("frontlegtarget");
        backlegtarget = (Node2D)FindChild("backlegtarget");

        local_frontarmtarget = (Node2D)FindChild("local_frontarmtarget");
        local_backarmtarget = (Node2D)FindChild("local_backarmtarget");
        local_frontlegtarget = (Node2D)FindChild("local_frontlegtarget");
        local_backlegtarget = (Node2D)FindChild("local_backlegtarget");
    }

    public override void _Process(double delta) {
        local_frontarmtarget.GlobalPosition = frontarmtarget.Position + new Vector2(50, 50);
        local_backarmtarget.Position = backarmtarget.Position + new Vector2(50, 50);
        local_frontlegtarget.Position = frontlegtarget.Position + new Vector2(50, 50);
        local_backlegtarget.Position = backlegtarget.Position + new Vector2(50, 50);
    }
}
