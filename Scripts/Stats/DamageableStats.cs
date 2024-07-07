using UnityEngine;


[CreateAssetMenu(fileName = "New damage stat", menuName = "PiscolSystems/Stats/Damage")]
public class DamageableStats : ScriptableObject {
    public float Health;
    public float Shield;
    public float ArmorPercertange;
}