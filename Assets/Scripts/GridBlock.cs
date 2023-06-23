using System.Collections.Generic;
using UnityEngine;

public enum GridBlockStatus { empty, activeEmpty, brick, concrete, bronze, obsidian, diamond };
public class GridBlock : MonoBehaviour
{
    public GridBlockStatus status = GridBlockStatus.empty;

    public SpriteRenderer spriteRenderer;
    public Sprite[] blockSprites;
    public Dictionary<GridBlockStatus, Sprite> sprites;

    private void Start()
    {
        sprites= new Dictionary<GridBlockStatus, Sprite>();
        sprites.Add(GridBlockStatus.empty, blockSprites[0]);
        sprites.Add(GridBlockStatus.activeEmpty, blockSprites[0]);
        sprites.Add(GridBlockStatus.brick, blockSprites[1]);
        sprites.Add(GridBlockStatus.concrete, blockSprites[2]);
        sprites.Add(GridBlockStatus.bronze, blockSprites[3]);
        sprites.Add(GridBlockStatus.obsidian, blockSprites[4]);
        sprites.Add(GridBlockStatus.diamond, blockSprites[5]);

        setStatus(GridBlockStatus.empty);
    }

    public void setStatus(GridBlockStatus newStatus)
    {
        status = newStatus;
        spriteRenderer.sprite = sprites[status];
    }
}
