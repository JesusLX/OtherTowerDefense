using UnityEngine;
using UnityEngine.EventSystems;

public class HoverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private HoverController hoverController;

    void Start() {
        hoverController = HoverController.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hoverController.MoveHoverObject(this.transform);
    }
    private void OnMouseEnter() {
        hoverController.MoveHoverObject(this.transform);
    }
    public void OnPointerExit(PointerEventData eventData) {
        // Puedes agregar alguna l�gica aqu� si necesitas hacer algo cuando el rat�n sale del objeto
    }
    public void OnMouseUpAsButton() {
        hoverController.OnObjectClicked(this.gameObject);
    }
}
