using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


public class Point : MonoBehaviour {
    public List<Point> points = new List<Point>();
    public UnityEvent<Point> OnGetNextPoint;
    public enum PointType {
        Path, Door, Intersection
    }
    public PointType pointType = PointType.Path;
    public Point GetNextPoint(GameObject gameObject) {
        Point point = null;
        if (points.Count > 0) {

            point = points[Random.Range(0, points.Count)];
            OnGetNextPoint?.Invoke(point);
        } else {
            point = this;
        }

        return point;
    }
}
