using PiscolSystems.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {


    public GameObject target;
    public float speed;
    public float lifetime;
    public int maxTargets;

    private float timeAlive;
    private int targetsHit;
    private Coroutine moveCoroutine;

    protected Damage damage;
    protected IAttacker attacker;
    public virtual void Shoot(Damage damage, IAttacker attacker) {
        timeAlive = 0f;
        targetsHit = 0;
        this.damage = damage;
        this.attacker = attacker;
        moveCoroutine = StartCoroutine(LifeCycleCoroutine());
    }

    protected virtual void OnDestroy() {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
    }

    private IEnumerator LifeCycleCoroutine() {
        while (timeAlive < lifetime) {
            Move();
            timeAlive += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Deactivate();
    }

    protected abstract void Move();

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject == target) {
            OnHitTarget(other.gameObject.GetComponent<Damageable>());
            targetsHit++;
            if (targetsHit >= maxTargets) {
                Deactivate();
            }

        }
    }

    protected virtual void OnHitTarget(Damageable hitTarget) {
        hitTarget.getHit(this.damage, this.attacker);
    }

    protected virtual void Deactivate() {
        gameObject.SetActive(false);
        GetComponent<IPoolItem>().Kill();
    }

}
