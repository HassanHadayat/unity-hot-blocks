using UnityEngine;


public class BubblePowerup : Bubble
{
    public PowerupData data { get; private set; }


    public void Initialize(Board board, Vector3 position, PowerupData data)
    {
        this.data = data;
        this.board = board;
        this.itemPrefab = data.prefab;
        // Store Powerup Cell data
        if (cells == null)
            cells = new Vector3Int[data.cells.Length];

        cells[0] = (Vector3Int)data.cells[0];

        // Set Bubble Position
        this.transform.position = position;
        Vector3 pos = new Vector3(cells[0].x, cells[0].y, 0f);
        GameObject item = new GameObject();
        item = Instantiate(itemPrefab, this.container.transform);

        item.transform.localPosition = pos;
        itemsList.Add(item);

        Shrink();
    }
    private void OnMouseUp()
    {
        if (Time.timeScale == 0) return;
        if (!isActive) return;
        
        if (!isFalling && data.powerup == Powerup.MagicBlock)
        {
            bool isValid = board.MagicBlockPowerup(itemsList[0]);
            if (isValid)
            {
                AudioManager.instance?.PlaySFX(SfxAudioClip.powerup);

                // destroy the piece
                Destroy(this.gameObject);
            }
            else
            {
                // Normal size
                for (int i = 0; i < itemsList.Count; i++)
                {
                    itemsList[i].transform.localScale = Vector3.one;
                }

                Shrink();

                isFalling = true;
            }
        }
    }
    public override void Shrink()
    {
        // Downscale the Piece
        container.transform.localScale = Vector3.one * 0.8f;

        ReformBubble();
    }
    public override void Expand()
    {
        PopBubble();
        // Upscale the Piece
        container.transform.localScale = Vector3.one;

        if(data.powerup == Powerup.Points2x)
        {
            AudioManager.instance?.PlaySFX(SfxAudioClip.powerup);

            ScoringSystem.instance.Start2xPoints();
            Destroy(this.gameObject);
        }
        else if(data.powerup == Powerup.FreezeLava)
        {
            AudioManager.instance?.PlaySFX(SfxAudioClip.powerup);

            Debug.Log("Freeze Lava Powerup");
            FindObjectOfType<Lava>().StartFreezeLava();
            Destroy(this.gameObject);
        }

    }

}
