using System;
using System.Collections.Generic;
using Godot;

public partial class PolygonChecker : Node2D {
    public struct Segment {
        public Vector2 A, B;
        public Segment(Vector2 a, Vector2 b) {
            A = a;
            B = b;
        }
    }

    public bool HasSelfIntersection(List<Vector2> polygon) {
        if (polygon.Count < 3) return false;

        polygon = UpScalePolygon(polygon, 1000f);

        var events = new List<(Vector2 point, bool isStart, Segment segment)>();
        var activeSegments = new SortedDictionary<Vector2, Segment>(new YOrderComparer());

        for (int i = 0; i < polygon.Count; i +=2) {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Count];
            if (a.Y > b.Y) (a, b) = (b, a);
            events.Add((a, true, new Segment(a, b)));
            events.Add((b, false, new Segment(a, b)));
        }

        events.Sort((e1, e2) => e1.point.X.CompareTo(e2.point.X));

        foreach (var e in events) {
            if (e.isStart) {
                activeSegments[e.point] = e.segment;
                if (HasIntersectionWithNeighbors(activeSegments, e.point)) return true;
            }
            else {
                activeSegments.Remove(e.point);
            }
        }
        return false;
    }

    private bool HasIntersectionWithNeighbors(SortedDictionary<Vector2, Segment> segments, Vector2 point) {
        var values = new List<Segment>(segments.Values);
        int index = values.FindIndex(s => s.A == point || s.B == point);
        if (index > 0 && SegmentsIntersect(values[index - 1], values[index])) {
            if(SegmentsSharePoint(values[index - 1], values[index])) return false; //Ignore if they share a point
            GD.Print("index: ", index);
            GD.Print("Intersection found between segments: ", values[index - 1].A," ",values[index - 1].B, " and ", values[index].A," ",values[index].B);
            return true;
        }
        if (index < values.Count - 1 && SegmentsIntersect(values[index], values[index + 1])) {
            if (SegmentsSharePoint(values[index], values[index + 1])) return false; //Ignore if they share a point
            GD.Print("index: ", index);
            GD.Print("Intersection found between segments: ", values[index].A," ", values[index].B, " and ", values[index + 1].A, " ", values[index + 1].B);
            return true;
        }
        return false;
    }

    private bool SegmentsIntersect(Segment s1, Segment s2) {
        float o1 = Orientation(s1.A, s1.B, s2.A);
        float o2 = Orientation(s1.A, s1.B, s2.B);
        float o3 = Orientation(s2.A, s2.B, s1.A);
        float o4 = Orientation(s2.A, s2.B, s1.B);
        return o1 != o2 && o3 != o4;
    }
    public bool SegmentsSharePoint(Segment s1, Segment s2) {
        return s1.A == s2.A || s1.A == s2.B || s1.B == s2.A || s1.B == s2.B;
    }


    private float Orientation(Vector2 p, Vector2 q, Vector2 r) {
        float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
        return val == 0 ? 0 : (val > 0 ? 1 : -1);
    }

    private List<Vector2> UpScalePolygon(List<Vector2> polygon, float scale) {
        if (polygon == null || polygon.Count == 0) return new List<Vector2>();

        //Get center
        Vector2 centroid = Vector2.Zero;
        foreach (var point in polygon)
            centroid += point;
        centroid /= polygon.Count;

        //Scale polygon from center
        var scaledPolygon = new List<Vector2>();
        foreach (var point in polygon) {
            Vector2 direction = point - centroid;
            Vector2 scaledPoint = centroid + direction * scale;
            scaledPolygon.Add(scaledPoint);
        }

        return scaledPolygon;
    }

    private class YOrderComparer : IComparer<Vector2> {
        public int Compare(Vector2 v1, Vector2 v2) {
            return v1.Y.CompareTo(v2.Y);
        }
    }
}

