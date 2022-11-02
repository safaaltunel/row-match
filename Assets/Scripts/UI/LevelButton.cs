using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("Active Stuff")]
    public string levelToLoad;
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    public Image playButtonImage;

    [Header("Level UI")]
    public Text levelText;
    public Text highestScoreText;
    public Text movesText;
    public int level;
    public int highestScore;
    public int moves;
    public Button playButton;

    private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        DecideSprite();
        SetLevelText();
        SetHighestScoreText();
        SetMovesText();
    }

    void OnEnable()
    {
        gameData = FindObjectOfType<GameData>();
        
    }

    void LoadData()
    {
        // Check if GameData is present
        if(gameData != null && gameData.saveData.levels[level - 1] != null)
        {
            // Decide if the level is active and highest score
            isActive = gameData.saveData.levels[level - 1].isActive;
            highestScore = gameData.saveData.levels[level - 1].highScore;
            moves = gameData.saveData.levels[level - 1].moves;
        }
    }

    private void SetMovesText()
    {
        movesText.text = "" + moves + " moves";
    }

    private void SetLevelText()
    {
        levelText.text = "Level " + level;
    }

    private void SetHighestScoreText()
    {
        if(isActive)
        {
            if(highestScore == 0)
            {
                highestScoreText.text = "No Score";
            }
            else
            {
                highestScoreText.text = "Highest Score: " + highestScore;
            }
        }
        else
        {
            highestScoreText.text = "Locked Level";
        }
    }

    void DecideSprite()
    {
        if(isActive)
        {
            playButtonImage.sprite = activeSprite;
            playButton.enabled = true;
            
        }
        else
        {
            playButtonImage.sprite = lockedSprite;
            playButton.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        if(isActive)
        {
            PlayerPrefs.SetInt("Current Level", level);
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
