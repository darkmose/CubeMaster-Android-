using UnityEngine.Advertisements;

public class AdverManager
{ 
    public bool IsSupported
    {
        get
        {
            return Advertisement.isSupported;
        }
    }
    public System.Action onEndAd;

    public AdverManager()
    {
        if (IsSupported)
        {
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
            {
                Advertisement.Initialize("2754385");
            }
            else if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
            {
                Advertisement.Initialize("2754383");
            }
            else
            {
                Advertisement.Initialize("2754385"); //TESTTESTTEST TEST!!!!!!!!!!!!!!
            }

        }
    }

    public AdverManager(bool isTestMod)
    {
        if (IsSupported)
        {
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
            {
                Advertisement.Initialize("2754385", isTestMod);
            }
            else if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
            {
                Advertisement.Initialize("2754383", isTestMod);
            }
            else
            {
                Advertisement.Initialize("2754385", isTestMod); //TESTTESTTEST TEST!!!!!!!!!!!!!!
            }
        }        
    }

    public void GetAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;
            Advertisement.Show("rewardedVideo", options);
        }
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            SaveManager.coins += 10;
            onEndAd();
        }
    }

}
