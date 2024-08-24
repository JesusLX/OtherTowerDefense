using PiscolSystems.Pools;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    Damageable damageable;
    MoveToPoint moveToPoint;
    Effectable effectable;
    public MovementStats movementStats;
    void OnEnable() {
        damageable.onDied.AddListener(this.Die);
    }

    void OnDisable() {
        damageable.onDied.RemoveListener(this.Die);
    }
    private void Awake() {
        damageable = GetComponent<Damageable>();
        moveToPoint = GetComponent<MoveToPoint>();
        effectable = GetComponent<Effectable>();
    }

    public void Init(Point target) {
        damageable.Init();
        moveToPoint.SetTargetPoint(target);
        moveToPoint.SetStats(movementStats);
        moveToPoint.StartMoving();
        if (GetComponentInChildren<Canvas>().worldCamera == null) {
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }
    }
    public void Die(IAttacker attacker, Damageable damageable) {
        if(effectable != null) {
            this.effectable.RemoveEffects();
        }
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<PoolItem>().Kill();
    }

    internal MovementStats GetMovStats() {
        return moveToPoint.movStat;
    }
}
