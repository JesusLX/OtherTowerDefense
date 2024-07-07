using PiscolSystems.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
    public float timeBetweenShots;
    public float timeBetweenRounds;
    public int shotsPerRound;
    public GameObject target;
    public PoolSystemBase bulletsPool;
    private Coroutine shootCoroutine;
    private Damage damage;
    private IAttacker attacker;

    void OnPreShootAnimation() { }
    void OnShootAnimation() { }
    void OnPostShootAnimation() { }
    void OnPreRafagaAnimation() { }
    void OnRafagaAnimation() { }
    void OnPostRafagaAnimation() { }

    public void Init(Damage damage, IAttacker attacker) {
        this.damage = damage;
        this.attacker = attacker;
    }
    public void StartShoot(GameObject target) {
        this.target = target;
        if (shootCoroutine == null) {
            shootCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    public void StopShoot() {
        this.target = null;
        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }

    private IEnumerator ShootCoroutine() {
        while (true) {
            if (target != null) {
                OnPreRafagaAnimation();
                for (int i = 0; i < shotsPerRound; i++) {
                    OnPreShootAnimation();
                    OnShootAnimation();
                    var bullet = bulletsPool.Play(null, transform.position, Quaternion.identity).GameObject.GetComponent<Bullet>();
                    bullet.target = target;
                    bullet.Shoot(damage, attacker);
                    OnPostShootAnimation();
                    yield return new WaitForSeconds(timeBetweenShots);
                }
                OnPostRafagaAnimation();
                yield return new WaitForSeconds(timeBetweenRounds);
            } else {
                yield return null;
            }
        }
    }
}
