using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;

[Serializable]
public class Level
{
    public bool isActive;
    public int highScore;
    public int moves;
    public int width;
    public int height;
    public string[] grid;
}

[Serializable]
public class SaveData
{
    public Level[] levels;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;


    // Start is called before the first frame update
    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject); // there will be one and only one gameObject. Singleton pattern!
        }
        Load();
        GetMissingLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetMissingLevels()
    {
        for (int i = 10; i < 25; ++i)
        {
            if (saveData.levels[i].grid.Length == 0)
            {
                if (i < 15)
                {
                    Get("RM_A" + (i + 1), (error) => { Debug.LogError(error); }, ParseText);
                }
                else
                {
                    Get("RM_B" + (i - 14), (error) => { Debug.LogError(error); }, ParseText);
                }
            }
        }
    }

    private void ParseText(string text)
    {
        string[] lines = text.Split('\n');
        int level = int.Parse(lines[0].Split(':')[1].Split(' ')[1]) - 1;
        int width = int.Parse(lines[1].Split(':')[1].Split(' ')[1]);
        int height = int.Parse(lines[2].Split(':')[1].Split(' ')[1]);
        int moves = int.Parse(lines[3].Split(':')[1].Split(' ')[1]);
        string[] grid = lines[4].Split(':')[1].Split(' ')[1].Split(',');

        saveData.levels[level].width = width;
        saveData.levels[level].height = height;
        saveData.levels[level].moves = moves;
        saveData.levels[level].grid = grid;
    }

    private void Get(string file_name, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(GetCoroutine(file_name, onError, onSuccess));
    }



    private IEnumerator GetCoroutine(string file_name, Action<string> onError, Action<string> onSuccess)
    {
        string url = "https://row-match.s3.amazonaws.com/levels/" + file_name;
        using (UnityEngine.Networking.UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                onError(www.error);
            }
            else
            {
                onSuccess(www.downloadHandler.text);
            }
        }
    }

    public void Save()
    {
        string filePath = Application.persistentDataPath + "/player.dat";
        // Create a binary formatter which can read binary files
        BinaryFormatter formatter = new BinaryFormatter();

        // Create a route from the program to the file
        FileStream file = new FileStream(filePath, FileMode.Create);

        // Create a copy of save data
        SaveData data = new SaveData();
        data = saveData;

        //Save the data in the file
        formatter.Serialize(file, data);

        //Close file
        file.Close();

    }

    public void Load()
    {
        string filePath = Application.persistentDataPath + "/player.dat";
        BinaryFormatter formatter = new BinaryFormatter();
        // Check if the save game file exists
        if (File.Exists(filePath))
        {
            // Create a Binary Formatter
            
            FileStream file = File.Open(filePath, FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
        }
        else
        {
            for(int i = 1; i <= 10; ++i)
            {
                var textFile = Resources.Load<TextAsset>("RM_A" + i);
                ParseText(textFile.text);
            }
        }
    }

    private void OnApplicationPause()
    {
        Save();
    }
}
