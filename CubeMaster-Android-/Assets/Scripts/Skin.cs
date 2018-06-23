using UnityEngine;

[System.Serializable]
public class Skin : ScriptableObject
{
    public new string name;
    public enum Rarity
    {
        Common,
        Uncommon,
        Golden
    }
    public Rarity rarity;
    public Sprite sprite;
    public Texture2D normalSprite;

}
