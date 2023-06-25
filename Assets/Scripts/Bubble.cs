using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] pieceCells { get; private set; }
    public Vector3 position { get; private set; }
    public Vector3 prevPosition;
    public GameObject piece;
    public List<GameObject> piecesList = new List<GameObject>();
    public GameObject groundPrefab;

    public SpriteRenderer bubbleSpriteRenderer;
    public Collider2D bubbleCol;

    private bool isFalling = true;
    public Vector3 dragOffset = new Vector3(0f, 0.2f, 0f);
    private bool isActive = true;

    public void Initialize(Board board, Vector3 position, TetrominoData data)
    {
        this.data = data;
        this.board = board;
        this.position = position;

        // Store Bubble Piece Cell data
        if (pieceCells == null)
        {
            pieceCells = new Vector3Int[data.cells.Length];
        }
        for (int i = 0; i < pieceCells.Length; i++)
        {
            pieceCells[i] = (Vector3Int)data.cells[i];
        }

        // Apply Rotation randomly
        ApplyRotationMatrix();

        // Instantiate Bubble Piece
        this.transform.position = position;
        for (int i = 0; i < pieceCells.Length; i++)
        {
            float alignmentX = 0f;
            float alignmentY = 0f;
            //if (this.data.tetromino == Tetromino.I || this.data.tetromino == Tetromino.O)
            //{
            //    //alignmentX = 0.1f;
            //}
            //if (this.data.tetromino == Tetromino.I || this.data.tetromino == Tetromino.S || this.data.tetromino == Tetromino.Z)
            //{
            //    //alignmentY = 0.1f;
            //}

            //Vector3 pos = new Vector3((pieceCells[i].x * 0.2f) - alignmentX, (pieceCells[i].y * 0.2f) - alignmentX - alignmentY, 0f);
            Vector3 pos = new Vector3(pieceCells[i].x - alignmentX, pieceCells[i].y - alignmentX - alignmentY, 0f);
            GameObject p = Instantiate(groundPrefab, piece.transform);
            p.transform.localPosition = pos;

            piecesList.Add(p);
        }

        ShrinkPiece();
    }
    private void Update()
    {
        //Debug.Log("Prev Pos = " + prevPosition);
        if (isFalling && isActive)
            Fall(new Vector3(0f, -Time.deltaTime, 0f));
    }
    private void Fall(Vector3 translation)
    {
        Vector3 newPosition = transform.position;
        newPosition.y += translation.y;

        transform.position = newPosition;
        position = newPosition;

    }

    private void ApplyRotationMatrix()
    {
        int randValue = Random.Range(0, 3);
        if (randValue == 0) return;
        int direction = (randValue == 1) ? -1 : 1;

        float[] matrix = Data.RotationMatrix;

        // Rotate all of the cells using the rotation matrix
        for (int i = 0; i < pieceCells.Length; i++)
        {
            Vector3 cell = pieceCells[i];

            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    // "I" and "O" are rotated from an offset center point
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            pieceCells[i] = new Vector3Int(x, y, 0);
        }
    }


    private void OnMouseDown()
    {
        if (!isActive) return;
        prevPosition = transform.position;
        isFalling = false;
        PopBubble();
        ExpandPiece();
    }
    private void OnMouseDrag()
    {
        if (!isActive) return;

        if (!isFalling)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //Vector3 newPosition = worldPosition + dragOffset;
            Vector3 newPosition = worldPosition;
            newPosition.x = Mathf.Round(newPosition.x) - 0.5f;
            newPosition.y = Mathf.Round(newPosition.y) - 0.5f;

            bool isValid = board.IsValidPosition(piecesList);
            if (isValid)
            {
                for (int i = 0; i < piecesList.Count; i++)
                {
                    piecesList[i].transform.localScale = Vector3.one * 0.7f;
                }
            }
            else
            {
                for (int i = 0; i < piecesList.Count; i++)
                {
                    piecesList[i].transform.localScale = Vector3.one;
                }
            }

            transform.position = newPosition;

            position = newPosition;
        }
    }
    private void OnMouseUp()
    {
        if (!isActive) return;

        if (!isFalling)
        {
            bool isValid = board.IsValidMove(piecesList);
            if (isValid)
            {
                // destroy the piece
                Destroy(this.gameObject);
            }
            else
            {
                // Normal size
                for (int i = 0; i < piecesList.Count; i++)
                {
                    piecesList[i].transform.localScale = Vector3.one;
                }

                ShrinkPiece();

                bubbleSpriteRenderer.enabled = true;
                bubbleCol.enabled = true;
                isFalling = true;
            }
        }
    }
    private void PopBubble()
    {
        bubbleSpriteRenderer.enabled = false;
        bubbleCol.enabled = false;
    }
    private void ShrinkPiece()
    {
        piece.transform.localScale = Vector3.one * 0.3f;
    }
    private void ExpandPiece()
    {
        piece.transform.localScale = Vector3.one;
    }
}
