using UnityEngine;

public class BouncingBullet : Bullet {
    protected override void Move() {
        if (target != null) {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    protected override void OnHitTarget(Damageable hitTarget) {
        base.OnHitTarget(hitTarget);
        // Find the next target to bounce to
        Collider[] targets = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider potentialTarget in targets) {
            if (potentialTarget.gameObject != hitTarget && potentialTarget.gameObject != gameObject) {
                target = potentialTarget.gameObject;
                break;
            }
        }
    }
}
