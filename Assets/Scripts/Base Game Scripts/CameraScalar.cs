using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour
{

    private Board board;
    public float cameraOffset;
    public float padding;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        if (board != null)
        {
            RepositionCamera(board.width - 1, board.height - 1, .56f);
        }
    }

    void RepositionCamera(float x, float y, float aspectRatio)
    {
        Vector3 tempPosition = new Vector3(x/2, y/2, cameraOffset);
        transform.position = tempPosition;
        Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
    }
}
