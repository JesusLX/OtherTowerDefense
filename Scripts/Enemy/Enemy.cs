using PiscolSystems.Pools;
using UnityEngine;

public class Enemy : MonoBehaviour {
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

    public void Init(Point target) {
        damageable.Init();
        moveToPoint.SetTargetPoint(target);
        moveToPoint.StartMoving();
        if (GetComponentInChildren<Canvas>().worldCamera == null) {
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }
    }
    public void Die(IAttacker attacker, Damageable damageable) {
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<PoolItem>().Kill();
    }

}
