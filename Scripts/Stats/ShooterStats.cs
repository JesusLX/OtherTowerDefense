using UnityEngine;

[CreateAssetMenu(fileName = "New shoot stat", menuName = "PiscolSystems/Stats/Shoots")]
public class ShooterStats : ScriptableObject{
    public float timeBetweenShots;
    public float timeBetweenRounds;
    public int shotsPerRound;

}