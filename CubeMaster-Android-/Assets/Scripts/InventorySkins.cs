using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Inventory", fileName = "Default name"),System.Serializable]
public class InventorySkins : ScriptableObject
{
    public List<Skin> skins;
}
