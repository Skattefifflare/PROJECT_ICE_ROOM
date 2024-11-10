using Godot;
using System;
using System.Collections.Generic;
using System.Linq;



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

        NewLvlShapeObject lso = new NewLvlShapeObject(2, new Vector2(200, 100));
        
        Polygon2D polygon = new Polygon2D {
            Polygon = lso.GetShape(),
            Color = new Color(0.4f, 0.8f, 0.2f, 0.8f)
        };
        AddChild(polygon);
        
        lso.PrintArray();
        
    }


    public override void _Process(double delta) {
        base._Process(delta);

    }
}



