using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITween : MonoBehaviour
{
    [SerializeField] GameObject backPanel, star1, star2, star3, scoreText, colorWheel, highScore;
    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("High Score"))
        {
            score.text = PlayerPrefs.GetInt("High Score").ToString();
        }
        LeanTween.rotateAround(colorWheel, Vector3.forward, -360, 10f).setLoopClamp();
        LeanTween.scale(highScore, new Vector3(1f, 1f, 1f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
        LeanTween.moveLocal(highScore, new Vector3(0f, 550f, 2f), .7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
    }

    void LevelComplete()
    {
        LeanTween.scale(backPanel, new Vector3(1.5f, 1.5f, 1.5f), 1f);
        LeanTween.moveLocal(backPanel, new Vector3(0f, -25f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc).setOnComplete(StarsAnim);
        LeanTween.alpha(scoreText.GetComponent<RectTransform>(), 1f, .5f).setDelay(1f);
    }

    void StarsAnim()
    {
        LeanTween.scale(star1, new Vector3(2f, 2f, 2f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star2, new Vector3(2f, 2f, 2f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(2f, 2f, 2f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(Close);
    }

    void Close()
    {
        LeanTween.moveLocal(highScore, new Vector3(0f, 1000f, 0f), 0.7f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.moveLocal(backPanel, new Vector3(0f, -2000f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc).setOnComplete(GoToMainScene);
    }

    void GoToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    
}
