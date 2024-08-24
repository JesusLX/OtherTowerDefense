public interface IEffect {
    void DoEffect();
    void Init(EffectStat effect, IEffectable effectable, IAttacker attacker);
    void RemoveEffect();
    void SetEffectable(IEffectable effectable);
    void StartEffect();
}