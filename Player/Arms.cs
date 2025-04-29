using Godot;
using System;
using Project_Ice_Room.Weapons;

public partial class Arms : Node2D
{
	Marker2D left_marker;
    Marker2D right_marker;
    PlayerWeapon equipped_weapon;
    
    public override void _Ready()
	{
        left_marker = (Marker2D)FindChild("left_marker");
        right_marker = (Marker2D)FindChild("right_marker");
        try {
            equipped_weapon = (PlayerWeapon)FindChild("weapon");
        }
        catch {
            GD.PrintErr("problem finding the equipped weapon");
        }      
        if (equipped_weapon != null) {
            GD.Print("found weapon");
        }
    }
	public override void _Process(double delta)
	{
        try {
            left_marker.GlobalPosition = equipped_weapon.left_hold.GlobalPosition + new Vector2(23, 23);
            right_marker.GlobalPosition = equipped_weapon.right_hold.GlobalPosition + new Vector2(23, 23);
        }
        catch {
            if (equipped_weapon == null) {
                GD.PrintErr("equipped_weapon is null");
            }
            else if (equipped_weapon.left_hold == null) {
                GD.PrintErr("left_hold is null");
            }
        }
    }
}
