using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using static Point;

public class BlockTerrain : MonoBehaviour {
    public List<Door> doors;

    [ContextMenu("GetDoorsCount")]
    public int GetDoorsCount() {

        return doors.Count;
    }
    public int GetActiveDoorsCount() {

        return doors.FindAll(d => d.active).Count;
    }

    [ContextMenu("HasOppositeDoors")]
    public bool HasOppositeDoors() {
        Door northDoor = doors.Find(d => d.doorPosition == Door.DoorPosition.North);
        Door southDoor = doors.Find(d => d.doorPosition == Door.DoorPosition.South);
        Door eastDoor = doors.Find(d => d.doorPosition == Door.DoorPosition.East);
        Door westDoor = doors.Find(d => d.doorPosition == Door.DoorPosition.West);
        var result = (northDoor != null && southDoor != null) || (eastDoor != null && westDoor != null);
        Debug.Log(result.ToString());
        return result;
    }
    public Door GetExitDoor() {
        Door door = doors.Find(d => d != null && d.doorType == Door.DoorType.Exit);
        return door;
    }
    public void OnDoorClicked(Door door) {
        if (door.active) {
            Debug.Log("Click " + door, door);
            TerrainManager.Instance.OnDoorClicked(door);
        }
    }
    [ContextMenu("CheckDoors")]
    public void Test() {
        CheckDoors(2);
    }
    public bool CheckDoors(int doorsCount) {
        //Debug.Log(doorsCount + " Total", this);
        int airTouch = 0;
        int activeDoorsCount = GetActiveDoorsCount();
        GetActiveDoors().ForEach(d => {
            if (d != null) {
                var hit = d.CheckTouchDoorOrWall();
                if (hit == Door.DoorType.Exit || hit == Door.DoorType.Entrance) {
                    doorsCount--;
                    activeDoorsCount--;
                    //Debug.Log(doorsCount + " " + hit + "-" + d.doorType + " Cuenta " + d.transform.position, d);
                } else {
                    if (hit == Door.DoorType.None) {
                        //Debug.Log(doorsCount + " " + hit + "-" + d.doorType + " AIRE Cuenta" + d.transform.position, d);
                        airTouch++;

                    } else {
                        //Debug.Log(doorsCount + " " + hit + "-" + d.doorType + " No Cuenta" + d.transform.position, d);
                    }

                }
            }
        }
        );
        if (doorsCount == 0) {
            //Debug.Log(activeDoorsCount + " - " + airTouch);
            return (activeDoorsCount - airTouch) <= 0;
        }
        return false;
    }
    public Door TouchDoor(Door door) {
        var _door = doors.Find(d => {
            if (d != null) {
                bool hit = d.TouchDoor(door);
                if (hit) {
                    return d;
                }
            }
            return false;
        }
        );
        return _door;
    }
    public List<Door> TouchDoors() {
        List<Door> _door = doors.FindAll(d => {
            if (d != null) {
                bool hit = d.TouchDoor();
                if (hit) {
                    return d;
                }
            }
            return false;
        }
        );
        return _door;
    }
    public List<Door> GetActiveDoors() {
        List<Door> _door = doors.FindAll(d => d.active && d.doorType != Door.DoorType.Portal);
        return _door;
    }
    public void ResetDoors() {
        doors.ForEach(d => { if (d.doorType != Door.DoorType.Portal) d.doorType = Door.DoorType.Entrance; });
    }
    public void DeactiveTouchedOcupedDoors() {
        doors.ForEach(d => d.GetTouchingDoors().ForEach(sd => sd.terrain.DeactiveOcupedDoors()));
    }
    [ContextMenu("DeactiveOcupedDoors")]
    public void DeactiveOcupedDoors() {
        foreach (var door in GetActiveDoors()) {
            var doorType = door.CheckTouchDoorOrWall();
            if (doorType == Door.DoorType.Exit || doorType == Door.DoorType.Entrance) {
                //Debug.Log("DeactiveOcupedDoors DENTRO " + doorType, door);
                door.Desactivate();
            }
        }
    }
    public void RePath() {
        var exitDoor = GetExitDoor().point;
        InvertPointChain(exitDoor);
    }
    public static List<Point> InvertPointChain(Point startPoint) {
        List<Point> originalChain = new List<Point>();
        HashSet<Point> visitedPoints = new HashSet<Point>();
        CollectPoints(startPoint, originalChain, visitedPoints);

        // Invertir la lista
        originalChain.Reverse();

        // Reasignar referencias en la lista invertida
        for (int i = 0; i < originalChain.Count; i++) {
            originalChain[i].points.Clear();
            if (i + 1 < originalChain.Count) {
                originalChain[i].points.Add(originalChain[i + 1]);
            }
        }

        return originalChain;
    }

    // Método recursivo para recoger todos los puntos en la cadena
    private static void CollectPoints(Point currentPoint, List<Point> chain, HashSet<Point> visitedPoints) {
        if (currentPoint == null || visitedPoints.Contains(currentPoint)) {
            return;
        }

        visitedPoints.Add(currentPoint);
        chain.Add(currentPoint);

        // Detenerse en puntos de tipo Intersección
        if (currentPoint.pointType == PointType.Intersection) {
            return;
        }

        foreach (Point nextPoint in currentPoint.points) {
            CollectPoints(nextPoint, chain, visitedPoints);
        }
    }
}
