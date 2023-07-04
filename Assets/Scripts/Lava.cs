using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public Board board;
    public GameUIManager GameUIManager;
    public Collider2D col;
    public SpriteRenderer spriteRenderer;
    public Material lavaMat;
    public Material snowMat;

    public float speed = 1f / 90f;      // Speed at which the lava rises

    private float groundDelay = 2f;
    private float concreteDelay = 7f;
    private float bronzeDelay = 9f;
    private float obsidianDelay = 11f;
    private float diamondDelay = 13f;

    private float intensity = 2f;
    private bool isReached = false;
    private bool isBurning = false;

    private float freezeLavaTimer;
    private bool isFreezeLava = false;
    private bool isGameEnd = false;

    private void Update()
    {
        // Freeze Lava Powerup
        if (isFreezeLava)
        {
            freezeLavaTimer -= Time.deltaTime;

            if (freezeLavaTimer <= 0)
            {
                StopFreezeLava();
            }
        }
        else
        {

            // Move the lava upwards
            if (!isReached)
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            else if (isGameEnd)
                transform.Translate(Vector3.up * 15f * Time.deltaTime);
            else
                Burn();
        }

    }
    private void Burn()
    {
        if (!isBurning)
        {
            isBurning = true;
            GridBlockStatus _status = board.CheckRowStatus(0);

            if (_status == GridBlockStatus.ground)
            {
                Debug.Log("Ground");
                intensity = groundDelay;
            }
            else if (_status == GridBlockStatus.concrete)
            {
                Debug.Log("concrete");
                intensity = concreteDelay;
            }
            else if (_status == GridBlockStatus.bronze)
            {
                Debug.Log("bronze");
                intensity = bronzeDelay;
            }
            else if (_status == GridBlockStatus.obsidian)
            {
                Debug.Log("obsidian");
                intensity = obsidianDelay;
            }
            else if (_status == GridBlockStatus.diamond)
            {
                Debug.Log("diamond");
                intensity = diamondDelay;
            }
            else
            {
                intensity = groundDelay;

                // Check Game End Condition
                if (!board.AnyActiveBlockRem())
                {
                    isGameEnd = true;
                    Invoke("DelayGameEnd", 1.5f);
                    return;
                }
            }

            StartCoroutine(DestroyGridRow());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bubble")
        {
            Debug.Log("Lava Touched Bubble");

            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Border")
        {
            Debug.Log("Lava Touched Border Start Line....");
            isReached = true;
        }
    }

    IEnumerator DestroyGridRow()
    {
        float tempIntensity = intensity;
        yield return new WaitForSeconds(tempIntensity / 2f);
        yield return new WaitUntil(() => !isFreezeLava);

        AudioManager.instance?.PlaySFX(SfxAudioClip.blockCracking);
        board.BrokeRow(0);

        yield return new WaitForSeconds(tempIntensity / 2);
        yield return new WaitUntil(() => !isFreezeLava);
        AudioManager.instance?.PlaySFX(SfxAudioClip.blockBreaking);
        board.DestroyRow(0);

        yield return new WaitForSeconds(0.2f);

        // Move All the Blocks Downward
        board.MoveRowsDownward();
        isBurning = false;
    }

    public void StartFreezeLava()
    {
        freezeLavaTimer = 10f;
        isFreezeLava = true;
        spriteRenderer.material = snowMat;
    }
    public void StopFreezeLava()
    {
        isFreezeLava = false;
        freezeLavaTimer = 0f;
        spriteRenderer.material = lavaMat;
    }

    private void DelayGameEnd()
    {
        GameUIManager.GameEnd();
    }
}
