using CombatLab.Core.Interfaces;

namespace CombatLab.Core.Payloads;

public struct DeathData
{
    public ICombatant Victim;
    public ICombatant Killer;
    public string DamageSourceId;
    public float Timestamp;
    public int GoldReward;
    
}