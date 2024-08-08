using UnityEngine;
using LemonTree.Pools;

namespace LemonTree.ParticlesPool {

    [CreateAssetMenu(fileName = "New Particle pool", menuName = "TreeLemon/ParticleSystemPool")]
    public class ParticleSystemPool : PoolSystemBase {
        public override PoolItem Play(Transform parent, Vector3 position, Quaternion rotation) {
            PoolItem ps = base.Play(parent, position, rotation);
            ps = this.PlayParticles(ps);
            return ps;
        }
        public override PoolItem Play(Vector3 position, Quaternion rotation) {
            PoolItem ps = base.Play(position, rotation);
            ps = this.PlayParticles(ps);
            return ps;
        }
        public override PoolItem Play(Transform parent) {
            PoolItem ps = base.Play(parent);
            ps = this.PlayParticles(ps);
            return ps;
        }

        public PoolItem PlayParticles(PoolItem poolItem) {
            poolItem.GetComponent<ParticleSystem>().Play();
            return poolItem;
        }
    }
}
