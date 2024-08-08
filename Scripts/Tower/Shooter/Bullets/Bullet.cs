using LemonTree.ParticlesPool;
using PiscolSystems.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {

    public string endParticles;
    public string startParticles;

    public GameObject target;
    public float speed;
    public float lifetime;
    public int maxTargets;

    private float timeAlive;
    protected int targetsHit;
    private Coroutine moveCoroutine;

    protected Damage damage;
    public List<string> particlesTrails;
    public List<ParticleSystem> particles;
    protected TowerController attacker;
    public virtual void Shoot(Damage damage, TowerController attacker) {
        timeAlive = 0f;
        targetsHit = 0;
        this.damage = damage;
        this.attacker = attacker;

        if (particlesTrails.Count > 0) {
            foreach (var p in particlesTrails) {
                particles.Add(PSManager.Instance.Play(p, transform, Vector3.zero, transform.rotation));
            }
        }
        if (startParticles != "") {
            PSManager.Instance.Play(startParticles, attacker.shooter.transform, Vector3.zero, attacker.shooter.transform.rotation);
        }
        moveCoroutine = StartCoroutine(LifeCycleCoroutine());
    }

    protected virtual void OnDestroy() {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
    }

    private IEnumerator LifeCycleCoroutine() {
        yield return StartCoroutine(Positione());
        while (timeAlive < lifetime) {
            Move();
            timeAlive += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Deactivate();
    }
    protected virtual IEnumerator Positione() {
        yield return null;
    }

    protected abstract void Move();

    protected virtual void OnTriggerEnter(Collider other) {
        Damageable hit;
        if (other.gameObject.TryGetComponent<Damageable>(out hit)) {
            OnHitTarget(hit);
            targetsHit++;
            if (targetsHit >= maxTargets) {
                Deactivate();
            }
        }
    }

    protected virtual void OnHitTarget(Damageable hitTarget) {
        Debug.Log(hitTarget, hitTarget);
        hitTarget.getHit(this.damage, this.attacker);
    }
    private void Update() {
        if (target != null) {
            if (!target.activeSelf) {
                target = null;
            }
        }
    }
    protected virtual void Deactivate() {
        particles.ForEach(p => { if (p != null) { p.transform.parent = null; PSManager.Instance.StopParticleSystem(p); } });
        particles.Clear();
        gameObject.SetActive(false);
        if (gameObject.activeSelf) {
            GetComponent<IPoolItem>().Kill();
            if (endParticles != "") {
                PSManager.Instance.Play(endParticles, null, transform.position, transform.rotation);
            }
        }

    }

}
