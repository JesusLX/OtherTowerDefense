using LemonTree.ParticlesPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectBullet : Bullet {
    public Transform body1;
    public Transform body2;
    public override void Shoot(Damage damage, TowerController attacker) {
        base.Shoot(damage, attacker);
        particles = new List<ParticleSystem> {
            PSManager.Instance.Play("blue_fire", body1, Vector3.zero, body1.rotation),
            PSManager.Instance.Play("purple_fire", body2, Vector3.zero, body2.rotation)
        };
    }
    protected override IEnumerator Positione() {
        Vector3 targetPosition = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(.5f, 2f), Random.Range(-2f, 2f));
        Vector3 direction = (targetPosition - transform.position).normalized;
        float speed = 2f; // 1 segundos
        float startTime = Time.time;
        while (Time.time - startTime < 1f) {
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
        if(target != null)
        transform.LookAt(target.transform.position);

    }
    protected override void Move() {
        if (target != null) {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        } else {
            if (attacker.detector.DetectedObjects.Count > 0) {
                target = attacker.detector.DetectedObjects[0];
            } else {
                Deactivate();
            }
        }
    }
  
}
