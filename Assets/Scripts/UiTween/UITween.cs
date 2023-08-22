using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UITween : MultiplayerSingleton<UITween>
{
   
    [SerializeField]
    GameObject backPanel, claimButton
      , coins, levelSuccess;
    public GameObject UiCompleteTask1 , UiCompleteTask2, star1, star2, star3;
    public TMP_Text errorText;
    void Start()
    {
        LeanTween.scale(levelSuccess, new Vector3(1.2f, 1.2f, 1.2f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
        LeanTween.moveLocal(levelSuccess, new Vector3(-30f, 429f, 2f), .7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(levelSuccess, new Vector3(1f, 1f, 1f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic);
    }

    void LevelComplete()
    {

        LeanTween.moveLocal(backPanel, new Vector3(0f, -70f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc).setOnComplete(StarsAnim);
        LeanTween.scale(claimButton, new Vector3(1f, 1f, 1f), 2f).setDelay(.8f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.alpha(coins.GetComponent<RectTransform>(), 1f, .5f).setDelay(1f);
    }

    void StarsAnim()
    {
        LeanTween.scale(star1, new Vector3(0.02f, 0.02f, 0.02f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star2, new Vector3(0.02f, 0.02f, 0.02f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(0.02f, 0.02f, 0.02f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);

    }
   
}
