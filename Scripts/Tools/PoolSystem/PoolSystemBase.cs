using UnityEngine;
using UnityEngine.Pool;
using PiscolSystems.Pools;
using System;
using System.Collections;
using Unity.VisualScripting;

namespace PiscolSystems.Pools {

    /// <summary>
    /// Base class for object pool systems. Implements IPool interface.
    /// </summary>
    [CreateAssetMenu(fileName = "New basic pool", menuName = "PiscolSystems/PoolSystem/BasicPool")]
    public class PoolSystemBase : ScriptableObject, IPool {
        public string Id;
        public Transform restParent;
        private Vector3 initPos;
        private Quaternion initRot;

        [Header("Object Pool")]
        [Tooltip("Default capacity of the pool")]
        public int defaultCapacity = 100;
        [Tooltip("Max size of the pool")]
        public int maxCapacity = 1000;
        [Tooltip("GameObject to spawn")]
        [SerializeField]
        public PoolItem prefab;
        public bool resetRigidbody = false;
        internal IObjectPool<IPoolItem> psPool;
        internal bool collectionChecks = true;

        /// <summary>
        /// Constructor for PoolSystemBase. Initializes the object pool.
        /// </summary>
        public PoolSystemBase() {
            psPool = new ObjectPool<IPoolItem>(
                   CreatePooledItem,
                   OnTakeFromPool,
                   OnReturnedToPool,
                   OnDestroyPoolObject,
                   collectionChecks,
                   defaultCapacity,
                   maxCapacity
                   );
            if (prefab != null) {
                initPos = Vector3.zero;
                initRot = Quaternion.identity;
            }
        }

        /// <summary>
        /// Initializes a pooled item.
        /// </summary>
        /// <param name="poolItem">The pooled item to initialize.</param>
        /// <returns>The initialized pooled item.</returns>
        public virtual IPoolItem Init(IPoolItem poolItem) {
            this.Reset(poolItem);
            poolItem.GameObject.transform.parent = null;
            poolItem.Init(Kill);
            return poolItem;
        }

        /// <summary>
        /// Resets the specified pooled item's transform and position.
        /// </summary>
        /// <param name="poolItem">The pooled item to reset.</param>
        /// <returns>The reset pooled item.</returns>
        public virtual IPoolItem Reset(IPoolItem poolItem) {
            poolItem.GameObject.transform.SetParent(restParent, true);
            poolItem.GameObject.transform.position = initPos;
            poolItem.GameObject.transform.rotation = initRot;
            return poolItem;
        }
        #region Plays

        public virtual IPoolItem Play(Transform parent, Vector3 position, Quaternion rotation) {
            IPoolItem poolItem = this.Play(position, rotation);
            ResetRigidBody(poolItem);
            poolItem = this.SetParent(poolItem, parent);
            return poolItem;
        }

        public virtual IPoolItem Play(Vector3 position, Quaternion rotation) {
            IPoolItem poolItem = psPool.Get();
            poolItem = Init(poolItem);
            poolItem = this.SetRotation(poolItem, rotation);
            poolItem = this.SetPosition(poolItem, position);
            poolItem.Instanciate();
            return poolItem;
        }
        public virtual IPoolItem Play(Transform parent) {
            IPoolItem poolItem = psPool.Get();
            ResetRigidBody(poolItem);
            poolItem = Init(poolItem);
            poolItem = SetParent(poolItem, parent);
            poolItem.Instanciate();
            return poolItem;
        }
        private void ResetRigidBody(IPoolItem poolItem) {
            if (resetRigidbody) {
                Rigidbody rb;
                if (poolItem.GameObject.TryGetComponent<Rigidbody>(out rb)) {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.angularDamping = 0;
                    Debug.Log(rb.linearVelocity.sqrMagnitude + " " + rb.angularVelocity);
                }
                Rigidbody2D rb2d;
                if (poolItem.GameObject.TryGetComponent<Rigidbody2D>(out rb2d)) {
                    rb2d.linearVelocity = Vector3.zero;
                    rb2d.angularDamping = 0;
                    rb2d.angularVelocity = 0;
                }

            }
        }
        public virtual IPoolItem Play(Transform parent, Vector3 position, Quaternion rotation, float timeToLive) {
            IPoolItem poolItem = this.Play(parent, position, rotation);

            poolItem.KillAfterTime(timeToLive);

            return poolItem;
        }


        #endregion
        #region Pool Funcions

        public virtual void Kill(IPoolItem poolItem) => psPool.Release(poolItem);

        public virtual IPoolItem CreatePooledItem() {
            return Instantiate(prefab.GameObject, null, true).GetComponent<IPoolItem>();
        }

        public virtual void OnReturnedToPool(IPoolItem obj) {
            obj.GameObject.SetActive(false);
        }

        public virtual void OnTakeFromPool(IPoolItem obj) {
            if (obj != null)
                obj.GameObject.SetActive(true);
        }

        public virtual void OnDestroyPoolObject(IPoolItem obj) {
            Destroy(obj.GameObject);
        }
        #endregion

        #region SETTERS
        public virtual IPoolItem SetPosition(IPoolItem poolItem, Vector3 position) {
            poolItem.GameObject.transform.position = position;
            if (poolItem.GameObject.transform.parent != null) {
                poolItem.GameObject.transform.SetLocalPositionAndRotation(position, poolItem.GameObject.transform.localRotation);
            }
            return poolItem;
        }
        public virtual IPoolItem SetRotation(IPoolItem poolItem, Quaternion rotation) {
            poolItem.GameObject.transform.rotation = rotation;
            return poolItem;
        }
        public virtual IPoolItem SetParent(IPoolItem poolItem, Transform parent) {
            poolItem.GameObject.transform.SetParent(parent);
            return poolItem;
        }
        #endregion
    }
}
