using UnityEngine;

public class DirectBullet : Bullet {
    protected override void Move() {
        if (target != null) {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
