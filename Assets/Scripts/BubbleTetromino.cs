using System.Collections.Generic;
using UnityEngine;


public class BubbleTetromino : Bubble
{
    public TetrominoData data { get; private set; }

    public bool isRotated = false;


    public void Initialize(Board board, Vector3 position, TetrominoData data)
    {
        this.data = data;
        this.board = board;
        this.itemPrefab = data.prefab;

        // Store Bubble Piece Cell data
        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }

        // Apply Rotation randomly
        isRotated = ApplyRotationMatrix();
        
        // Instantiate Bubble Piece
        this.transform.position = position;
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 pos = new Vector3(cells[i].x, cells[i].y, 0f);
            GameObject p = Instantiate(itemPrefab, container.transform);
            p.transform.localPosition = pos;

            itemsList.Add(p);

        }
        Shrink();
    }

    private void OnMouseUp()
    {
        if (Time.timeScale == 0) return;
        if (!isActive) return;

        if (!isFalling)
        {
            bool isValid = board.IsValidMove(itemsList);
            if (isValid)
            {
                // destroy the piece
                Destroy(this.gameObject);
            }
            else
            {
                //// Normal size
                //for (int i = 0; i < itemsList.Count; i++)
                //{
                //    itemsList[i].transform.localScale = Vector3.one;
                //}

                Shrink();

                isFalling = true;
            }
        }
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
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

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

            cells[i] = new Vector3Int(x, y, 0);
        }
        return true;
    }


    public override void Shrink()
    {
        // Downscale the Piece
        container.transform.localScale = Vector3.one * 0.3f;

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

        Vector3 prePos = container.transform.localPosition;
        container.transform.localPosition = new Vector3(prePos.x + alignmentX, prePos.y + alignmentY, prePos.z);

        ReformBubble();
    }
    public override void Expand()
    {
        PopBubble();

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

        Vector3 prePos = container.transform.localPosition;
        container.transform.localPosition = new Vector3(prePos.x + alignmentX, prePos.y + alignmentY, prePos.z);


        // Upscale the Piece
        container.transform.localScale = Vector3.one;
    }


}
