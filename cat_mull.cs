using Godot;
using System;


[Tool]
internal partial class CatMull : Path2D
{
	Vector2[] points;
    Curve spline;

	internal CatMull(/*Vector2[] ipoints*/)
	{
        spline = new Curve();
        points = new Vector2[4];
        points[0] = new Vector2(0, 0);
        points[1] = new Vector2(100, 0);
        points[2] = new Vector2(100, 100);
        points[3] = new Vector2(0, 100);

        Vector2[] c_points = new Vector2[2];
        c_points[0] = new Vector2(25, -25);
        c_points[1] = new Vector2(75, -25);
    }
    internal void CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        Vector2 q0 = p0.Lerp(p1, t);
        Vector2 q1 = p1.Lerp(p2, t);
        Vector2 q2 = p2.Lerp(p3, t);

        Vector2 r0 = q0.Lerp(q1, t);
        Vector2 r1 = q1.Lerp(q2, t);

        Vector2 s = r0.Lerp(r1, t);    
    }
}
