using Godot;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class GenSpline : Node2D
{
    int index = 0;
    List<Vector2> shape = new List<Vector2>();
    public List<Vector2> ControlPoints = new List<Vector2>();
    public int SegmentsPerCurve = 100;
    private List<Vector2> _splinePoints = new List<Vector2>();
    private Polygon2D ControllPolygon;
    public override void _Ready()
    {
        if (shape.Count == 0)
        {
            //shape.Add(new Vector2(200,200));
            //shape.Add(new Vector2(0, 200));
            //shape.Add(new Vector2(0, 0));
            //shape.Add(new Vector2(200, 0));
            shape = new List<Vector2>
            {
                new Vector2(0, 0),    
                new Vector2(0, 50),   
                new Vector2(100, 50), 
                new Vector2(100, 30), 
                new Vector2(100, 80), 
                new Vector2(150, 80), 
                new Vector2(150, 30),
                new Vector2(100, 30)  
            };

            ControllPolygon = new Polygon2D
            {
                Polygon = new Vector2[]
                {
                    new Vector2(0, 0),
                    new Vector2(0, 50),
                    new Vector2(100, 50),
                    new Vector2(100, 30),
                    new Vector2(100, 80),
                    new Vector2(150, 80),
                    new Vector2(150, 30),
                    new Vector2(100, 30)
                },
                Color = new Color(0.5f, 0.5f, 0.5f, 0.5f)
            };
        }
        AddChild(ControllPolygon);

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
            _Ready();
        }
    }
    //The catmull Rom algorithim to create the spline
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
            _ when index == max => (index - 1, 0, 1),
            _ when index == max - 1 => (index - 1, index + 1, 0),
            _ when index == 0 => (max, index + 1, index + 2),
            _ => (index - 1, index + 1, index + 2),
        };
    }
}
