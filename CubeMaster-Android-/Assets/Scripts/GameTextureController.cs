using UnityEngine;

public class GameTextureController : MonoBehaviour
{

    public Texture2D CubeTexture;
    public Texture2D CubeNormal;
    public Texture2D PlatformNormal;
    public Texture2D PlatformsTexture;
    public Vector2 tilingPlatform=Vector2.one, tilingCube=Vector2.one;
    public Material[] PlatformsMaterial;
    public Material CubeMaterial;

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

        if (CubeTexture)
        {
            CubeMaterial.mainTexture = CubeTexture;
            CubeMaterial.mainTextureScale = tilingCube;
        }       

        if (CubeNormal)
        {
            CubeMaterial.SetTexture("_BumpMap", CubeNormal);
            CubeMaterial.SetTextureScale("_BumpMap", tilingCube);
        }
        else
            CubeMaterial.SetTexture("_BumpMap", null);
    }
}
