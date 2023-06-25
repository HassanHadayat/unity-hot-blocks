using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float speed = 1f / 30f;      // Speed at which the lava rises

    private float groundSpeed = 1f / 1f;
    private float concreteSpeed = 1f / 6f;
    private float bronzeSpeed = 1f / 8f;
    private float obsidianSpeed = 1f / 10f;
    private float diamondSpeed = 1f / 12f;

    private float groundDelay = 1f;
    private float concreteDelay = 6f;
    private float bronzeDelay = 8f;
    private float obsidianDelay = 10f;
    private float diamondDelay = 12f;

    private void Start()
    {
    }

    private void Update()
    {

        // Move the lava upwards
        transform.Translate(Vector3.up * speed * Time.deltaTime);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter");
        float delayTime = 1f;
        if (collision.tag == "Grid Block")
        {
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
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DestroyGridBlock(GameObject gridBlock, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gridBlock.GetComponent<GridBlock>().setStatus(GridBlockStatus.empty);
    }
}
