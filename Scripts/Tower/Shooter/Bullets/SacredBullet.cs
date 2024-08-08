using LemonTree.ParticlesPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredBullet : Bullet {

    public override void Shoot(Damage damage, TowerController attacker) {
        base.Shoot(damage, attacker);

    }
    protected override IEnumerator Positione() {

        if (target != null) {
            transform.position = target.gameObject.transform.position + Vector3.up * Random.Range(5f, 7f);
            transform.LookAt(target.transform.position);

            PSManager.Instance.Play(startParticles, null, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(.5f);

    }
    protected override void Move() {
        if (target != null) {
            transform.LookAt(target.transform.position);

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
