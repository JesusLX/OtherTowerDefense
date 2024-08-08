using UnityEngine;

public class InfiniteRotation : MonoBehaviour {
    public float rotationSpeed = 45f; // Velocidad de rotación en grados por segundo

    void Update() {
        // Rotar el objeto en el eje Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
