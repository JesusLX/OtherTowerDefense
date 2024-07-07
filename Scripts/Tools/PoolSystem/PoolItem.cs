using System;
using System.Collections;
using UnityEngine;

namespace PiscolSystems.Pools {

    public class PoolItem : MonoBehaviour, IPoolItem {

        public Action<IPoolItem> OnKilled;

        public GameObject GameObject => this.gameObject;

        /// <summary>
        /// Initialize the PoolItem adding the Action of been Killed
        /// </summary>
        /// <param name="actionKilled">Action with the pool item to be added to the OnKill Action</param>
        public void Init(Action<IPoolItem> actionKilled) {
            OnKilled = actionKilled;
        }

        public virtual void Instanciate() {
        }

        /// <summary>
        /// Calls the pool Manager and Action OnKilled to be Release to the pool
        /// </summary>
        public virtual void Kill() {
            CancelInvoke("Kill");
            OnKilled?.Invoke(this);
        }
        public virtual void KillAfterTime(float timeToLive) {
            Invoke("Kill", timeToLive); 
        }
        
    }

}
