using System.Collections.Generic;
using UnityEngine;

public class TerrainChecker : MonoBehaviour {
    public float rayDistance = 5f;
    public LayerMask detectableLayers;

    [ContextMenu("TEST")]
    public void CheckWOPos() {
        List<CollisionInfo> collisionInfos = new List<CollisionInfo>();


        // Lanza los raycasts en las direcciones especificadas
        for (int i = 0; i < 4; i++) {
            RaycastHit[] hits;
            Vector3 direction = GetDirection(i);
            Debug.Log(Physics.RaycastAll(this.transform.position, direction, rayDistance, detectableLayers, QueryTriggerInteraction.Collide).Length);
            if ((hits = Physics.RaycastAll(this.transform.position, direction, rayDistance, detectableLayers, QueryTriggerInteraction.Collide)).Length > 0) {
                foreach (var collider in hits) {
                    Door doorScript = collider.collider.GetComponent<Door>();
                    if(doorScript != null) {

                     Debug.Log(collider.transform.position + ", " 
                        + doorScript.doorType + ", " 
                        + direction + ", " 
                        + doorScript,
                        collider.transform);
                    } else {
                        Debug.Log(collider.transform.position + ", "
                           + collider.transform.gameObject+ ", "
                           + direction                            ,
                           collider.transform);

                    }
                }
            } else {
                Debug.Log(this.transform.position + ", " + Door.DoorType.None + ", " + direction);
            }
        }

    }
    public List<CollisionInfo> Check(Vector3 spawnPosition) {
        List<CollisionInfo> collisionInfos = new List<CollisionInfo>();

        this.transform.position = spawnPosition;
        // Lanza los raycasts en las direcciones especificadas
        for (int i = 0; i < 4; i++) {
            RaycastHit[] hits;
            Vector3 direction = GetDirection(i);
            if ((hits = Physics.RaycastAll(this.transform.position, direction, rayDistance, detectableLayers, QueryTriggerInteraction.Collide)).Length > 0) {
                DoorHandle(hits, direction, collisionInfos);
            } else {
                collisionInfos.Add(new CollisionInfo(this.transform.position, Door.DoorType.None, direction));

            }
        }

        return collisionInfos;
    }
    public static Collider[] OverlapSphere(Vector3 position, float rayDistance, LayerMask detectableLayers) {
        return Physics.OverlapSphere(position, rayDistance, detectableLayers, QueryTriggerInteraction.Collide);
    }
    public static void DoorHandle(Collider[] colliders, Vector3 direction, List<CollisionInfo> collisionInfos) {
        foreach (var collider in colliders) {
            Door doorScript = collider.GetComponent<Door>();

            if (doorScript != null) {
                collisionInfos.Add(new CollisionInfo(collider.transform.position, doorScript.doorType, direction, doorScript));
            } else {
                collisionInfos.Add(new CollisionInfo(collider.transform.position, Door.DoorType.Wall, direction));
            }
        }
    }
    public static void DoorHandle(RaycastHit[] colliders, Vector3 direction, List<CollisionInfo> collisionInfos) {
        int walls = 0;
        foreach (var collider in colliders) {
            Door doorScript = collider.collider.GetComponent<Door>();

            if (doorScript != null) {
                collisionInfos.Add(new CollisionInfo(collider.transform.position, doorScript.doorType, direction, doorScript));
            } else {
                if (walls < 1) {
                walls++;

                collisionInfos.Add(new CollisionInfo(collider.transform.position, Door.DoorType.Wall, direction));
                }
            }
        }
    }

    private Vector3 GetDirection(int index) {
        float angle = index * 90;
        return Quaternion.Euler(0, angle, 0) * this.transform.forward;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        // Define las direcciones en las que lanzar los raycasts
        Vector3 forward = this.transform.forward;
        Vector3 right = this.transform.right;
        Vector3 backward = -forward;
        Vector3 left = -right;

        Gizmos.DrawLine(this.transform.position, this.transform.position + forward * rayDistance);
        Gizmos.DrawLine(this.transform.position, this.transform.position + right * rayDistance);
        Gizmos.DrawLine(this.transform.position, this.transform.position + backward * rayDistance);
        Gizmos.DrawLine(this.transform.position, this.transform.position + left * rayDistance);
    }
}
