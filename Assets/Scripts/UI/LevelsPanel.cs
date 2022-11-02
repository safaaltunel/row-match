using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsPanel : MonoBehaviour
{

    public GameObject levelPrefab;
    private GameData gameData;
    private int levelsCount;

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        levelsCount = 0;
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelsCount < 25)
        {
            while (levelsCount < 25 && gameData.saveData.levels[levelsCount].grid.Length != 0)
            {
                GameObject level = Instantiate(levelPrefab, this.transform);
                level.GetComponent<LevelButton>().level = levelsCount + 1;
                levelsCount += 1;
            }
        }
    }

    private void SetUp()
    {
        for(int i = 0; i < 25; ++i)
        {
            if (gameData.saveData.levels[i].grid.Length != 0)
            {
                GameObject level = Instantiate(levelPrefab, this.transform);
                level.GetComponent<LevelButton>().level = i + 1;
                levelsCount += 1;
            }
            else break;
        }
    }
}
