using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Case
{
    public Color colorRarity;

    public enum Rarity
    {
        Common,
        Uncommon,
        Golden
    }

    public Rarity rarity;
    public Sprite caseSprite, caseSpriteOpen;

}

