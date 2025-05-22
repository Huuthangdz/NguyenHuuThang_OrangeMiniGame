using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
        playGameManager.StartLevel(playGameManager.level + 1);
    }
}
