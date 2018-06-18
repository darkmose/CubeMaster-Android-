using UnityEngine;
using System.Collections.Generic;

[System.Serializable, CreateAssetMenu(menuName = "SkinPack", fileName = "SkinPack Default")]
public class SkinsPack : ScriptableObject
{
    public string packName;
    public string rarity;
    public List<Skin> skins;
}
