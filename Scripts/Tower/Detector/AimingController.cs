using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class AimingController : MonoBehaviour {
    public Detector detector;
    public float aimSpeed = 2.0f;
    private GameObject currentTarget;
    private List<GameObject> targetList;
    private Coroutine aimCoroutine;
    public UnityEvent<GameObject> OnTargetSelected;
    public UnityEvent OnTargetLost;



    [ContextMenu("StartAiming")]
    public void StartAiming() {
        if (aimCoroutine == null) {
            aimCoroutine = StartCoroutine(AimCoroutine());
        }
    }

    public void StopAiming() {
        if (aimCoroutine != null) {
            StopCoroutine(aimCoroutine);
            aimCoroutine = null;
        }
    }

    public void SetTarget(List<GameObject> targetList) {
        this.targetList = targetList;
        if (!targetList.Contains(currentTarget) || currentTarget == null) {
            if (targetList.Count > 0) {
                currentTarget = targetList[0];
                this.OnTargetSelected?.Invoke(currentTarget);
            } else {
                currentTarget = null;
                this.OnTargetLost?.Invoke();
            }
        }
    }

    private IEnumerator AimCoroutine() {
        while (true) {
            if (currentTarget != null && currentTarget.activeSelf) {
                // Apunta hacia el objetivo
                Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
                Vector3 direction = currentTarget.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * aimSpeed);
            } else if (targetList != null && targetList.Count > 0) {
                targetList.RemoveAll(obj => !obj.activeSelf);
                SetTarget(targetList);
            }
            yield return null;
        }
    }
}
