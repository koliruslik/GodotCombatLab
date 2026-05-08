using CombatLab.entities;

namespace CombatLab.Core.Payloads;

public struct DeathData
{
    public Entity Victim;
    public Entity Killer;

    public string DamageSourceId;
    //public DamageType Type // not implemented
    public float Timestamp;
}