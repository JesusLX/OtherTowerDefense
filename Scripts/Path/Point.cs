using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Point : MonoBehaviour
{
    public List<Point> points = new List<Point>();
    public UnityEvent<Point> OnGetNextPoint;

    public Point GetNextPoint(GameObject gameObject) {
        var point = points[Random.Range(0, points.Count)];
        OnGetNextPoint?.Invoke(point);
        return point;
    }
}
