using LemonTree.ParticlesPool;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Effect : ICloneable {
    public string id;
    public EffectStat initStat;
    protected Stats stat;
    private Countdown countdown;
    protected IEffectable effectable;
    private IAttacker attacker;
    private CancellationTokenSource cancellationTokenSource;

    public void Init(IEffectable effectable, IAttacker attacker) {
        stat = new Stats(initStat.stats);
        this.effectable = effectable;
        this.attacker = attacker;

    }

    internal void SetEffectable(IEffectable effectable) {
        this.effectable = effectable;
    }
    internal void StartEffect() {
        if (countdown != null) {
            Debug.Log("Actualizando Efecto " + id);
            countdown.UpdateRemainingTime(stat.lifetime);
        } else {
            Debug.Log("Empezando Efecto " + id);
            countdown = new Countdown(stat.lifetime);
            countdown.StartCountdown();
            cancellationTokenSource = new CancellationTokenSource();
            // Iniciar la tarea asincrónica para activar el efecto
            DoParticles(stat.start_particles);
            ActivateEffectAsync(cancellationTokenSource.Token);
            //effectable.DoCoroutine(ActivateEffectCor());
        }
    }
    private IEnumerator ActivateEffectCor() {
        Debug.Log("COR: A Contar " + countdown.IsCountingDown());
        while (countdown.IsCountingDown()) {
            Debug.Log("COR: Contando " + countdown.IsCountingDown());
            DoEffect();
            yield return new WaitForSeconds(stat.timeBetweenEffect);
        }
        Debug.Log("COR: Salio de contar" + countdown.IsCountingDown());
    }
    public async Task ActivateEffectAsync(CancellationToken cancellationToken) {
        Debug.Log("A Contar " + countdown.IsCountingDown());
        while (countdown.IsCountingDown() && !cancellationToken.IsCancellationRequested) {
            Debug.Log("Contando " + countdown.IsCountingDown());
            DoEffect();
            await Task.Delay(TimeSpan.FromSeconds(stat.timeBetweenEffect), cancellationToken);
        }
        Debug.Log("Salio de contar" + countdown.IsCountingDown());
        RemoveEffect();
    }

    public void DoEffect() {

        DoParticles(stat.on_effect_particles);
        if (stat.damage.damage != 0) {
            effectable.GetDamageable().getHit(stat.damage, this.attacker);
        }
        Debug.Log("Speedbonus " + stat.speedBonus);
        if (stat.speedBonus != 0) {
            effectable.GetMovementStat().SetBonus(id, stat.speedBonus);
        }
    }

    public void RemoveEffect() {
        DoParticles(stat.end_particles);
        effectable.RemoveEffect(this);
        ClearEffect();
    }
    public void ClearEffect() {
        if (stat.speedBonus != 0) {
            effectable.GetMovementStat().RemoveBonus(id);
        }
        cancellationTokenSource?.Cancel(); // Cancelar la tarea asincrónica si se remueve el efecto
        if (countdown != null) {
            countdown.StopCountdown();
        }
    }
    private void DoParticles(string particleKey) {
        if (particleKey != "") {
            PSManager.Instance.Play(particleKey, effectable.GameObject.transform, Vector3.zero, Quaternion.identity);
        }
    }

    public object Clone() {
        return this.MemberwiseClone();
    }

    [Serializable]
    public class Stats {

        public float lifetime;
        public float timeBetweenEffect;
        public float speedBonus;
        public Damage damage;

        public string start_particles;
        public string on_effect_particles;
        public string end_particles;
        public Stats() {
        }
        public Stats(Stats stats) {
            this.lifetime = stats.lifetime;
            this.timeBetweenEffect = stats.timeBetweenEffect;
            this.speedBonus = stats.speedBonus;
            this.damage = stats.damage;
            this.start_particles = stats.start_particles;
            this.on_effect_particles = stats.on_effect_particles;
            this.end_particles = stats.end_particles;

        }
        public override string ToString() {
            return
            "lifetime = " + this.lifetime +
            ", timeBetweenEffect = " + this.timeBetweenEffect +
            ", speedBonus = " + this.speedBonus +
            ", damage = " + this.damage +
            ", start_particles = " + this.start_particles +
            ", on_effect_particles = " + this.on_effect_particles +
            ", end_particles = " + this.end_particles;
        }
    }
}