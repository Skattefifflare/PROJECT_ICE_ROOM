using Godot;
using Project_Ice_Room.LVL_GENERATOR;
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

        NewerLvlShapeObject lso = new NewerLvlShapeObject(200, 200, 4);
       
        var generated_shape = lso.complete_shape;


        Polygon2D polygon = new Polygon2D {
            Polygon = generated_shape,
            Color = new Color(0.4f, 0.8f, 0.2f)
        };
        AddChild(polygon);

        lso.PrintShape();


        GD.Print("______polygon points_______");
        foreach (var p in polygon.Polygon) {
            GD.Print(p);
        }
        
        
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



