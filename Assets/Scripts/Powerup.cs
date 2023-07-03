
using UnityEngine;

public enum Powerup
{
    FreezeLava, Points2x, MagicBlock
}

[System.Serializable]
public struct PowerupData
{
    public Sprite sprite;
    public Powerup powerup;
    public GameObject prefab;

    public Vector2Int[] cells { get; private set; }

    public void Initialize()
    {
        cells = new Vector2Int[] { new Vector2Int(0, 0) };
    }

}