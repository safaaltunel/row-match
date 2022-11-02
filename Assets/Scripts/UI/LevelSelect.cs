using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Home()
    {
        PlayerPrefs.SetInt("Levels Panel On", 0);
        this.gameObject.SetActive(false);
    }

    public void OpenLevelsPanel()
    {
        this.gameObject.SetActive(true);
    }
}
