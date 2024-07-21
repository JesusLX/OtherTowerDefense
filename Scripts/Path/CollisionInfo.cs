using UnityEngine;

public struct CollisionInfo {
    public Vector3 point;
    public Door.DoorType objectType; // Puede ser "Door" o "Wall"
    public Vector3 direction; // La dirección del rayo
    public Door door;
    public CollisionInfo(Vector3 point, Door.DoorType objectType, Vector3 direction, Door door = null ) {
        this.point = point;
        this.objectType = objectType;
        this.direction = direction;
        this.door = door;
    }
    public static bool AreDirectionsOpposite(Vector3 dir1, Vector3 dir2, float tolerance = 0.001f) {
        // Normaliza los vectores
        Vector3 normalizedDir1 = dir1.normalized;
        Vector3 normalizedDir2 = dir2.normalized;

        // Calcula el producto escalar
        float dotProduct = Vector3.Dot(normalizedDir1, normalizedDir2);

        // Comprueba si el producto escalar está cercano a -1
        return Mathf.Abs(dotProduct + 1) < tolerance;
    }
}
