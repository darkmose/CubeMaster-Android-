using UnityEngine;

public class GameTextureController : MonoBehaviour
{
    public Texture2D PlatformNormal;
    public Texture2D PlatformsTexture;
    public Vector2 tilingPlatform = Vector2.one;
    public Material[] PlatformsMaterial;

    [ContextMenu("Texture")]
    private void Start()
    {
        foreach (Material m in PlatformsMaterial)
        {
            if (PlatformsTexture)
            {
                m.mainTexture = PlatformsTexture;
                m.mainTextureScale = tilingPlatform;
            }         

            if (PlatformNormal)
            {
                m.SetTexture("_BumpMap", PlatformNormal);
                m.SetTextureScale("_BumpMap", tilingPlatform);
            }
            else
                m.SetTexture("_BumpMap", null);            
        }
    }
}
