using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Detector : MonoBehaviour {
    public float detectionRadius = 5f;
    public LayerMask detectionLayer;

    private bool isSearching = false;
    private SphereCollider detectionCollider;
    private List<GameObject> detectedObjects = new List<GameObject>();
    public List<GameObject> DetectedObjects => detectedObjects;

    // Eventos para notificar a los suscriptores
    public UnityEvent<List<GameObject>> OnObjectDetected;
    public UnityEvent<List<GameObject>> OnObjectLost;

    void Start() {
        detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;
    }

    [ContextMenu("StartSearching")]
    public void StartSearching() {
        if (!isSearching) {
            isSearching = true;
        }
    }

    public void StopSearching() {
        if (isSearching) {
            isSearching = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (isSearching) {
            if (((1 << other.gameObject.layer) & detectionLayer) != 0) {
                if (!detectedObjects.Contains(other.gameObject)) {
                    detectedObjects.Add(other.gameObject);
                    OnObjectDetected?.Invoke(detectedObjects);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (isSearching) {
            if (((1 << other.gameObject.layer) & detectionLayer) != 0) {
                if (detectedObjects.Contains(other.gameObject)) {
                    detectedObjects.Remove(other.gameObject);
                    OnObjectLost?.Invoke(detectedObjects);
                }
            }
        }
    }
}
