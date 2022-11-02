using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadLockFinder : MonoBehaviour
{

    public Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void IsDeadLocked()
    {
        StartCoroutine(FindDeadLockCo());
    }

    private bool CanMatch(string color, int currentRow, int currentColumn, int startRow, int endRow, in HashSet<Vector2> remainingCoordinates, ref HashSet<Vector2> usedCoordinates, int movesLeft, ref GameObject[,] dots)
    {
        if (movesLeft < 0) return false;
        if (currentColumn == board.width) return true;

        if (dots[currentColumn, currentRow].tag == color) return CanMatch(color, currentRow, currentColumn + 1, startRow, endRow, remainingCoordinates, ref usedCoordinates, movesLeft, ref dots);

        foreach (Vector2 coordinate in remainingCoordinates)
        {
            if (coordinate.y != currentRow && !usedCoordinates.Contains(coordinate))
            {
                // Make change
                usedCoordinates.Add(coordinate);
                int movesToUse = (int)Mathf.Abs(currentColumn - coordinate.x) + (int)Mathf.Abs(currentRow - coordinate.y);
                GameObject oldDot = dots[currentColumn, currentRow];
                dots[currentColumn, currentRow] = dots[(int)coordinate.x, (int)coordinate.y];
                dots[(int)coordinate.x, (int)coordinate.y] = oldDot;

                // try to match other columns
                if (CanMatch(color, currentRow, currentColumn + 1, startRow, endRow, remainingCoordinates, ref usedCoordinates, movesLeft - movesToUse, ref dots))
                {
                    string coords = "";
                    foreach (var co in usedCoordinates) coords += "(" + co.x + ", " + co.y + ") ";
                    return true;
                }

                // Reverse change
                usedCoordinates.Remove(coordinate);
                dots[(int)coordinate.x, (int)coordinate.y] = dots[currentColumn, currentRow];
                dots[currentColumn, currentRow] = oldDot;

            }
        }

        return false;
    }

    private IEnumerator FindDeadLockCo()
    {
        yield return new WaitForSeconds(.2f);
        int startRow = 0;
        while (startRow < board.height)
        {
            while (startRow < board.height && board.allDots[0, startRow].GetComponent<Dot>().isMatched)
            {
                startRow += 1;
            }
            if (startRow == board.height)
            {
                board.EndGame();
                yield break;
            }

            int endRow = startRow;
            while (endRow < board.height && !board.allDots[0, endRow].GetComponent<Dot>().isMatched)
            {
                endRow += 1;
            }
            endRow -= 1; // endRow is inclusive
            Dictionary<string, HashSet<Vector2>> colorCoordinates = new Dictionary<string, HashSet<Vector2>>();
            for (int row = startRow; row <= endRow; ++row)
            {
                for (int col = 0; col < board.width; ++col)
                {
                    string color = board.allDots[col, row].GetComponent<Dot>().tag;
                    if (!colorCoordinates.ContainsKey(color))
                        colorCoordinates[color] = new HashSet<Vector2>();
                        
                    colorCoordinates[color].Add(new Vector2(col, row));
                }
            }
            foreach(KeyValuePair<string, HashSet<Vector2>> entry in colorCoordinates)
            {
                if (entry.Value.Count >= board.width) 
                {
                    for(int currentRow = startRow; currentRow <= endRow; ++currentRow)
                    {
                        GameObject[,] dots = board.allDots.Clone() as GameObject[,];
                        HashSet<Vector2> remainingCoordinates = entry.Value;
                        HashSet<Vector2> usedCoordinates = new HashSet<Vector2>();
                        if (CanMatch(entry.Key, currentRow, 0, startRow, endRow, in remainingCoordinates, ref usedCoordinates, board.endGameManager.movesLeft, ref dots))
                        {
                            yield break;
                        }
                    }
                    
                }
            }
            startRow = endRow + 2;
        }

        board.EndGame();
        yield break;

    }
}
