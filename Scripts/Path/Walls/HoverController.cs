using UnityEngine;
using UnityEngine.Events;

public class HoverController : Singleton<HoverController> {
    public GameObject hoverObject; // El objeto que se moverá
    public GameObject radius;
    public UnityEvent<GameObject> onObjectClicked;
    public bool IsActive { get; private set; } = false;

    public void MoveHoverObject(Transform target) {
        if (IsActive) {
            hoverObject.SetActive(true);
            // Mueve el objeto flotante justo por encima del objeto objetivo
            Vector3 newPosition = target.position;
            newPosition.y += target.GetComponent<Renderer>().bounds.extents.y + hoverObject.GetComponent<Renderer>().bounds.extents.y;
            hoverObject.transform.position = newPosition;
        }
    }

    public void HideHoverObject() {
        hoverObject.SetActive(false);
    }

    public void Activate() {
        IsActive = true;
    }
    public void SetRadius(float radiusSize) {
        radius.transform.localScale = Vector3.one * radiusSize * 2;
    }

    public void Deactivate() {
        IsActive = false;
        HideHoverObject();
    }

    public void OnObjectClicked(GameObject clickedObject) {
        if (IsActive) {

            Debug.Log("Clicked on: " + clickedObject.name);
            onObjectClicked?.Invoke(clickedObject);
            // Aquí puedes agregar la lógica que necesites cuando se hace clic en un objeto
            Deactivate();
        }
    }
}
