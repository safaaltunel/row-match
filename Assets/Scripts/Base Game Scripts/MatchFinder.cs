using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchFinder : MonoBehaviour
{

    public Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches(int row)
    {
        StartCoroutine(FindAllMatchesCo(row));
    }

    private IEnumerator FindAllMatchesCo(int row)
    {
        yield return new WaitForSeconds(.1f);
        bool rowMatched = true;
        for (int i = 0; i < board.width - 1; ++i)
        {
            GameObject currDot = board.allDots[i, row];
            GameObject nextDot = board.allDots[i + 1, row];
            if (currDot != null && nextDot != null && currDot.tag != nextDot.tag)
            {
                rowMatched = false;
                break;
            }
        }
        if(rowMatched)
        {
            for (int i = 0; i < board.width; ++i)
            {
                if(board.allDots[i, row] != null)
                    board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }
            string color = board.allDots[0, row].GetComponent<Dot>().tag;
            board.IncreaseScore(color);
            board.PlayPopSound();
        }
    }

}
