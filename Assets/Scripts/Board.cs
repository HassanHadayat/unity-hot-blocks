using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;


public class Board : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject borderPrefab;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public GridBlock[,] grid;
    private Dictionary<Vector3, Vector2Int> gridIndex;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new GridBlock[boardSize.y, boardSize.x];
        gridIndex = new Dictionary<Vector3, Vector2Int>();
        // Create Grid
        int i = 0;
        for (int x = (-(boardSize.y / 2)); x < boardSize.y / 2; x++, i++)
        {
            int j = 0;
            for (int y = (-(boardSize.x / 2)); y < boardSize.x / 2; y++, j++)
            {
                Vector3 position = new Vector3(y + 0.5f, x + 0.5f, 0);
                GameObject gridBlock = Instantiate(gridPrefab, position, Quaternion.identity);
                gridBlock.name = "Grid (" + i + ", " + j + ")";
                gridBlock.transform.parent = transform;
                if (x == (-(boardSize.y / 2)))
                {
                    StartCoroutine(DelayInitializeGridBlock(gridBlock.GetComponent<GridBlock>()));
                }
                grid[i, j] = gridBlock.GetComponent<GridBlock>();
                gridIndex.Add(position, new Vector2Int(i, j));
            }
        }

        // Create Border
        Instantiate(borderPrefab, Vector3.zero, Quaternion.identity, this.transform);

    }
    private IEnumerator DelayInitializeGridBlock(GridBlock gb)
    {
        yield return new WaitForSeconds(1f);
        gb.setStatus(GridBlockStatus.activeEmpty);
    }


    public bool IsValidPosition(List<GameObject> piecesList)
    {
        try
        {
            bool isValid = false;
            for (int i = 0; i < piecesList.Count; i++)
            {
                Vector3 tilePosition = piecesList[i].transform.position;
                Vector2Int index = gridIndex[tilePosition];
                if (grid[index.x, index.y].status != GridBlockStatus.empty && grid[index.x, index.y].status != GridBlockStatus.activeEmpty)
                {
                    return false;
                }
                if (grid[index.x, index.y].status == GridBlockStatus.activeEmpty)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }
    public bool IsValidMove(List<GameObject> piecesList)
    {
        bool isValid = IsValidPosition(piecesList);
        try
        {
            if (isValid)
            {
                int preRowCount = CompletedRowCount();
                // make the upper tile active for next block
                Debug.Log("First Loop");
                for (int i = 0; i < piecesList.Count; i++)
                {
                    Vector3 tilePosition = piecesList[i].transform.position;
                    tilePosition.y += 1f;
                    Vector2Int index = gridIndex[tilePosition];
                    if (grid[index.x, index.y].status == GridBlockStatus.empty)
                        grid[index.x, index.y].setStatus(GridBlockStatus.activeEmpty);
                }

                Debug.Log("Second Loop");
                // placed the new piece sprites
                for (int i = 0; i < piecesList.Count; i++)
                {
                    Vector3 tilePosition = piecesList[i].transform.position;
                    Vector2Int index = gridIndex[tilePosition];

                    grid[index.x, index.y].setStatus(GridBlockStatus.ground);
                }

                // update to new material if combo rows
                int postRowCount = CompletedRowCount();
                if (postRowCount - preRowCount > 0)
                {
                    TransformRows(postRowCount - preRowCount);
                }

            }
            return isValid;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    private int CompletedRowCount()
    {
        int count = 0;
        for (int x = 0; x < boardSize.y; x++)
        {
            bool check = true;
            for (int y = 0; y < boardSize.x; y++)
            {
                if (grid[x, y].status == GridBlockStatus.empty || grid[x, y].status == GridBlockStatus.activeEmpty)
                {
                    check = false;
                    break;
                }
            }
            if (check)
                count++;
        }
        return count;
    }
    private void TransformRows(int rowsCount)
    {
        GridBlockStatus newRowMat = GridBlockStatus.empty;
        if (rowsCount == 1)
        {
            newRowMat = GridBlockStatus.concrete;
        }
        else if (rowsCount == 2)
        {
            newRowMat = GridBlockStatus.bronze;
        }
        else if (rowsCount == 3)
        {
            newRowMat = GridBlockStatus.obsidian;
        }
        else if (rowsCount == 4)
        {
            newRowMat = GridBlockStatus.diamond;
        }
        else { return; }

        for (int x = 0; x < boardSize.y; x++)
        {
            bool isRow = true;
            for (int y = 0; y < boardSize.x; y++)
            {
                if (grid[x, y].status != GridBlockStatus.ground)
                {
                    isRow = false;
                    break;
                }
            }
            if (isRow)
            {
                for (int y = 0; y < boardSize.x; y++)
                {
                    grid[x, y].setStatus(newRowMat);
                }
            }
        }
    }
}
