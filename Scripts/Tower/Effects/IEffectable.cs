using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable {

    Dictionary<string, Effect> Effects { get; set; }
    GameObject GameObject { get; }

    MovementStats GetMovementStat();
    Damageable GetDamageable();
    void RemoveEffect(Effect effect);
    void SetEffect(Effect effect);
    void DoCoroutine(IEnumerator cor);
}