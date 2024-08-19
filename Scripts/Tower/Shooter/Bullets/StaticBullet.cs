using LemonTree.ParticlesPool;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StaticBullet : Bullet {
    private bool canDamage;
    Coroutine damageCoroutine;
    public override void Shoot(Damage damage, TowerController attacker) {
        base.Shoot(damage, attacker);
        this.transform.parent = attacker.shooter.transform;
        this.transform.localRotation = Quaternion.identity;
        // Guardar la posición trasera (el origen) del objeto
        Vector3 originalPosition = transform.position - transform.forward * transform.localScale.z / 2;

        // Aumentar la escala en el eje Z
        transform.localScale += new Vector3(0, 0, this.attacker.detector.detectionRadius);

        // Ajustar la posición para que el objeto crezca hacia adelante
        transform.position = originalPosition + transform.forward * transform.localScale.z / 2;
    }
    protected override IEnumerator Positione() {
        yield return null;

    }
    protected override void Move() {
        //transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.GetComponent<Enemy>() != null) {
                OnHitTarget(other.gameObject.GetComponent<Damageable>());
            if (canDamage) {
            }
        }
    }

    protected override void Deactivate() {
        base.Deactivate();
    }
}
