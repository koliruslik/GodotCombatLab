using System;
using Godot;

namespace CombatLab.Core.Interfaces;

public interface IPlayer : IDamageable
{
    Vector2 GlobalPosition { get; }
    event Action OnInvincibilityEnded;
}