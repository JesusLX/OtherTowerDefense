using UnityEngine;

public class CameraController : MonoBehaviour {
    public float panSpeed = 20f;          // Velocidad de desplazamiento de la c�mara
    public float panBorderThickness = 10f; // Ancho del borde de la pantalla donde se activa el desplazamiento
    public float zoomSpeed = 20f;         // Velocidad de zoom de la c�mara
    public float minY = 20f;              // Altura m�nima de la c�mara
    public float maxY = 120f;             // Altura m�xima de la c�mara

    public Vector2 panLimit;              // L�mites del desplazamiento de la c�mara

    void Update() {
        Vector3 pos = transform.position;

        // Movimiento de la c�mara cuando el rat�n se acerca a los bordes de la pantalla
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

        // Zoom de la c�mara usando el scroll del rat�n
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;

        // Aplicar l�mites al movimiento de la c�mara
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // Establecer la nueva posici�n de la c�mara
        transform.position = pos;
        if(Input.GetKeyUp(KeyCode.C)) { 
        transform.position = new Vector3(0,pos.y,0);

        }
    }
}
