using PiscolSystems.Pools;
using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour {
    public ShooterStats stats;
    public float timeBetweenShots;
    public float timeBetweenRounds;
    public int shotsPerRound;
    public bool randomTarget;
    public GameObject target;
    public PoolSystemBase bulletsPool;
    private Coroutine shootCoroutine;
    private Damage damage;
    private TowerController attacker;

    void OnPreShootAnimation() { }
    void OnShootAnimation() { }
    void OnPostShootAnimation() { }
    void OnPreRafagaAnimation() { }
    void OnRafagaAnimation() { }
    void OnPostRafagaAnimation() { }

    public void Init(Damage damage, TowerController attacker) {
        this.damage = damage;
        this.attacker = attacker;
        SetStats(stats);
    }

    private void SetStats(ShooterStats stats) {
        timeBetweenShots = stats.timeBetweenShots;
        timeBetweenRounds = stats.timeBetweenRounds;
        shotsPerRound = stats.shotsPerRound;
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
            if (target != null && target.activeSelf) {
                OnPreRafagaAnimation();
                for (int i = 0; i < shotsPerRound; i++) {
                    OnPreShootAnimation();
                    OnShootAnimation();
                    var bullet = bulletsPool.Play(null, transform.position, Quaternion.identity).GameObject.GetComponent<Bullet>();
                    if (randomTarget) {
                        if (attacker.detector.DetectedObjects.Count > 0) {
                            var tmpTarget = attacker.detector.DetectedObjects[Random.Range(0, attacker.detector.DetectedObjects.Count - 1)];
                            bullet.target = tmpTarget;
                        }
                    } else {
                        bullet.target = target;
                    }
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
