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

        LvlShapeObject lso = new LvlShapeObject(13, new Vector2(200, 200));
        
        Polygon2D polygon = new Polygon2D {
            Polygon = lso.GetShape(),
            Color = new Color(0.4f, 0.8f, 0.2f, 0.8f)
        };
        AddChild(polygon);
        
        
        
        
    }
    public override void _ExitTree() {
        base._ExitTree();

        foreach (var c in GetChildren()) {
            RemoveChild(c);
            c.QueueFree();
        }
    }


    public override void _Process(double delta) {
        base._Process(delta);

    }
}



