using System;
using UnityEngine;

namespace LemonTree.Pools {

    public abstract class PoolItem : MonoBehaviour, IPoolItem {

        public Action<PoolItem> OnKilled;

        /// <summary>
        /// Initialize the PoolItem adding the Action of been Killed
        /// </summary>
        /// <param name="actionKilled">Action with the pool item to be added to the OnKill Action</param>
        public void Init(Action<PoolItem> actionKilled) {
            OnKilled = actionKilled;
        }

        /// <summary>
        /// Calls the pool Manager and Action OnKilled to be Release to the pool
        /// </summary>
        public virtual void Kill() {
            OnKilled?.Invoke(this);
        }
    }

}
