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
        grid = new GridBlock[boardSize.x, boardSize.y];
        gridIndex = new Dictionary<Vector3, Vector2Int>();
        // Create Grid
        int i = 0;
        for (int x = (-(boardSize.x / 2)); x < boardSize.x / 2; x++)
        {
            int j = 0;
            for (int y = (-(boardSize.y / 2)); y < boardSize.y / 2; y++)
            {
                Vector3 position = new Vector3(x + 0.5f, y + 0.5f, 0);
                GameObject gridBlock = Instantiate(gridPrefab, position, Quaternion.identity);
                gridBlock.name = "Grid (" + x + ", " + y + ")";
                gridBlock.transform.parent = transform;
                if (y == (-(boardSize.y / 2)))
                {
                    StartCoroutine(delay(gridBlock.GetComponent<GridBlock>()));
                }
                grid[i, j] = gridBlock.GetComponent<GridBlock>();
                gridIndex.Add(position, new Vector2Int(i, j));
                j++;
            }
            i++;
        }

        // Create Border
        Instantiate(borderPrefab, Vector3.zero, Quaternion.identity, this.transform);

    }
    private IEnumerator delay(GridBlock gb)
    {
        yield return new WaitForSeconds(1f);
        gb.setStatus(GridBlockStatus.activeEmpty);
    }
    public bool isValidMove(Vector3Int[] pieceCells, Vector3 piecePos, List<GameObject> piecesList)
    {
        try
        {

            bool activeCheck = false;

            for (int i = 0; i < piecesList.Count; i++)
            {
                //Vector3 tilePosition = new Vector3((float)pieceCells[i].x + 0.5f, (float)pieceCells[i].y + 0.5f, (float)pieceCells[i].z);
                Vector3 tilePosition = piecesList[i].transform.position;
                Vector2Int index = gridIndex[tilePosition];
                if (grid[index.x, index.y].status != GridBlockStatus.empty && grid[index.x, index.y].status != GridBlockStatus.activeEmpty)
                {
                    return false;
                }
                if (grid[index.x, index.y].status == GridBlockStatus.activeEmpty)
                {
                    activeCheck = true;
                }
            }
            if (activeCheck)
            {
                for (int i = 0; i < piecesList.Count; i++)
                {
                    Vector3 tilePosition = piecesList[i].transform.position;
                    tilePosition.y += 1f;
                    Vector2Int index = gridIndex[tilePosition];
                    grid[index.x, index.y].setStatus(GridBlockStatus.activeEmpty);
                }
                for (int i = 0; i < piecesList.Count; i++)
                {
                    Vector3 tilePosition = piecesList[i].transform.position;
                    Vector2Int index = gridIndex[tilePosition];

                    grid[index.x, index.y].setStatus(GridBlockStatus.brick);
                }

                return true;
            }

            else
                return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }
}
