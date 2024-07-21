using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer :  MonoBehaviour, IAttacker
{
    private void Start() {
        
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision, collision.gameObject);
        Enemy enemy;
        if (collision.gameObject.TryGetComponent(out enemy)) {
            enemy.Die(this);
        }
    }
    private void OnTriggerEnter(Collider other) {
        Enemy enemy;
        if (other.gameObject.TryGetComponent(out enemy)) {
            enemy.Die(this);
        }
    }
}
