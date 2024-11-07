using Godot;
using System;


[Tool]
public partial class LvlGenNode : Node2D
{
    private Vector2 SIZE;
    [Export]
    public Vector2 _SIZE {
        get {
            return SIZE;
        }
        set {
            SIZE = value;
        }
    }

    public override void _Ready() {
        base._Ready();

        Polygon2D polygon = new Polygon2D() {
            Polygon = new Vector2[] {
                new Vector2( -100, -100),
                new Vector2( 100, -100),
                new Vector2( 100, 100),
                new Vector2( -100, 100)
            },
            Color = new Color(1,1,1,1)

        };
        AddChild(polygon);


        GD.Print("hello");
    }


    public override void _Process(double delta) {
        base._Process(delta);

        GD.Print("hello");
    }

}
