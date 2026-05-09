using Godot;

namespace CombatLab.Core.Data.Items;

public enum Rarity
{
    Cursed,
    Common ,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public static class RarityExtensions
{
    public static Color ToColor(this Rarity rarity) => rarity switch
    {
        Rarity.Cursed => Colors.DarkRed,
        Rarity.Common => Colors.LightGray,
        Rarity.Uncommon => Colors.MediumSeaGreen,
        Rarity.Rare => Colors.RoyalBlue,
        Rarity.Epic => Colors.DarkMagenta,
        Rarity.Legendary => Colors.Gold,
        _ => Colors.White
    };
}