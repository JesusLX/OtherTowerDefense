using System;
using UnityEngine;

namespace PiscolSystems.Pools {

    public interface IPoolItem {

        GameObject GameObject { get; }

        void Init(Action<IPoolItem> actionKilled);
        public void Instanciate();
        public void Kill();
        void KillAfterTime(float timeToLive);
    }
}