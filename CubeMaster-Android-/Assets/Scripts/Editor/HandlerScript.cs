using UnityEngine;
using UnityEditor;

public class HandlerScript
{
    [MenuItem("AssetBundle/CreateAll")]
    static void CreateAssetBundle()
    {
        if (!System.IO.Directory.Exists(Application.streamingAssetsPath + "/Assets/"))
        {
            System.IO.Directory.CreateDirectory(Application.streamingAssetsPath + "/Assets/");
        }
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Assets/", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
    [MenuItem("AssetBundle/CreateLevelWindows")]
    static void CreateAssetBundle1()
    {
        if (!System.IO.Directory.Exists(Application.streamingAssetsPath + "/AssetsW/"))
        {
            System.IO.Directory.CreateDirectory(Application.streamingAssetsPath + "/AssetsW/");
        }
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsW/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }


}
