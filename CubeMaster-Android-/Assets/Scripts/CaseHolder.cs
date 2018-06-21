using UnityEngine;

public class CaseHolder : MonoBehaviour
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Golden
    }
    public Rarity rarity;
    public Sprite sprite,spriteOp;
}
