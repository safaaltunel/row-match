using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    WAIT,
    MOVE,
    END,
    PAUSE,
}

[Serializable]
public class ColorDotPair
{
    public string color;
    public GameObject dot;
}

public class Board : MonoBehaviour
{
    [Header("Scriptable Object Stuff")]
    public int level;

    public GameState currentState = GameState.MOVE;

    [Header("Board Dimensions")]
    public int width;
    public int height;
    public int offset;


    public int movesLeft;
    public GameObject tilePrefab;

    [Header("Dots Stuff")]
    public List<ColorDotPair> dotColorsList = new List<ColorDotPair>();
    public Dictionary<string, GameObject> dots = new Dictionary<string, GameObject>(); // 4 farklı candy'yi temsil ediyor
    public GameObject[,] allDots; // griddeki candyleri temsil ediyor
    public string[] grid;


    private BackgroundTile[,] allTiles;

    [Header("Helper Managers")]
    private MatchFinder matchFinder;
    private DeadLockFinder deadLockFinder;
    private ScoreManager scoreManager;
    private SoundManager soundManager;
    public EndGameManager endGameManager;

    [Header("Color Scores")]
    private Dictionary<string, int> colorScores = new Dictionary<string, int>()
    {
        { "Red", 100 },
        { "Green", 150 },
        { "Blue", 200 },
        { "Yellow", 250 }
    };

    [Header("Persistent Game Data")]
    private GameData gameData;

    void Awake()
    {
        foreach (ColorDotPair pair in dotColorsList)
        {
            dots[pair.color] = pair.dot;
        }

        if(PlayerPrefs.HasKey("Current Level"))
        {
            level = PlayerPrefs.GetInt("Current Level");
        }
        gameData = FindObjectOfType<GameData>();

        if (gameData != null && gameData.saveData.levels[level - 1] != null)
        {
            width = gameData.saveData.levels[level - 1].width;
            height = gameData.saveData.levels[level - 1].height;
            movesLeft = gameData.saveData.levels[level - 1].moves;
            grid = gameData.saveData.levels[level - 1].grid;
        }
    }

    void Update()
    {
        if(currentState == GameState.END)
        {
            StartCoroutine(EndGameCo());
            currentState = GameState.WAIT;
        }
    }

    private IEnumerator EndGameCo()
    {
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetInt("Levels Panel On", 1);
        if (gameData.saveData.levels[level - 1].highScore < scoreManager.score)
        {
            gameData.saveData.levels[level - 1].highScore = scoreManager.score;
            if(level < gameData.saveData.levels.Length)
                gameData.saveData.levels[level].isActive = true;
            gameData.Save();
            PlayerPrefs.SetInt("High Score", scoreManager.score);
            SceneManager.LoadScene("HighScoreScene");
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        matchFinder = FindObjectOfType<MatchFinder>();
        deadLockFinder = FindObjectOfType<DeadLockFinder>();
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.highestScore = gameData.saveData.levels[level - 1].highScore;

        soundManager = FindObjectOfType<SoundManager>();
        endGameManager = FindObjectOfType<EndGameManager>();
        endGameManager.movesLeft = movesLeft;
        SetUp();
    }

    private void SetUp()
    {
        for (int row = 0; row < height; ++row)
        {
            for (int column = 0; column < width; ++column)
            {
                Vector2 tempPosition = new Vector2(column, row + offset);
                Vector2 tilePosition = new Vector2(column, row);
                GameObject backgroundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = $"({column}, {row}) tile";
                string dotToUse = grid[row * width + column];
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().row = row;
                dot.GetComponent<Dot>().column = column;
                dot.transform.parent = this.transform;
                dot.name = $"({column}, {row})";
                allDots[column, row] = dot;
            }
        }
    }

    public void FindMatch(int row)
    {
        matchFinder.FindAllMatches(row);
    }

    public void FindDeadLock()
    {
        deadLockFinder.IsDeadLocked();
    }

    public void IncreaseScore(string color)
    {
        scoreManager.IncreaseScore(colorScores[color]);
    }

    public void PlayPopSound()
    {
        soundManager.PlayPopSound();
    }

    public void EndGame()
    {
        endGameManager.EndGame();
    }
}
