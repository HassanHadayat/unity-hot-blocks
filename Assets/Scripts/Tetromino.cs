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

    public Vector2Int[] cells { get; private set; }

    public void Initialize()
    {
        cells = Data.Cells[tetromino];
    }

}
