using System.Collections;
using TMPro;
using UnityEngine;

public class BackPathBullet : Bullet {

    Point targetPoint;
    Vector3 offset = Vector3.up * .5f;
    float stoppingDistance = .1f;

    public override void Shoot(Damage damage, TowerController attacker) {
        base.Shoot(damage, attacker);
       
    }
    protected override IEnumerator Positione() {
        targetPoint = target.GetComponent<MoveToPoint>().targetPoint;
        if (targetPoint != null) {
            transform.LookAt(targetPoint.transform);
        }
        yield return null;
    }

    protected override void Move() {
        if (targetPoint != null) {
            Vector3 targetPosition = targetPoint.transform.position + offset;
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            if (distance < stoppingDistance) {
                Point nextPoint = targetPoint.GetPreviousPoint();
                if (nextPoint != null) {
                    targetPoint = nextPoint;
                    transform.LookAt(targetPoint.transform);
                } else {
                    Debug.Log("Sin target point "+ targetPoint.previousPoints, targetPoint);
                    Deactivate();
                }
            } else {
                direction.Normalize();
                transform.position += direction * speed * Time.deltaTime;
            }
        } else {
                    Debug.Log("Sin target point 2",this);
            Deactivate();
        }
    }

}
