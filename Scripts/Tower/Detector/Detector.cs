using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Detector : MonoBehaviour {
    public float detectionRadius = 5f;
    public LayerMask detectionLayer;

    private bool isSearching = false;
    private SphereCollider detectionCollider;
    private List<GameObject> detectedObjects = new List<GameObject>();
    public List<GameObject> DetectedObjects => GetDetectedObjects();

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
    public List<GameObject> GetDetectedObjects() {
        detectedObjects.RemoveAll(x => !x.activeSelf);
        return detectedObjects;
    }
    private void OnTriggerEnter(Collider other) {
        if (isSearching) {
            if (((1 << other.gameObject.layer) & detectionLayer) != 0) {
                if (!detectedObjects.Contains(other.gameObject)) {
                    detectedObjects.Add(other.gameObject);
                    OnObjectDetected?.Invoke(detectedObjects);
                    Damageable tmpDmg;
                    if (other.TryGetComponent<Damageable>(out tmpDmg)) {
                        tmpDmg.onDied.AddListener(OnTargetDied);
                    }
                }
            }
        }
    }
    public void OnTargetDied(IAttacker attacker, Damageable damageable) {
        detectedObjects.Remove(damageable.gameObject);
        OnObjectLost?.Invoke(detectedObjects);
    }

    private void OnTriggerExit(Collider other) {
        if (isSearching) {
            if (((1 << other.gameObject.layer) & detectionLayer) != 0) {
                if (detectedObjects.Contains(other.gameObject)) {
                    detectedObjects.Remove(other.gameObject);
                    detectedObjects.RemoveAll(x => !x.gameObject.activeSelf);
                    OnObjectLost?.Invoke(detectedObjects);
                    Damageable tmpDmg;
                    if (other.TryGetComponent<Damageable>(out tmpDmg)) {
                        tmpDmg.onDied.RemoveListener(OnTargetDied);
                    }
                }
            }
        }
    }
}
