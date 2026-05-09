using CombatLab.Core.Interfaces;

namespace CombatLab.Core.Payloads;

public struct DeathData
{
    public IDamageable Victim;
    public IAttacker Killer;
    public string DamageSourceId;
    public float Timestamp;
    public int GoldReward;
    
}