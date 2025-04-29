using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Tool]
public partial class GenSpline : Node2D {
    int index = 0; //index for which point to draw
    public List<Vector2> shape = new List<Vector2>(); //for testing
    private List<Vector2> ControlPoints = new List<Vector2>();
    private int segmentsPerCurve = 50;
    public List<Vector2> splinePoints = new List<Vector2>(); //the actual spline 
    public Polygon2D splinePoly;
    private Random rand = new Random();
    NewLvlShapeObject lso;

    public void Update(Vector2[] shape) {
        if (this.shape.Count == 0) {
            this.shape = ChangeShape(shape);
        }
        SetControlPoints();
        CalculateSpline();
        //splinePoints = this.shape.ToList();
        //SplineToPolygon(); //temp
    }
    private List<Vector2> ChangeShape(Vector2[] shape) {
        List<Vector2> concaveVertices = new List<Vector2>();
        int count = shape.Length;

        bool clockWise = GetPolygonWinding(shape);

        for (int i = 0; i < count; i++) {
            Vector2 prev = shape[(i - 1 + count) % count];
            Vector2 current = shape[i];
            Vector2 next = shape[(i + 1) % count];

            float cross = CrossProduct(prev, current, next);

            if ((clockWise && cross > 0) || (!clockWise && cross < 0)) {
                concaveVertices.Add(current);
            }
        }

        concaveVertices = NaturalShape(concaveVertices);

        return concaveVertices;
    }

    private bool GetPolygonWinding(Vector2[] points) {
        float sum = 0;
        int count = points.Length;

        for (int i = 0; i < count; i++) {
            Vector2 current = points[i];
            Vector2 next = points[(i + 1) % count];
            sum += (next.X - current.X) * (next.Y + current.Y);
        }

        return sum < 0;
    }

    private List<Vector2> NaturalShape(List<Vector2> shape) {
        int count = shape.Count * 2;

        for (int i = 0; i < count; i += 2) {
            int index = i % shape.Count;
            Vector2 current = shape[index];
            Vector2 next = shape[(index + 1) % shape.Count];
            Vector2 mean = (current + next) / 2;
            float distance = current.DistanceTo(next);
            shape.Insert(index + 1, Offset(mean, distance));
        }

        return shape;
    }
    private Vector2 Offset(Vector2 mean, float distance) {
        float scale = 0.25f;

        Vector2 offset = new Vector2((float)(rand.NextDouble() * 2 - 1) * distance * scale, (float)(rand.NextDouble() * 2 - 1) * distance * scale);

        return mean + offset;
    }

    private float CrossProduct(Vector2 a, Vector2 b, Vector2 c) {
        Vector2 ab = b - a;
        Vector2 bc = c - b;
        return ab.X * bc.Y - ab.Y * bc.X;
    }
    private void SetControlPoints() {
        var currentIndex = GetControlIndex();
        if (index == shape.Count) {
            return;
        }
        ControlPoints.Clear();
        ControlPoints.Add(shape[currentIndex.Item1]);//First control point
        ControlPoints.Add(shape[index]);             //Point where drawing begins
        ControlPoints.Add(shape[currentIndex.Item2]);//Point where drawing ends
        ControlPoints.Add(shape[currentIndex.Item3]);//Secon control point
        index++;

        //segmentsPerCurve = SetSegmentsPerCurve(ControlPoints[1], ControlPoints[2]);

    }
    private void CalculateSpline() {
        HashSet<Vector2> uniquepoints = new HashSet<Vector2>();
        for (int i = 0; i < ControlPoints.Count - 3; i++) // Iterate through control points
        {
            Vector2 p0 = ControlPoints[i];
            Vector2 p1 = ControlPoints[i + 1];
            Vector2 p2 = ControlPoints[i + 2];
            Vector2 p3 = ControlPoints[i + 3];

            for (int j = 0; j <= segmentsPerCurve; j++) {
                float t = j / (float)segmentsPerCurve; // Parameter t (0 to 1)
                splinePoints.Add(CalculateCatmullRomPoint(t, p0, p1, p2, p3));
            }
            CheckDuplicate();
        }
        if (index < shape.Count) {
            Update(shape.ToArray());
        }
        else {
            OS.DelayMsec(1000);
            SplineToPolygon();
        }
    }
    //The catmull Rom algorithim to create the spline points
    private Vector2 CalculateCatmullRomPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) {
        float t2 = t * t;
        float t3 = t2 * t;

        // Catmull-Rom blending matrix
        return 0.5f * (
            (2 * p1) +
            (-p0 + p2) * t +
            (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
            (-p0 + 3 * p1 - 3 * p2 + p3) * t3
        );
    }

    private (int, int, int) GetControlIndex() {
        int max = shape.Count - 1;

        return index switch {
            _ when index == max => (index - 1, 0, 1), //drawing from last -> first point
            _ when index == max - 1 => (index - 1, index + 1, 0), //--||-- second to last -> last point
            _ when index == 0 => (max, index + 1, index + 2), //--||-- first -> second point
            _ => (index - 1, index + 1, index + 2), //base case
        };
    }
    private void CheckDuplicate() {
        for (int a = 0; a < splinePoints.Count - 1; a++) {
            if (splinePoints[a] == splinePoints[a + 1]) {
                //GD.Print("Found duplicate killed");
                splinePoints.RemoveAt(a);
                a--;
            }
        }
    }
    private void SplineToPolygon() {
        PolygonChecker pc = new PolygonChecker();
        //GD.Print("______________________");
        if (pc.HasSelfIntersection(splinePoints)) {
            GD.Print("Invalid polygon, restarting");
            lso = new NewLvlShapeObject(700, 700, 5);
            shape = ChangeShape(lso.GetShape());
            splinePoints.Clear();
            index = 0;
            Update(shape.ToArray());
            return;
        }
        OS.DelayMsec(1000);
        splinePoly = new Polygon2D() {
            Polygon = splinePoints.ToArray(),
            Color = new Color(0.455f, 0.604f, 0.396f, 1f)
        };
        splinePoly.ZIndex =  -100;
    }
}
