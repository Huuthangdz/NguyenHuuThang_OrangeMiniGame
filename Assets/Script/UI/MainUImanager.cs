using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class MainUImanager : MonoBehaviour
{
    [SerializeField] private GameObject panelHowToPlay;
    [SerializeField] private GameObject panelSelectLevel;
    
    
    [SerializeField] private RectTransform panelHowToPlayTransform;
    [SerializeField] private RectTransform buttonBackOfHowToPlayTransform;
    [SerializeField] private RectTransform buttonBackSelectLevelTransform;

    [SerializeField] private RectTransform panelSelectLevelTransform;
    [SerializeField] private RectTransform gameObjectSelectLevelTransform;

    [SerializeField] private RectTransform buttonPlayTransform;


    [SerializeField] private float tweenDuration;

    public async void OnClickHowToPlay()
    {
        panelHowToPlay.SetActive(true);
        HowtoPlayPanelIntro();
        await ButtonPlayOutro();
    }

    public async void OnClickToMainMenu()
    {
        await HowtoPlayPanelOutro();
        ButtonPlayIntro();
        panelHowToPlay.SetActive(false);
    }

    public async void OnClickPlay()
    {
        panelHowToPlay.SetActive(false);
        panelSelectLevel.SetActive(true);
        await SelectLevelPanelIntro();
    }
    public async void OnClickBackToMainMenu()
    {
        await SelectLevelPanelOutro();
        ButtonPlayIntro();
        panelSelectLevel.SetActive(false);
        panelHowToPlay.SetActive(false);
    }

    private void HowtoPlayPanelIntro()
    {
        panelHowToPlayTransform.DOAnchorPosY(0, tweenDuration);
        buttonBackOfHowToPlayTransform.DOAnchorPosX(-580, tweenDuration);

    }
    async Task HowtoPlayPanelOutro()
    {
       await  panelHowToPlayTransform.DOAnchorPosY(700, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
       await  buttonBackOfHowToPlayTransform.DOAnchorPosX(-760, tweenDuration).SetUpdate(true).AsyncWaitForCompletion(); 
    }

    private void ButtonPlayIntro()
    {
        buttonPlayTransform.DOAnchorPosY(-250, tweenDuration);
    }
    async Task ButtonPlayOutro()
    {
        await buttonPlayTransform.DOAnchorPosY(-480, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    async Task SelectLevelPanelIntro()
    {
        await panelSelectLevelTransform.DOAnchorPosY(0, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        await gameObjectSelectLevelTransform.DOAnchorPosY(0, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    async Task SelectLevelPanelOutro()
    {
        await gameObjectSelectLevelTransform.DOAnchorPosY(780, tweenDuration).SetUpdate(true).AsyncWaitForElapsedLoops(1);
        await panelSelectLevelTransform.DOAnchorPosY(780, tweenDuration).SetUpdate(true).AsyncWaitForElapsedLoops(1);
    }
}
