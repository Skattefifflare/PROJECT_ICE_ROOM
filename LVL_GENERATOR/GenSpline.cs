using Godot;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class GenSpline : Node2D
{
    int index = 0; //index for which point to draw
    Vector2[] shape; //lvl shape
    public List<Vector2> ControlPoints = new List<Vector2>();
    public int SegmentsPerCurve = 100; //Needs some testing for a better vaule?
    private List<Vector2> _splinePoints = new(); //the actual spline
    public Polygon2D splinePoly;

    public GenSpline(Vector2[] shape)
    {
        this.shape = shape;
    }
    public void Update()
    {
        SetControlPoints();
        CalculateSpline();
    }
    public void CreateSplinePolygon()
    {
        splinePoly = new Polygon2D() {
            Polygon = _splinePoints.ToArray(),
            Color = new Color(0.4f, 0.8f, 0.2f, 1f)
        };
    }
    private void SetControlPoints()
    {
        var currentIndex = GetControlIndex();
        if(index == shape.Length)
        {
            return;
        }
        ControlPoints.Clear();
        ControlPoints.Add(shape[currentIndex.Item1]);
        ControlPoints.Add(shape[index]);
        ControlPoints.Add(shape[currentIndex.Item2]);
        ControlPoints.Add(shape[currentIndex.Item3]);
        index++;
    }
    //Adds points to the spline using control points
    private void CalculateSpline()
    {
        for (int i = 0; i < ControlPoints.Count - 3; i++) // Iterate through control points
        {
            Vector2 p0 = ControlPoints[i];
            Vector2 p1 = ControlPoints[i + 1];
            Vector2 p2 = ControlPoints[i + 2];
            Vector2 p3 = ControlPoints[i + 3];

            for (int j = 0; j <= SegmentsPerCurve; j++)
            {
                float t = j / (float)SegmentsPerCurve; // Parameter t (0 to 1)
                _splinePoints.Add(CalculateCatmullRomPoint(t, p0, p1, p2, p3));
            }
        }
        if (index < shape.Length) 
        {
            Update();
        }
    }
    //The catmull Rom algorithim to create the spline points
    private Vector2 CalculateCatmullRomPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
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

    private (int, int, int) GetControlIndex()
    {
        int max = shape.Length - 1;

        return index switch
        {
            //Peak readability
            _ when index == max => (index - 1, 0, 1), //drawing from last -> first point
            _ when index == max - 1 => (index - 1, index + 1, 0), //--||-- second to last -> last point
            _ when index == 0 => (max, index + 1, index + 2), //--||-- first -> second point
            _ => (index - 1, index + 1, index + 2), //base case
        };
    }
}
