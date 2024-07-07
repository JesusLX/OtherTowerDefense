using UnityEngine;

[CreateAssetMenu(fileName = "New damage stat", menuName = "PiscolSystems/Stats/Do Damage")]
public class DamageStat : ScriptableObject{
    public float damage;
    public bool ignoreArmor;
    public bool ignoreShield;

}