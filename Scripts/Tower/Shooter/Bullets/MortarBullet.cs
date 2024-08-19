using System.Collections;
using System.Net;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MortarBullet : Bullet {
    public float initialRotationSpeed = 6f; // Velocidad de rotación inicial
    public float maxRotationSpeed = 40f;   // Velocidad de rotación máxima
    public float closeDistance = 5f;
    Vector3 targetPosition;
    Vector3 mediumTargetPosition;
    float currentSpeed;
    protected override IEnumerator Positione() {
        transform.LookAt(target.transform);
        transform.LookAt(transform.position + Vector3.up);
        targetPosition = target.transform.position;
        Vector3 mediumTargetPosition = (transform.position + targetPosition) / 2 + Vector3.up * Vector3.Distance(transform.position, targetPosition);
        currentSpeed = speed;
        float dist = 0;
        while ((dist = Vector3.Distance(transform.position, mediumTargetPosition)) > 0.1f) {
            // Calcular la dirección hacia la posición objetivo
            Vector3 direction = (mediumTargetPosition - transform.position).normalized;

            // Calcular la rotación hacia la dirección del objetivo
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Calcular la distancia actual al objetivo
            float distanceToTarget = Vector3.Distance(transform.position, mediumTargetPosition);

            // Aumentar la velocidad de rotación cuanto más cerca esté del objetivo
            float t = Mathf.Clamp01((closeDistance - distanceToTarget) / closeDistance);
            float rotationSpeed = Mathf.Lerp(initialRotationSpeed, maxRotationSpeed, t);

            // Rotar suavemente hacia la posición objetivo con la velocidad ajustada
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if(dist < 2) {
                currentSpeed = Mathf.Max(currentSpeed-0.1f, 1f);
            }
            // Mover el objeto hacia adelante en su dirección actual
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            // Esperar hasta el siguiente frame antes de continuar
            yield return null;
        }

        // Asegurarse de que el objeto esté exactamente en la posición objetivo
        transform.position = mediumTargetPosition;
        if (target == null) {
            Deactivate();
        }
        // Asegurarse de que el objeto esté exactamente en la posición objetivo
        transform.position = mediumTargetPosition;
    }
    protected override void Move() {
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calcular la rotación hacia la dirección del objetivo
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Calcular la distancia actual al objetivo
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Aumentar la velocidad de rotación cuanto más cerca esté del objetivo
        float t = Mathf.Clamp01((closeDistance - distanceToTarget) / closeDistance);
        float rotationSpeed = Mathf.Lerp(initialRotationSpeed, maxRotationSpeed, t);

        // Rotar suavemente hacia la posición objetivo con la velocidad ajustada
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        currentSpeed = Mathf.Min(currentSpeed + 0.05f, speed);

        // Mover el objeto hacia adelante en su dirección actual
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
}
