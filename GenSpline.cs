using Godot;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class GenSpline : Node2D
{
    int index = 0; //index for which point to draw
    List<Vector2> shape = new List<Vector2>(); //for testing
    public List<Vector2> ControlPoints = new List<Vector2>();
    public int SegmentsPerCurve = 100; //Needs some testing for a better vaule?
    private List<Vector2> _splinePoints = new List<Vector2>(); //the actual spline
    private Polygon2D ControllPolygon; //for testing
    public override void _Ready()
    {
        Update();
        AddChild(ControllPolygon);
    }
    private void Update()
    {
        if (shape.Count == 0)
        {
            shape = new List<Vector2>
            {
                new Vector2(0, 0),    
                //new Vector2(0, 200),   
                //new Vector2(300, 200), 
                //new Vector2(300, 300), 
                //new Vector2(500, 300), 
                //new Vector2(500, 100), 
                //new Vector2(400, 100),
                //new Vector2(400, 0)  
                new Vector2(300, 0),
                new Vector2(300, 150),
                new Vector2(100, 150),
                new Vector2(100, 350),
                new Vector2(400, 350),
                new Vector2(400, 200),
                new Vector2(650, 200),
                new Vector2(650, -100),
                new Vector2(400, -100),
                new Vector2(400, -200),
                new Vector2(600, -200),
                new Vector2(600, -600),
                new Vector2(350, -600),
                new Vector2(350, -300),
                new Vector2(0, -300)

            };
            ControllPolygon = new Polygon2D
            {
                Polygon = new Vector2[]
                {
                    new Vector2(0, 0),
                    //new Vector2(0, 200),
                    //new Vector2(300, 200),
                    //new Vector2(300, 300),
                    //new Vector2(500, 300),
                    //new Vector2(500, 100),
                    //new Vector2(400, 100),
                    //new Vector2(400, 0)
                    new Vector2(300, 0),
                    new Vector2(300, 150),
                    new Vector2(100, 150),
                    new Vector2(100, 350),
                    new Vector2(400, 350),
                    new Vector2(400, 200),
                    new Vector2(650, 200),
                    new Vector2(650, -100),
                    new Vector2(400, -100),
                    new Vector2(400, -200),
                    new Vector2(600, -200),
                    new Vector2(600, -600),
                    new Vector2(350, -600),
                    new Vector2(350, -300),
                    new Vector2(0, -300)
                },
                Color = new Color(0.5f, 0.5f, 0.5f, 0.5f)
            };
        }
        SetControlPoints();
        CalculateSpline();
    }
    public override void _Draw()
    {
        if (_splinePoints.Count > 1)
        {
            for (int i = 0; i < _splinePoints.Count - 1; i++)
            {
                DrawLine(_splinePoints[i], _splinePoints[i + 1], Colors.White, 1);
            }
        }
    }
    private void SetControlPoints()
    {
        var currentIndex = GetControlIndex();
        if(index == shape.Count)
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
        if (index < shape.Count) 
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
        int max = shape.Count - 1;

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
