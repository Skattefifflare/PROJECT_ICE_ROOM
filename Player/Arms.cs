using Godot;
using System;

[Tool]
public partial class Arms : Node2D
{
	Marker2D left_marker;
    Marker2D right_marker;
	Marker2D left_hold;
    Marker2D right_hold;
    
    public override void _Ready()
	{
        left_hold = (Marker2D)FindChild("left_hold");
        right_hold = (Marker2D)FindChild("right_hold");
        left_marker = (Marker2D)FindChild("left_marker");
        right_marker = (Marker2D)FindChild("right_marker");
    }
	public override void _Process(double delta)
	{
        try {
            left_marker.GlobalPosition = left_hold.GlobalPosition + new Vector2(23, 23);
            right_marker.GlobalPosition = right_hold.GlobalPosition + new Vector2(23, 23);
        }
        catch {
            if (left_hold == null) {
                GD.Print("Left hold is null");
            }
            if (right_hold == null) {
                GD.Print("Right hold is null");
            }
            if (left_marker == null) {
                GD.Print("Left marker is null");
            }
            if (right_marker == null) {
                GD.Print("Right marker is null");
            }
        }
    }
}
