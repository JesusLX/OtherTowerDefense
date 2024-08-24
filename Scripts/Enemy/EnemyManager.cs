using System.Collections.Generic;
using UnityEngine;
using PiscolSystems.Pools;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class EnemyManager : Singleton<EnemyManager> {
    public List<PoolSystemBase> enemyPools;
    public List<EnemyGroup> enemyGroups;

    public GameObject Spawn(string id, Transform doorPos) {
        PoolSystemBase pool = enemyPools.Find(p => p.Id == id);
        GameObject enemy = null;
        if (pool != null) {
            enemy = pool.Play(null, doorPos.position, doorPos.rotation).GameObject;
        }
        return enemy;
    }
    public EnemyGroup GetRandomGroup() {
        return enemyGroups[Random.Range(0, enemyPools.Count)];
    }
    public void SpawnGroup(EnemyGroup group) {
        StartCoroutine(SpawnGroupCor(group));
    }
    public IEnumerator SpawnGroupCor(EnemyGroup group) {
        for (int i = 0; i < group.steps.Count; i++) {
            Spawner door = TerrainManager.Instance.GetRandomSpawner();
            for (int turn = 0; turn < group.steps[i].count; turn++) {
                door.Spawn(group.steps[i].key);
                yield return new WaitForSeconds(group.steps[i].time);
            }
        }
    }

}