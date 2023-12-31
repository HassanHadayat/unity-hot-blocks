using System.Collections.Generic;
using UnityEngine;

public enum GridBlockStatus { empty, activeEmpty, ground, concrete, bronze, obsidian, diamond };
public class GridBlock : MonoBehaviour
{
    public Vector2Int index;
    public GridBlockStatus status = GridBlockStatus.empty;

    public SpriteRenderer spriteRenderer;
    public Sprite[] blockSprites;
    public Dictionary<GridBlockStatus, Sprite> sprites;
    public GameObject brokeSprite;

    private void Start()
    {
        sprites = new Dictionary<GridBlockStatus, Sprite>();
        sprites.Add(GridBlockStatus.empty, blockSprites[0]);
        sprites.Add(GridBlockStatus.activeEmpty, blockSprites[6]);
        sprites.Add(GridBlockStatus.ground, blockSprites[1]);
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
        brokeSprite.SetActive(false);

    }
    public void Broke()
    {
        if (status != GridBlockStatus.empty && status != GridBlockStatus.activeEmpty)
            brokeSprite.SetActive(true);

    }
}
