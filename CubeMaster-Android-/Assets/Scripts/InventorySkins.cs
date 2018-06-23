using System.Collections.Generic;

[System.Serializable]
public class InventorySkins
{
    public InventorySkins()
    {
        skins = new List<Skin>();
    }

    public List<Skin> skins;
}
