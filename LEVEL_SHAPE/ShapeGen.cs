using Godot;
using System;


[Tool]
public partial class ShapeGen : Node2D
{
    // M for meta data
    int CIRCLE_RADIUS_M;
    int DETAIL_M;

    Vector2[] circle_points;
    Polygon2D poly_circle;

    Vector2[] grav_points;

    public override void _Ready() {
        circle_points = new Vector2[DETAIL_M * 4];
        GetSetPoints();    

        poly_circle = new Polygon2D() {
            Polygon = circle_points
        };
        AddChild(poly_circle);     
    }
    
    private void GravityPull() {
        int grav_num = 10;
        for (int i = 0; i < grav_num; i++) {

        }
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

    public override void _Process(double delta) {
        base._Process(delta);

        if ((int)GetMeta("CIRCLE_RADIUS") != CIRCLE_RADIUS_M) {
            GetSetPoints();
            poly_circle.Polygon = circle_points;
        }
        if ((int)GetMeta("DETAIL") != DETAIL_M) {
            GetSetPoints();
            poly_circle.Polygon = circle_points;
        }
    }
    void GetSetPoints() {
        CIRCLE_RADIUS_M = (int)GetMeta("CIRCLE_RADIUS");
        DETAIL_M = (int)GetMeta("DETAIL");
        circle_points = GenerateCircle(CIRCLE_RADIUS_M, DETAIL_M);
    }
}
