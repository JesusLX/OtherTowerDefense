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
        // Puedes agregar alguna lógica aquí si necesitas hacer algo cuando el ratón sale del objeto
    }
    public void OnMouseUpAsButton() {
        hoverController.OnObjectClicked(this.gameObject);
    }
}
