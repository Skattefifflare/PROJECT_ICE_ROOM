using Godot;
using System;


[Tool]
public partial class ShapeGen : Node2D
{
    public override void _Ready() {
        Polygon2D poly_circle = new Polygon2D() {
            Polygon = GenerateCircle(100, 4),
        };
        AddChild(poly_circle);
        // polygon points seemingly cannot be manipulated after creation
    }
    
    private void GravityPull() {

    }

    private Vector2[] GenerateCircle(int radius = 1, int detail = 1) {

        detail *= 4;
        Vector2[] points = new Vector2[detail];
        double radian_step = (2 * Math.PI) / detail;
        int points_index = 0;

        for (double v = 0; v < 2 * Math.PI; v += radian_step) {

            Vector2 point = new Vector2(radius * (float)Math.Cos(v), radius * (float)Math.Sin(v));
            points[points_index] = point;

            points_index++;
        }
        return points;
    }
}
