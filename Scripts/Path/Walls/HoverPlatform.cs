using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPlatform : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private HoverController hoverController;
    public float height;

    void Start() {
        hoverController = HoverController.Instance;
    }
    public void RemoveAllChildren() {
        // Iterar sobre todos los hijos y destruirlos
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
    public float Height => transform.position.y+height;

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
