using UnityEngine;

public class MoveWithinCubeBounds : MonoBehaviour {
    public Vector3 relativePointA = new Vector3(-10f, 1f -10f);
    public Vector3 relativePointB = new Vector3(10f, 1f -10f);
    public Vector3 relativePointC = new Vector3(10f,1f, 10f);
    public Vector3 relativePointD = new Vector3(-10f,1f, 10f);
    public Vector3 relativePointE = new Vector3(-10f,5f, -10f);
    public Vector3 relativePointF = new Vector3(10f, 5f, -10f);
    public Vector3 relativePointG = new Vector3(10f, 5f, 10f);
    public Vector3 relativePointH = new Vector3(-10f,5f, 10f);

    public float speed = 5f;
    public float rotationSpeed = 2f;
    public float arrivalThreshold = 0.5f; // Umbral para considerar que el objeto ha llegado a la posici�n

    private Vector3 targetPosition;
    private Vector3 pointA, pointB, pointC, pointD, pointE, pointF, pointG, pointH;

    void Start() {
        // Calcular las posiciones absolutas basadas en la posici�n inicial del objeto
        Vector3 initialPosition = transform.position;
        pointA = initialPosition + relativePointA;
        pointB = initialPosition + relativePointB;
        pointC = initialPosition + relativePointC;
        pointD = initialPosition + relativePointD;
        pointE = initialPosition + relativePointE;
        pointF = initialPosition + relativePointF;
        pointG = initialPosition + relativePointG;
        pointH = initialPosition + relativePointH;

        // Establecer la primera posici�n objetivo
        SetRandomTargetPosition();
    }

    void Update() {
        // Rotar suavemente hacia la posici�n objetivo
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mover el objeto hacia adelante en su direcci�n actual
        transform.position += transform.forward * speed * Time.deltaTime;

        // Verificar si el objeto ha llegado a la posici�n objetivo
        if (Vector3.Distance(transform.position, targetPosition) <= arrivalThreshold) {
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition() {
        // Elegir una nueva posici�n aleatoria dentro del cubo definido por los puntos
        float randomX = Random.Range(pointA.x, pointC.x);
        float randomY = Random.Range(pointA.y, pointE.y);
        float randomZ = Random.Range(pointA.z, pointC.z);

        targetPosition = new Vector3(randomX, randomY, randomZ);
    }

    void OnDrawGizmos() {
        // Calcular las posiciones absolutas para dibujar los Gizmos si el juego no est� corriendo
        Vector3 initialPosition = Application.isPlaying ? transform.position : transform.position;
        Vector3 gizmoPointA = pointA;
        Vector3 gizmoPointB = pointB;
        Vector3 gizmoPointC = pointC;
        Vector3 gizmoPointD = pointD;
        Vector3 gizmoPointE = pointE;
        Vector3 gizmoPointF = pointF;
        Vector3 gizmoPointG = pointG;
        Vector3 gizmoPointH = pointH;

        Gizmos.color = Color.green;

        // Dibujar las l�neas de la base del cubo
        Gizmos.DrawLine(gizmoPointA, gizmoPointB);
        Gizmos.DrawLine(gizmoPointB, gizmoPointC);
        Gizmos.DrawLine(gizmoPointC, gizmoPointD);
        Gizmos.DrawLine(gizmoPointD, gizmoPointA);

        // Dibujar las l�neas de la parte superior del cubo
        Gizmos.DrawLine(gizmoPointE, gizmoPointF);
        Gizmos.DrawLine(gizmoPointF, gizmoPointG);
        Gizmos.DrawLine(gizmoPointG, gizmoPointH);
        Gizmos.DrawLine(gizmoPointH, gizmoPointE);

        // Dibujar las l�neas verticales que conectan la base con la parte superior
        Gizmos.DrawLine(gizmoPointA, gizmoPointE);
        Gizmos.DrawLine(gizmoPointB, gizmoPointF);
        Gizmos.DrawLine(gizmoPointC, gizmoPointG);
        Gizmos.DrawLine(gizmoPointD, gizmoPointH);

        // Opcional: dibujar esferas en cada punto para mayor visibilidad
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gizmoPointA, 0.1f);
        Gizmos.DrawSphere(gizmoPointB, 0.1f);
        Gizmos.DrawSphere(gizmoPointC, 0.1f);
        Gizmos.DrawSphere(gizmoPointD, 0.1f);
        Gizmos.DrawSphere(gizmoPointE, 0.1f);
        Gizmos.DrawSphere(gizmoPointF, 0.1f);
        Gizmos.DrawSphere(gizmoPointG, 0.1f);
        Gizmos.DrawSphere(gizmoPointH, 0.1f);

        // Dibujar la posici�n objetivo
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPosition, 0.2f);
    }
}
