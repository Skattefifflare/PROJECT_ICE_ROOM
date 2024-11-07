using Godot;
using System;
using System.Collections.Generic;


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

class LvlShapeObject { // contains all the bounding points of the level and some other stuff. 
    List<Vector2> POINTS;

    LvlComponent BASE_COMPONENT;

    LvlShapeObject(int rect_num) {
        POINTS = new List<Vector2>();

    }

    class LvlComponent { // a rectangle that can have a rectangle on each corner
        (bool, bool, bool, bool) TAKEN_CORNERS; // which corners are attached to other components



        LvlComponent() {
            TAKEN_CORNERS = (false, false, false, false); // redundant
        }
        void AttachComponent() {

        }

    }
    class CustomRectangle { // needed to pass as reference and not value. maybe not needed
        float startX, startY, endX, endY;

        CustomRectangle(float startX, float startY, float endX, float endY) {
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
        }   
        float GetWidth() {
            return Math.Abs(startX - endX);
        }
        float GetHeight() {
            return Math.Abs(startY - endY);
        }
    }
}


