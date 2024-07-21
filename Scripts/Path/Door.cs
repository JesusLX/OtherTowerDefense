using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public Point point;
    public Spawner spawner;
    public BlockTerrain terrain;

    public float rayDistance = 1f;
    public LayerMask detectableLayers;

    public bool active = true;

    public enum DoorPosition {
        North, South, East, West
    }
    public enum DoorType {
        Entrance, Exit, Wall, None, Portal
    }
    public DoorPosition doorPosition;
    public DoorType doorType;
   
 
    public DoorType CheckTouchDoorOrWall() {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position + transform.forward;
        //Debug.Log(endPos, this);
        //Debug.Log(terrain, terrain);
        //Vector3 endPos = transform.forward;
        Collider[] hits;
        List<CollisionInfo> collisionInfos = new List<CollisionInfo>();
        if ((hits = TerrainChecker.OverlapSphere(startPos, rayDistance, detectableLayers)).Length > 0) {

            TerrainChecker.DoorHandle(hits, endPos, collisionInfos);
            //Debug.Log("Hits: "+hits.Length+" "+ startPos, this);
            //Debug.Log("Collisions "+collisionInfos.Count, this);

            foreach (var collisionInfo in collisionInfos) {
                if (collisionInfo.door != this) {
                    //Debug.Log(this + " Tocando " + collisionInfo.door, this);
                    //Debug.Log(this + " Tocando " + collisionInfo.door, collisionInfo.door);
                    //Debug.Log(" ----------------", collisionInfo.door);
                    return collisionInfo.objectType;
                } else {
                    //Debug.Log(this + " Tocando el mismo " + collisionInfo.door, this);
                    //Debug.Log(this + " Tocando el mismo " + collisionInfo.door, collisionInfo.door);
                    //Debug.Log(this + " Tocando el mismo " + (collisionInfo.door == this), collisionInfo.door);
                    //Debug.Log(" ----------------", collisionInfo.door);
                }
            }
        }
        return DoorType.None;
    }
    public bool TouchDoor(Door toFind) {
        Vector3 startPos = transform.position;
        //Vector3 endPos = transform.position + transform.forward;
        Vector3 endPos = transform.forward;

        Collider[] hits;
        List<CollisionInfo> collisionInfos = new List<CollisionInfo>();
        Debug.DrawLine(startPos, endPos, Color.red, 2.0f);
        Ray ray = new Ray(startPos, endPos);

        if ( (hits = TerrainChecker.OverlapSphere(startPos, rayDistance, detectableLayers )).Length > 0) {

            TerrainChecker.DoorHandle(hits, endPos, collisionInfos);
            foreach (var collisionInfo in collisionInfos) {
                if (collisionInfo.door == toFind) {
                    return true;
                }
            }
        }
        return false;
    }
    public bool TouchDoor() {
        Vector3 startPos = transform.position;
        //Vector3 endPos = transform.position + transform.forward;
        Vector3 endPos = transform.forward;

        Collider[] hits;
        List<CollisionInfo> collisionInfos = new List<CollisionInfo>();

        if ((hits = TerrainChecker.OverlapSphere(transform.position, rayDistance, detectableLayers)).Length > 0) {

            TerrainChecker.DoorHandle(hits, endPos, collisionInfos);
            foreach (CollisionInfo collisionInfo in collisionInfos) {
                if (collisionInfo.door != this) {
                    return true;
                }
            }
        }
        return false;
    }
    public List<Door> GetTouchingDoors() {
        Vector3 startPos = transform.position;
        //Vector3 endPos = transform.position + transform.forward;
        Vector3 endPos = transform.forward;

        Collider[] hits;
        List<CollisionInfo> collisionInfos = new List<CollisionInfo>();
        List<Door> doors = new List<Door>();
        if ((hits = TerrainChecker.OverlapSphere(startPos, rayDistance, detectableLayers)).Length > 0) {

            foreach (Collider collisionInfo in hits) {
                if (collisionInfo.GetComponent<Door>() != this) {
                    doors.Add(collisionInfo.GetComponent<Door>());
                }
            }
        }
        return doors;
    }
    private void OnDrawGizmos() {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.forward;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawSphere(transform.position, rayDistance);
    }
    public void Desactivate() {
        if (active) {
            active = false;
            spawner.gameObject.SetActive(false);
            GetComponentInChildren<ClickHandler>().gameObject.SetActive(false);
        }
    }
}
