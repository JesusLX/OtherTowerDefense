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
    }
    protected override IEnumerator Positione() {
        transform.rotation = attacker.shooter.transform.rotation;
        yield return null;

    }
    protected override void Move() {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Enemy>() != null) {
            OnHitTarget(other.gameObject.GetComponent<Damageable>());
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.GetComponent<Enemy>() != null) {
            if (canDamage)
                OnHitTarget(other.gameObject.GetComponent<Damageable>());
        }
    }
    
    protected override void Deactivate() {
        base.Deactivate();
    }
}
