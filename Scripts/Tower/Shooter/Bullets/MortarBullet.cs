using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MortarBullet : Bullet {

    private Vector3 targetPost;
    private Vector3 startPost;
    public float smooth = 5f;
    public float count = 0;
    public override void Shoot(Damage damage, TowerController attacker) {
        base.Shoot(damage, attacker);

        if (target != null) {
            targetPost = target.transform.position;
            transform.LookAt(targetPost);
            transform.LookAt(transform.position + Vector3.up);

        }
    }

    protected override void Move() {
        if (target != null) {
            count += 1.0f * Time.deltaTime;


        }
    }
    void SmoothLookAt(Vector3 target) {
        Vector3 dir = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }
}
