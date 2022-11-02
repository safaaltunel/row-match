using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public bool amICheckmark = false;
    public float swipeAngle = 0;
    public Sprite checkmark;
    public GameObject destroyEffect;

    private EndGameManager endGameManager;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    private GameObject otherDot;
    private Board board;
    

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        endGameManager = FindObjectOfType<EndGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMatched && !amICheckmark)
        {
            amICheckmark = true;
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.sprite = checkmark;
            GameObject particle = Instantiate(destroyEffect, this.transform.position, Quaternion.identity);
            Destroy(particle, .5f);
        }
        targetX = column;
        targetY = row;

        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            // Move towards target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if (board.allDots[column, row] != this.gameObject)
                board.allDots[column, row] = this.gameObject;
        }
        else
        {
            // Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move towards target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if (board.allDots[column, row] != this.gameObject)
                board.allDots[column, row] = this.gameObject;       
        }
        else
        {
            // Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    private void OnMouseDown()
    {
        if(board.currentState == GameState.MOVE)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if(board.currentState == GameState.MOVE)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if ((firstTouchPosition.x == finalTouchPosition.x && firstTouchPosition.y == finalTouchPosition.y) || isMatched)
                return;
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        board.currentState = GameState.WAIT;
        MovePieces();
        board.currentState = GameState.MOVE;
    }

    private void MovePieces(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        if (otherDot.GetComponent<Dot>().isMatched)
            return;
        otherDot.GetComponent<Dot>().column -= (int)direction.x;
        otherDot.GetComponent<Dot>().row -= (int)direction.y;

        column += (int)direction.x;
        row += (int)direction.y;

        if(direction == Vector2.up || direction == Vector2.down)
        {
            board.FindMatch(row);
            board.FindMatch(row - (int)direction.y);
        }
        board.FindDeadLock();
    }

    void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            // right swipe
            MovePieces(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            // up swipe
            MovePieces(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            // left swipe
            MovePieces(Vector2.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // down swipe
            MovePieces(Vector2.down);
        }

        endGameManager.DecreaseCounter();
    }
}
