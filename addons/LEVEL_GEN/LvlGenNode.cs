using Godot;
using System;
using System.Collections.Generic;



[Tool]
public partial class LvlGenNode : Node2D
{
    private Vector2 BASE_SIZE;
    [Export]
    public Vector2 _BASE_SIZE {
        get {
            return BASE_SIZE;
        }
        set {
            BASE_SIZE = value;
        }
    }

    public override void _Ready() {
        base._Ready();

        

        GD.Print("hello");
    }


    public override void _Process(double delta) {
        base._Process(delta);

        GD.Print("hello");
    }
}



