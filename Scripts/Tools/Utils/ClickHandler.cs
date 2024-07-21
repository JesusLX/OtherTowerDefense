using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler {
    // Referencia al script que contiene la acción
    public UnityEvent<GameObject> actionScript;

    public void OnPointerClick(PointerEventData eventData) {
        actionScript?.Invoke(this.gameObject);
    }

    private void OnMouseDown() {
        actionScript?.Invoke(this.gameObject);
        // Llama al método PerformAction del script ActionScript

    }
    private void OnMouseOver() {
    }

}
