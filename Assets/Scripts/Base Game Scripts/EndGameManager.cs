using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{

    public int movesLeft;
    public Text counter;
    public Text scoreText;
    public Text highestScoreText;
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        counter.text = movesLeft.ToString();
        board = FindObjectOfType<Board>();
    }

    public void DecreaseCounter()
    {
        StartCoroutine(DecreaseCounterCo());
    }

    private IEnumerator DecreaseCounterCo()
    {
        yield return new WaitForSeconds(.1f);
        movesLeft -= 1;
        counter.text = movesLeft.ToString();

        if (movesLeft == 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        board.currentState = GameState.END;
    }
}
