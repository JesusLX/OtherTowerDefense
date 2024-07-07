using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Damageable damageable;
    MoveToPoint moveToPoint;
    void OnEnable() {
        damageable.onDied.AddListener(this.Die);
    }

    void OnDisable() {
        damageable.onDied.RemoveListener(this.Die);
    }
    private void Awake() {
        damageable = GetComponent<Damageable>();
        moveToPoint = GetComponent<MoveToPoint>();
    }
    private void Start() {
        Init();
    }
    public void Init() {
        damageable.Init();
        moveToPoint.StartMoving();
    }
    public void Die(IAttacker attacker) {
        this.gameObject.SetActive(false);
    }
}
