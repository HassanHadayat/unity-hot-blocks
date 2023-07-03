using System.Collections.Generic;
using UnityEngine;

public enum Tetromino
{
    A, B,
    I, J, L, O, S, T, Z
}

[System.Serializable]
public struct TetrominoData
{
    [HideInInspector]
    public Sprite sprite;
    public Tetromino tetromino;
    public GameObject prefab;

    public Vector2Int[] cells { get; private set; }

    public void Initialize()
    {
        cells = Data.Cells[tetromino];
        //Debug.Log("Cells : " + cells.Length);
        //foreach (var cell in cells)
        //{
        //    Debug.Log("Cell Item : " + cell);
        //}
    }

}
