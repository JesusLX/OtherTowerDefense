using UnityEngine;

namespace PiscolSystems.Pools {
    internal interface IPool {
        IPoolItem Init(IPoolItem poolItem);
        IPoolItem Play(Transform parent, Vector3 position, Quaternion rotation);
        IPoolItem Play(Vector3 position, Quaternion rotation);
        IPoolItem Play(Transform parent);
        void Kill(IPoolItem poolItem);
        IPoolItem Reset(IPoolItem poolItem);
        IPoolItem CreatePooledItem();
        void OnDestroyPoolObject(IPoolItem obj);
        void OnReturnedToPool(IPoolItem obj);
        void OnTakeFromPool(IPoolItem obj);
    }
}