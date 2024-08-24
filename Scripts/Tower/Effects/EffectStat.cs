using LemonTree.ParticlesPool;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effects/EffectXStat")]
public  class EffectStat : ScriptableObject {
    public Effect.Stats stats;

}