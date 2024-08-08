using UnityEngine;
using LemonTree.Pools;
using System;

namespace LemonTree.ParticlesPool {

    public class PSPoolItem : PoolItem {
        public bool UseOnParticleSystemStopped = true;

        private void Awake() {
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();

            var main = particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
        public void OnParticleSystemStopped() {
            Kill();
        }
    }
}
