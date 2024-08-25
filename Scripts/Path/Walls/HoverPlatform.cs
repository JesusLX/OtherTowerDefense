using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPlatform : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private HoverController hoverController;
    public float height;
    public GameObject occupier;

    void Start() {
        hoverController = HoverController.Instance;
        float rnd = UnityEngine.Random.Range(0f, 1f);
        if (rnd < .02f) {
            RandomObjectGenerator.Instance.InstantiateRandom(this);
        }
    }
    public void RemoveAllChildren() {
        // Iterar sobre todos los hijos y destruirlos
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
    public float Height => transform.position.y+height;
    public Vector3 Position => transform.position + (Vector3.up * height);

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

    internal bool IsOccupied() {
        return occupier != null;
    }

    public void SetOccupier(GameObject occupier) {
        this.occupier = occupier;
    }
 
}
