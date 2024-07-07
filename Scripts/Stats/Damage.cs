using UnityEngine.PlayerLoop;

public class Damage {
    public float damage;
    public bool ignoreArmor;
    public bool ignoreShield;
    public Damage(DamageStat stat) {
        damage = stat.damage;
        ignoreArmor = stat.ignoreArmor;
        ignoreShield = stat.ignoreShield;
    }
    public Damage(Damage stat) {
        damage = stat.damage;
        ignoreArmor = stat.ignoreArmor;
        ignoreShield = stat.ignoreShield;
    }

    public override string ToString() {
        return "Damage: "+damage+", IgnoreArmor: "+ignoreArmor+", IgnoreShield: "+ignoreShield;
    }
}