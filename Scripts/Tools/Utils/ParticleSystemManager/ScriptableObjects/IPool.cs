using UnityEngine;

namespace LemonTree.Pools {
    internal interface IPool {
        PoolItem Init(PoolItem poolItem);
        PoolItem Play(Transform parent, Vector3 position, Quaternion rotation);
        PoolItem Play(Vector3 position, Quaternion rotation);
        PoolItem Play(Transform parent);
        void Kill(PoolItem poolItem);
        PoolItem Reset(PoolItem poolItem);
        PoolItem CreatePooledItem();
        void OnDestroyPoolObject(PoolItem obj);
        void OnReturnedToPool(PoolItem obj);
        void OnTakeFromPool(PoolItem obj);
    }
}