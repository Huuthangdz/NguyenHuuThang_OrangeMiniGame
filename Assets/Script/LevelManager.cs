using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject panelLose;
    [SerializeField] private RectTransform imageLoseTransform;
    [SerializeField] private RectTransform[] buttonLoseTransform;
    [SerializeField] private RectTransform starImageLoseTransform;

    [SerializeField] private GameObject PanelWin;
    [SerializeField] private RectTransform imageWinTransform;
    [SerializeField] private RectTransform buttonWinTransform;
    [SerializeField] private RectTransform starYellow;
    [SerializeField] private RectTransform starBlack;

    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject pauseButton;

    public PlayGameManager playGameManager;


    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextLevel = playGameManager.level;
        PlayerPrefs.SetInt("SelectedLevelRecent", nextLevel);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public  void PauseGame()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0f;
    }

    public  void UnPauseGame()
    {
        panelPause.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public async void LoseLevelGame()
    {
        panelLose.SetActive(true);
        pauseButton.SetActive(false);
        await LosePanelLevelIntro();
    }

    async Task LosePanelLevelIntro()
    {
        await imageLoseTransform.DOAnchorPosY(0, 1).SetUpdate(true).AsyncWaitForCompletion();
        await starImageLoseTransform.DOAnchorPosY(2660, 1).SetUpdate(true).AsyncWaitForCompletion();
        for ( int i = 0; i <= buttonLoseTransform.Length; i++)
        {
           await  buttonLoseTransform[0].DOAnchorPosX(-5000, 1).SetUpdate(true).AsyncWaitForCompletion();
           await  buttonLoseTransform[1].DOAnchorPosX(500, 1).SetUpdate(true).AsyncWaitForCompletion();
        }
    }

    public async void WinLevelGame()
    {
        PanelWin.SetActive(true);
        pauseButton.SetActive(false);
        await WinPanelLevelIntro();
    }

    async Task WinPanelLevelIntro()
    {
        await imageWinTransform.DOAnchorPosY(0,1).SetUpdate(true).AsyncWaitForCompletion();
        await starBlack.DOAnchorPosY(7500, 1).SetUpdate(true).AsyncWaitForCompletion();
        await starYellow.DOAnchorPosY(7500, 1).SetUpdate(true).AsyncWaitForCompletion();
        await buttonWinTransform.DOAnchorPosY(-9500, 1).SetUpdate(true).AsyncWaitForCompletion();
    } 
}
