using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsPanelManager : MonoBehaviour
{
    public GameObject levelsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelsPanel.activeSelf && PlayerPrefs.HasKey("Levels Panel On") && PlayerPrefs.GetInt("Levels Panel On") == 1)
        {
            levelsPanel.SetActive(true);
        }
    }

    private void OnApplicationPause()
    {
        PlayerPrefs.SetInt("Levels Panel On", 0);
    }
}
