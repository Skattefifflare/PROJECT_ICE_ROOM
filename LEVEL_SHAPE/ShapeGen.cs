using Godot;
using System;


[Tool]
public partial class ShapeGen : Node2D
{
    // M for meta data
    int CIRCLE_RADIUS_M;
    int DETAIL_M;
    Vector4 POINT_BOUNDS_M;

    Vector2[] circle_points;
    Polygon2D poly_circle;
    Grav_Field[] fields;


    struct Grav_Field {
        internal Vector2 point;
        internal double radius;
    }

    public override void _Ready() {
        GetSetMeta();
        poly_circle = GenerateCircle(CIRCLE_RADIUS_M, DETAIL_M);
        AddChild(poly_circle);     
        fields = new Grav_Field[1];




    }
    
    private void GravityPull() {
        

        int grav_num = 1;
        for (int i = 0; i < grav_num; i++) {
            var grav_point = new Vector2((float)GD.RandRange(POINT_BOUNDS_M.X, POINT_BOUNDS_M.Z), (float)GD.RandRange(POINT_BOUNDS_M.Y, POINT_BOUNDS_M.W));
            double point_circle_dist = (grav_point.DistanceTo(new Vector2(0, 0))); // abs val
            double grav_radius = GD.RandRange(point_circle_dist - CIRCLE_RADIUS_M * 0.9, point_circle_dist);

            fields[i] = new Grav_Field {
                point = grav_point,
                radius = grav_radius,
            };
        }


        foreach (var p in poly_circle.Polygon) {
            double dist = p.DistanceTo(fields[0].point);
            if (p.DistanceTo(fields[0].point) < fields[0].radius) {
                PullPoint(dist);
            }
        }
        

        void PullPoint(double Adist) {



        }
    }

    private Polygon2D GenerateCircle(int radius = 1, int detail = 1) {

        detail *= 4;
        Vector2[] points = new Vector2[detail];
        double radian_step = (2 * Math.PI) / detail;
        int points_index = 0;

        for (double v = 0; v < 2 * Math.PI; v += radian_step) {

            Vector2 point = new Vector2(radius * (float)Math.Cos(v), radius * (float)Math.Sin(v));
            points[points_index] = point;

            points_index++;
        }
        Polygon2D poly_circle = new Polygon2D() {
            Polygon = points
        };
        return poly_circle;
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if ((int)GetMeta("CIRCLE_RADIUS") != CIRCLE_RADIUS_M) {
            GetSetMeta();
            poly_circle = GenerateCircle();
        }
        if ((int)GetMeta("DETAIL") != DETAIL_M) {
            GetSetMeta();
            poly_circle = GenerateCircle();
        }
    }
    void GetSetMeta() {
        CIRCLE_RADIUS_M = (int)GetMeta("CIRCLE_RADIUS");
        DETAIL_M = (int)GetMeta("DETAIL");
        POINT_BOUNDS_M = (Vector4)GetMeta("POINT_BOUNDS");
    }
}
