using System.Collections.Generic;
using TMPro;
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

    public SpriteRenderer bubbleFrontSpriteRenderer;
    public SpriteRenderer bubbleBackSpriteRenderer;
    public Collider2D bubbleCol;

    private bool isFalling = true;
    public Vector3 dragOffset = new Vector3(0f, 0.2f, 0f);
    private bool isActive = true;
    public bool isRotated = false;


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
        isRotated = ApplyRotationMatrix();

        // Instantiate Bubble Piece
        this.transform.position = position;
        for (int i = 0; i < pieceCells.Length; i++)
        {
            Vector3 pos = new Vector3(pieceCells[i].x, pieceCells[i].y, 0f);
            GameObject p = Instantiate(groundPrefab, piece.transform);
            p.transform.localPosition = pos;

            piecesList.Add(p);
        }

        ShrinkPiece();
    }
    private void Update()
    {
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

    private bool ApplyRotationMatrix()
    {
        // no rotation for Tetromino O required
        if (data.tetromino == Tetromino.A || data.tetromino == Tetromino.O) { return false; }

        int randValue = Random.Range(0, 3);

        // 0 for no rotate
        if (randValue == 0) return false;

        // only one direction rotation for straight Tetromino
        if (data.tetromino == Tetromino.B || data.tetromino == Tetromino.I)
            randValue = 2;

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
        return true;
    }


    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        if (!isActive) return;
        prevPosition = transform.position;
        isFalling = false;
        PopBubble();
        ExpandPiece();
    }
    private void OnMouseDrag()
    {
        if (Time.timeScale == 0) return;

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
        if (Time.timeScale == 0) return;
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

                bubbleFrontSpriteRenderer.enabled = true;
                bubbleBackSpriteRenderer.enabled = true;
                bubbleCol.enabled = true;
                isFalling = true;
            }
        }
    }
    private void PopBubble()
    {
        bubbleFrontSpriteRenderer.enabled = false;
        bubbleBackSpriteRenderer.enabled = false;
        bubbleCol.enabled = false;
    }
    private void ShrinkPiece()
    {
        // Downscale the Piece
        piece.transform.localScale = Vector3.one * 0.3f;

        float alignmentX = 0f;
        float alignmentY = 0f;

        if (this.data.tetromino == Tetromino.O)
        {
            alignmentX = -0.15f;
            alignmentY = -0.15f;
        }
        else if (this.data.tetromino == Tetromino.B || this.data.tetromino == Tetromino.I)
        {
            if (isRotated)
            {
                if (this.data.tetromino == Tetromino.B)
                    alignmentY = 0.15f;
                else
                    alignmentY = -0.15f;
            }
            else
            {
                alignmentX = -0.15f;
            }
        }

        Vector3 prePos = piece.transform.localPosition;
        piece.transform.localPosition = new Vector3(prePos.x + alignmentX, prePos.y + alignmentY, prePos.z);

    }
    private void ExpandPiece()
    {
        float alignmentX = 0f;
        float alignmentY = 0f;

        if (this.data.tetromino == Tetromino.O)
        {
            alignmentX = 0.15f;
            alignmentY = 0.15f;
        }
        else if (this.data.tetromino == Tetromino.B || this.data.tetromino == Tetromino.I)
        {
            if (isRotated)
            {
                if (this.data.tetromino == Tetromino.B)
                    alignmentY = -0.15f;
                else
                    alignmentY = 0.15f;
            }
            else
            {
                alignmentX = 0.15f;
            }
        }

        Vector3 prePos = piece.transform.localPosition;
        piece.transform.localPosition = new Vector3(prePos.x + alignmentX, prePos.y + alignmentY, prePos.z);


        // Upscale the Piece
        piece.transform.localScale = Vector3.one;
    }
}
