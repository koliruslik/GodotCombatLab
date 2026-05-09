using Godot;

namespace CombatLab.Core.Data.Items;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string Name;
    [Export] public string Description;
    [Export] public Rarity Rarity;
    [Export] public int Level;
    [Export] public float Price;
    [Export] public float Weight;
    [Export] public Texture2D Icon;
}

