using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasicCharacterController : MonoBehaviour {
    public float moveSpeed = 5f;          // Velocidad de movimiento
    public float jumpHeight = 2f;         // Altura de salto
    public float gravity = -9.81f;        // Fuerza de gravedad
    public float mouseSensitivity = 100f; // Sensibilidad del ratón

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool inUse;

    private float xRotation = 0f;

    void Start() {
        controller = GetComponent<CharacterController>();
        // Bloquear el cursor en el centro de la pantalla
    }
    [ContextMenu("ToggleInUse")]
    void ToggleInUse() {
        SetInUse(!inUse);
    }
    private void SetInUse(bool use) {
        inUse = use;
        if (use) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update() {
        if(Input.GetKeyUp(KeyCode.LeftControl)) { 
            ToggleInUse();
        }
        if (inUse) {
            // Controlar la rotación de la cámara con el ratón
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseX);

            // Comprobar si el personaje está en el suelo
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0) {
                velocity.y = -2f;
            }

            // Movimiento del personaje con WASD
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * moveSpeed * Time.deltaTime);

            // Saltar con la tecla Espacio
            if (Input.GetButtonDown("Jump") && isGrounded) {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Aplicar gravedad
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
