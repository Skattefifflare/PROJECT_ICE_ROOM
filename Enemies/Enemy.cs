using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Enemy : Creature
{
	Area2D view_field;
	public bool sees_player = false;


    public override void _Ready() {
        base._Ready();	

        view_field = (Area2D)FindChild("view_field");
		view_field.AreaEntered += (object_that_entered) => {
			if (object_that_entered == gm.player.FindChild("visibility_box")) {
                sees_player = true;
            }       
        }; 		
    }
}
