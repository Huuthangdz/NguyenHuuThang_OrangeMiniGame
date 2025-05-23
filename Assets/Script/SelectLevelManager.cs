using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour
{
    [SerializeField] private Button[] buttonSelectLevel;

    private void Update()
    {
        int keySelectedLevel = Mathf.Max(PlayerPrefs.GetInt("LevelMax",1),PlayerPrefs.GetInt("SelectedLevel", 1));
        for (int i = 0; i < buttonSelectLevel.Length; i++)
        {
            if (i < keySelectedLevel)
            {
                buttonSelectLevel[i].interactable = true;
            }
            else
            {
                buttonSelectLevel[i].interactable = false;
            }
        }
    }

    public void OnClickLevelButton(int level)
    {
        PlayerPrefs.SetInt("SelectedLevelRecent", level);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}
