using UnityEngine;

public class CameraController : MonoBehaviour {
    public float panSpeed = 20f;          // Velocidad de desplazamiento de la cámara
    public float panBorderThickness = 10f; // Ancho del borde de la pantalla donde se activa el desplazamiento
    public float zoomSpeed = 20f;         // Velocidad de zoom de la cámara
    public float minY = 20f;              // Altura mínima de la cámara
    public float maxY = 120f;             // Altura máxima de la cámara

    public Vector2 panLimit;              // Límites del desplazamiento de la cámara

    void Update() {
        Vector3 pos = transform.position;

        // Movimiento de la cámara cuando el ratón se acerca a los bordes de la pantalla
        if (Input.mousePosition.y >= Screen.height - panBorderThickness) {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness) {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness) {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness) {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Zoom de la cámara usando el scroll del ratón
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;

        // Aplicar límites al movimiento de la cámara
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // Establecer la nueva posición de la cámara
        transform.position = pos;
        if(Input.GetKeyUp(KeyCode.C)) { 
        transform.position = new Vector3(0,pos.y,0);

        }
    }
}
