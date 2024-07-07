using TMPro;
using UnityEngine;

public class StraightBullet : Bullet {

    public override void Shoot(Damage damage, IAttacker attacker) {
        base.Shoot(damage, attacker);
        if (target != null) {
            this.transform.LookAt(target.transform.position);
        }
    }

    protected override void Move() {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
