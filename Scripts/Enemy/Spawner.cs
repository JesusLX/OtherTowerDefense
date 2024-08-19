using PiscolSystems.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PoolSystemBase enemyPool;
    public int rafaga;
    public int rafagaCount;
    public Point target;

    public float timeBetweenShots = 0.5f;
    public float timeBetweenRafagas = 0.5f;

    [ContextMenu("Spawn")]
    public void Spawn() {
        StartCoroutine(SpawnRafaga());
    }
    public void Spawn(string key) {
        EnemyManager.Instance.Spawn(key,this.transform).GetComponent<Enemy>().Init(target);
    }
    public IEnumerator SpawnRafaga() {
        for (int i = 0; i < rafagaCount; i++) {
            for (int j = 0; j < rafaga; j++) {
                var enemy = enemyPool.Play(transform,transform.position,transform.rotation).GameObject.GetComponent<Enemy>();
                enemy.Init(target);
                
                yield return new WaitForSeconds(timeBetweenShots);
            }
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
