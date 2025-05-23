using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayGameManager : MonoBehaviour
{
    public int level = 1;

    private Vector2 _startPos;
    private Vector2 _endPos;
    private float TimeRemaining;
    private bool _isGameActive = false;
    private bool _hasWon = false;   

    [SerializeField] private GridManager gridManager;
    [SerializeField] private TextMeshProUGUI TimeRemainText;
    [SerializeField] private float baseTime = 60f;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;

    // Update is called once per frame

    private void Start()
    {
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
            if (gridManager == null)
            {
                Debug.LogError("GridManager not found");
            }
        }
        level = PlayerPrefs.GetInt("SelectedLevelRecent", 1);
        StartLevel(level);
    }
    void Update()
    {
        SwipePieceImage();
        TimerCoundown();
        if (!_hasWon && gridManager.CheckWin())
        {
            _hasWon = true;
            Invoke("WinLevel", 1f);
        }
    }

    public void StartLevel(int levelNumber)
    {
        TimeRemaining = Mathf.Max(10f, baseTime - (levelNumber - 1) * 10f);
        _isGameActive = true;
    }

    private void TimerCoundown()
    {
        if (!_isGameActive) return;

        TimeRemaining -= Time.deltaTime;
        TimeRemainText.text = Mathf.CeilToInt(TimeRemaining).ToString();

        if (TimeRemaining <= 0)
        {
            _isGameActive = false;
            LosePanel.SetActive(true);
            GameOver();
        }
    }

    public void WinLevel()
    {
        _isGameActive = false;
        //SaveRecord(true, level, TimeRemaining);
        WinPanel.SetActive(true);
        level = level + 1;
        PlayerPrefs.SetInt("LevelMax", level);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        //SaveRecord(false, level, 0f);
        Time.timeScale = 0f;
    }

    //void SaveRecord(bool won, int level, float timeLeft)
    //{
    //    string key = "Level" + level + "_Result";
    //    PlayerPrefs.SetString(key, won ? "Win" : "Lose");

    //    if (won)
    //        PlayerPrefs.SetFloat("Level" + level + "_TimeLeft", timeLeft);

    //    PlayerPrefs.Save();
    //}

    private void SwipePieceImage()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
            //_startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition;
            //_endPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            Vector2 directionDelta = _endPos - _startPos;

            if (directionDelta.magnitude > 50f)
            {
                Vector2 direction = directionDelta.normalized;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                    {
                        gridManager.Swipe(Vector2Int.right);
                    }
                    else
                    {
                        gridManager.Swipe(Vector2Int.left);
                    }
                }
                else
                {
                    if (direction.y > 0)
                    {
                        gridManager.Swipe(Vector2Int.up);
                    }
                    else
                    {
                        gridManager.Swipe(Vector2Int.down);
                    }
                }
            }
        }
    }
}
