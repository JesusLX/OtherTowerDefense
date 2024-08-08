using UnityEngine;
using UnityEngine.Pool;
using LemonTree.Pools;

namespace LemonTree.ParticlesPool {

    public abstract class PoolSystemBase : ScriptableObject, IPool {
        public string Id;
        public Transform restParent;
        private Vector3 initPos;
        private Quaternion initRot;

        [Header("Object Pool")]
        public int defaultCapacity = 100;
        public int maxCapacity = 1000;
        public GameObject prefab;
        internal IObjectPool<PoolItem> psPool;
        internal bool collectionChecks = false;


        public PoolSystemBase() {
            psPool = new ObjectPool<PoolItem>(
                   CreatePooledItem,
                   OnTakeFromPool,
                   OnReturnedToPool,
                   OnDestroyPoolObject,
                   collectionChecks,
                   defaultCapacity,
                   maxCapacity
                   );
            if (prefab != null) {

                initPos = prefab.transform.position;
                initRot = prefab.transform.rotation;
            }
        }
        public virtual PoolItem Init(PoolItem poolItem) {
            this.Reset(poolItem);
            poolItem.gameObject.transform.parent = null;
            poolItem.Init(Kill);
            return poolItem;
        }

        /// <summary>
        /// Resets poolItem's transform parent with restParent
        /// Resets poolItem's position to the prefab position 
        /// Resets poolItem's rotation to the prefab rotation 
        /// </summary>
        /// <param name="poolItem">PoolItem to reset</param>
        /// <returns></returns>
        public virtual PoolItem Reset(PoolItem poolItem) {

            poolItem.gameObject.transform.SetParent(restParent,true);
            poolItem.gameObject.transform.position = initPos;
            poolItem.gameObject.transform.rotation = initRot;
            return poolItem;
        }
        #region Plays

        public virtual PoolItem Play(Transform parent, Vector3 position, Quaternion rotation) {
            PoolItem ps = psPool.Get();
            ps = Init(ps);
            ps = this.SetParent(ps, parent);
            ps = this.SetPosition(ps, position);
            ps = this.SetRotation(ps, rotation);
            return ps;
        }

        public virtual PoolItem Play(Vector3 position, Quaternion rotation) {
            PoolItem ps = psPool.Get();
            ps = Init(ps);
            ps = this.SetRotation(ps, rotation);
            ps = this.SetPosition(ps, position);
            return ps;
        }
        public virtual PoolItem Play(Transform parent) {
            PoolItem ps = psPool.Get();
            ps = Init(ps);
            ps = SetParent(ps, parent);
            return ps;
        }
        #endregion
        #region Pool Funcions

        public virtual void Kill(PoolItem poolItem) => psPool.Release(poolItem);

        public virtual PoolItem CreatePooledItem() {
            return Instantiate(prefab,null,true).GetComponent<PoolItem>();
        }

        public virtual void OnReturnedToPool(PoolItem obj) {
            obj.gameObject.SetActive(false);
        }

        public virtual void OnTakeFromPool(PoolItem obj) {
            if(obj != null)
            obj.gameObject.SetActive(true);
        }

        public virtual void OnDestroyPoolObject(PoolItem obj) {
            Destroy(obj.gameObject);
        }
        #endregion

        #region SETTERS
        public virtual PoolItem SetPosition(PoolItem poolItem, Vector3 position) {
            poolItem.transform.position = position;
            if(poolItem.transform.parent != null) {
                poolItem.transform.SetLocalPositionAndRotation(position,poolItem.transform.localRotation);
            }
            return poolItem;
        }
        public virtual PoolItem SetRotation(PoolItem poolItem, Quaternion rotation) {
            poolItem.transform.rotation = rotation;
            return poolItem;
        }
        public virtual PoolItem SetParent(PoolItem poolItem, Transform parent) {
            poolItem.transform.SetParent(parent);
            return poolItem;
        }
        #endregion
    }
}
