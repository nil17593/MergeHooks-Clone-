using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyGames;

public class AdManager : Singleton<AdManager>
{
    public CrazyAdType adType;
    public void ShowRewarded(CrazyAdType crazyAdType)
    {
        adType = crazyAdType;
        CrazyAds.Instance.beginAdBreakRewarded(ShowRewardedCallback);
    }

    void ShowRewardedCallback()
    {
        switch (adType)
        {
            case CrazyAdType.hookrewarded:
                GameManager.Instance.AddRewardedHook();
                break;

            case CrazyAdType.coinrewarded:
                GameManager.Instance.AddRewardedCoins();
                break;

            case CrazyAdType.extraDamageRewarded:
                GameManager.Instance.GetDoubleDamageReward();
                break;

        }
    }
}
