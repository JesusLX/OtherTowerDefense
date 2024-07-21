using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {
    protected float initHealth;
    protected float maxHealth;
    protected float currentHealth;
    protected float initShield;
    protected float maxShield;
    protected float currentShield;
    [Range(-1f, 0f)]
    protected float armorPercentage;

    public DamageableStats stats;

    public UnityEvent<IAttacker> onDied;
    public FillBar healthBar;
    public FillBar shieldBar;

    public void Init() {
        if (maxHealth <= 0f) {
            maxHealth = stats.Health;
            maxShield = stats.Shield;
            armorPercentage = stats.ArmorPercertange;
        }
        initHealth = currentHealth = maxHealth;
        initShield = currentShield = maxShield;
        shieldBar.ChangeFill(maxShield, maxShield);
        healthBar.ChangeFill(maxHealth, maxHealth);
    }

    public void getHit(Damage damage, IAttacker attacker) {
        damage = new Damage(damage);
        damage = hitShield(damage);
        damage = hitArmor(damage);
        damage = hitDamage(damage);
        if (currentHealth <= 0) {
            onDied?.Invoke(attacker);
        }
    }
    private Damage hitShield(Damage damage) {
        if (damage.ignoreShield || damage.damage <= currentShield) {
            currentShield -= damage.damage;
            damage.damage = 0;
        } else {
            damage.damage -= currentShield;
            currentShield = 0;
        }
        shieldBar.ChangeFill(maxShield, currentShield);
        return damage;
    }
    private Damage hitArmor(Damage damage) {
        if (damage.ignoreArmor) {
            return damage;
        }
        damage.damage -= damage.damage * armorPercentage;
        return damage;
    }
    private Damage hitDamage(Damage damage) {
        currentHealth = Mathf.Max(currentHealth - damage.damage, 0);
        healthBar.ChangeFill(maxHealth, currentHealth);

        return damage;
    }
}
