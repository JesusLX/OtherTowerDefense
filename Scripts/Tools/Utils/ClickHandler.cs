using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler {
    // Referencia al script que contiene la acci�n
    public UnityEvent<GameObject> actionScript;

    public void OnPointerClick(PointerEventData eventData) {
        actionScript?.Invoke(this.gameObject);
    }

    private void OnMouseDown() {
        actionScript?.Invoke(this.gameObject);
        // Llama al m�todo PerformAction del script ActionScript

    }
    private void OnMouseOver() {
    }

}
