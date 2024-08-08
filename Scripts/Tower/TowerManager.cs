using PiscolSystems.Pools;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    public List< PoolSystemBase> towerPools;

    public GameObject GetTower(string id, Vector3 position) {
        var pool = towerPools.Find(p => p.Id == id);
        Debug.Log(pool);
        GameObject tower = null;
        if (pool != null) {
            tower = pool.Play(this.transform, position, Quaternion.identity).GameObject;
            tower.GetComponent<TowerController>().Init();
        }
        return tower;
    }
    public GameObject GetTower(string id) {
        var pool = towerPools.Find(p => p.Id == id);
        Debug.Log(pool);
        GameObject tower = null;
        if (pool != null) {
            tower = pool.Play(this.transform, transform.position, Quaternion.identity).GameObject;
            tower.GetComponent<TowerController>().Init();
        }
        return tower;
    }
}
