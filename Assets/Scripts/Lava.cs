using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public GameUIManager GameUIManager;
    public float speed = 1f / 90f;      // Speed at which the lava rises

    private float groundSpeed = 1f / 2f;
    private float concreteSpeed = 1f / 7f;
    private float bronzeSpeed = 1f / 9f;
    private float obsidianSpeed = 1f / 11f;
    private float diamondSpeed = 1f / 13f;

    private float groundDelay =2f;
    private float concreteDelay = 7f;
    private float bronzeDelay = 9f;
    private float obsidianDelay = 11f;
    private float diamondDelay = 13f;

    private void Update()
    {
        // Move the lava upwards
        transform.Translate(Vector3.up * speed * Time.deltaTime);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float delayTime = 1f;
        if (collision.tag == "Grid Block")
        {
            Debug.Log("Lava Touched Grid Block");

            GridBlockStatus _status = collision.GetComponent<GridBlock>().status;
            if (_status == GridBlockStatus.ground)
            {
                Debug.Log("Ground");
                delayTime = groundDelay;
                speed = groundSpeed;
            }
            else if (_status == GridBlockStatus.concrete)
            {
                Debug.Log("concrete");
                delayTime = concreteDelay;
                speed = concreteSpeed;
            }
            else if (_status == GridBlockStatus.bronze)
            {
                Debug.Log("bronze");
                delayTime = bronzeDelay;
                speed = bronzeSpeed;
            }
            else if (_status == GridBlockStatus.obsidian)
            {
                Debug.Log("obsidian");
                delayTime = obsidianDelay;
                speed = obsidianSpeed;
            }
            else if (_status == GridBlockStatus.diamond)
            {
                Debug.Log("diamond");
                delayTime = diamondDelay;
                speed = diamondSpeed;
            }

            StartCoroutine(DestroyGridBlock(collision.gameObject, delayTime));
        }
        else if(collision.tag == "Bubble")
        {
            Debug.Log("Lava Touched Bubble");

            Destroy(collision.gameObject);
        }
        else if(collision.tag == "Border")
        {
            Debug.Log("Lava Touched Border End Line.... Game End");

            GameUIManager.GameEnd();
        }
    }

    IEnumerator DestroyGridBlock(GameObject gridBlock, float delayTime)
    {
        gridBlock.GetComponent<GridBlock>().Broke();
        yield return new WaitForSeconds(delayTime);
        gridBlock.GetComponent<GridBlock>().setStatus(GridBlockStatus.empty);
    }
}
