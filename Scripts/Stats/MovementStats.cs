using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New movement stat", menuName = "PiscolSystems/Stats/Movement")]
public class MovementStats : ScriptableObject {
    public Dictionary<string, float> speedBonus;
    public float currentSpeed;
    public float speed;

    public void Init(MovementStats movementStats) {
        this.currentSpeed = movementStats.currentSpeed;
        this.speed = movementStats.speed;
        speedBonus = new();
    }

    public float GetCurrentSpeed() {
        float affectedSpeed = speed;
        if (speedBonus != null) {

            foreach (var effect in speedBonus) {
                affectedSpeed *= effect.Value;
                Debug.Log("Speed bonus "+effect.Key+": "+effect.Value+" = "+affectedSpeed,this);
            }
        }
        return affectedSpeed;
    }
    public void SetBonus(string effectName, float speedBonus) {
        Debug.Log("Añadiendo bonus  "+this.speedBonus);
        if(this.speedBonus.ContainsKey(effectName)) {
            this.speedBonus[effectName] = speedBonus;
        } else {
            this.speedBonus.Add(effectName, speedBonus);
        }
    }
    public void RemoveBonus(string effectName) {
        this.speedBonus.Remove(effectName);
        Debug.Log("Borrado " + effectName);
    }
}