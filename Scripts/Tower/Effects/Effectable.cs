using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Effectable : MonoBehaviour, IEffectable {
    public List<Effect> testeffects;
    private Dictionary<string, Effect> effects;
    public Dictionary<string, Effect> Effects { get => effects; set => effects = value; }
    public GameObject GameObject { get => gameObject; }

    public void DoCoroutine(IEnumerator cor) {
        StartCoroutine(cor);
    }

    public Damageable GetDamageable() {
        return gameObject.GetComponent<Damageable>();
    }

    public MovementStats GetMovementStat() {
        return gameObject.GetComponent<Enemy>().GetMovStats();

    }

    public void RemoveEffect(Effect effect) {
        Effects ??= new();

        if (Effects.ContainsKey(effect.id)) {
            Effects.Remove(effect.id);
        }
    }

    public void SetEffect(Effect effect) {
        Effects ??= new();
        if (!Effects.ContainsKey(effect.id)) {
            Effects.Add(effect.id, effect);
            testeffects.Add(effect);
        } else {
            effect = Effects[effect.id];
        }
        effect.StartEffect();
        //if(!Effects.Contains(effect)) {
        //    effect.SetEffectable(this);
        //    effect.StartEffect();
        //}
        //Debug.Log(effect);


    }

    public void RemoveEffects() {
        if (Effects != null) {
            foreach (Effect effect in Effects.Values) {
                effect.ClearEffect();
            }
            Effects.Clear();
        }
    }
}
