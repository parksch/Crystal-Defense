using DG.Tweening;
using System;
using UnityEngine;

public class UILastCard : UICard
{
    bool isFirst = true;

    TweenCallback callback;

    public void SetCallBack(TweenCallback tweenCallback)
    {
        callback = tweenCallback;
    }

    public override void OpenCard()
    {
        base.OpenCard();

        if (isFirst)
        {
            cardSequqence.OnComplete(callback);
            isFirst = false;
        }
    }
}
