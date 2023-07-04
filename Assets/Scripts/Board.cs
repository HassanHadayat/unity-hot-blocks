using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public GameUIManager GameUIManager;
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
        for (int x = -10; x < (boardSize.y - 10); x++, i++)
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
                grid[i, j].index = new Vector2Int(i, j);
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


    public GridBlockStatus CheckRowStatus(int row)
    {
        return grid[row, 0].status;
    }
    public void BrokeRow(int row)
    {
        for (int j = 0; j < boardSize.x; j++)
        {
            grid[row, j].Broke();
        }
    }
    public void DestroyRow(int row)
    {
        for (int j = 0; j < boardSize.x; j++)
        {
            grid[row, j].setStatus(GridBlockStatus.empty);
        }
    }
    public void MoveBlockDownward(int j)
    {
        for (int i = 0; i < boardSize.y; i++)
        {
            if (i < boardSize.y - 1)
                grid[i, j].setStatus(grid[i + 1, j].status);
            else
            {
                if (grid[i - 1, j].status != GridBlockStatus.empty && grid[i - 1, j].status != GridBlockStatus.activeEmpty)
                    grid[i, j].setStatus(GridBlockStatus.activeEmpty);
                else
                    grid[i, j].setStatus(GridBlockStatus.empty);
            }

        }
    }
    public void MoveRowsDownward()
    {
        for (int i = 0; i < boardSize.y; i++)
        {
            for (int j = 0; j < boardSize.x; j++)
            {
                if (i < boardSize.y - 1)
                    grid[i, j].setStatus(grid[i + 1, j].status);
                else
                {
                    if (grid[i - 1, j].status != GridBlockStatus.empty && grid[i - 1, j].status != GridBlockStatus.activeEmpty)
                        grid[i, j].setStatus(GridBlockStatus.activeEmpty);
                    else
                        grid[i, j].setStatus(GridBlockStatus.empty);
                }

            }
        }
    }
    public bool MagicBlockPowerup(GameObject item)
    {
        try
        {
            bool isValid = IsValidPosition(new List<GameObject> { item });
            if (isValid)
            {
                Vector3 tilePosition = item.transform.position;
                Vector2Int index = gridIndex[tilePosition];
                int randomStatus = UnityEngine.Random.Range(3, 7);// concrete - diamond

                for (int j = 0; j < boardSize.x; j++)
                {
                    grid[index.x, j].setStatus((GridBlockStatus)randomStatus);
                    if (index.x + 1 < boardSize.y)
                    {
                        if (grid[index.x + 1, j].status == GridBlockStatus.empty)
                            grid[index.x + 1, j].setStatus(GridBlockStatus.activeEmpty);
                    }
                }
            }
            return isValid;
        }
        catch
        {

            return false;
        }
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
        catch
        {
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
                AudioManager.instance?.PlaySFX(SfxAudioClip.blockPlacing);

                int preRowCount = CompletedRowCount();
                // make the upper tile active for next block
                for (int i = 0; i < piecesList.Count; i++)
                {
                    Vector3 tilePosition = piecesList[i].transform.position;
                    tilePosition.y += 1f;
                    Vector2Int index = gridIndex[tilePosition];
                    if (grid[index.x, index.y].status == GridBlockStatus.empty)
                        grid[index.x, index.y].setStatus(GridBlockStatus.activeEmpty);
                }

                // placed the new piece sprites
                for (int i = 0; i < piecesList.Count; i++)
                {
                    Vector3 tilePosition = piecesList[i].transform.position;
                    Vector2Int index = gridIndex[tilePosition];

                    grid[index.x, index.y].setStatus(GridBlockStatus.ground);
                }

                // update to new material if combo rows
                int postRowCount = CompletedRowCount();
                int rowsCount = (postRowCount - preRowCount);
                ScoringSystem.instance.UpdateScore(rowsCount, piecesList.Count);

                if (rowsCount > 0)
                {
                    TransformRows(rowsCount);
                }

            }
            return isValid;
        }
        catch
        {
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
            AudioManager.instance?.PlaySFX(SfxAudioClip.rowComplete);
        }
        else if (rowsCount == 2)
        {
            newRowMat = GridBlockStatus.bronze;
            AudioManager.instance?.PlaySFX(SfxAudioClip.rowCompleteCombo);
        }
        else if (rowsCount == 3)
        {
            newRowMat = GridBlockStatus.obsidian;
            AudioManager.instance?.PlaySFX(SfxAudioClip.rowCompleteCombo);
        }
        else if (rowsCount == 4)
        {
            newRowMat = GridBlockStatus.diamond;
            AudioManager.instance?.PlaySFX(SfxAudioClip.rowCompleteCombo);
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

    public bool AnyActiveBlockRem()
    {
        for (int i = 0; i < boardSize.y; i++)
        {
            for (int j = 0; j < boardSize.x; j++)
            {
                if (grid[i, j].status == GridBlockStatus.activeEmpty)
                    return true;
            }
        }
        return false;
    }

}
