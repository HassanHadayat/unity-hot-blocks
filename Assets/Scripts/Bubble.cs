using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    public GameObject container { get; private set; }
    [SerializeField] private SpriteRenderer frontSR;
    [SerializeField] private SpriteRenderer backSR;
    [SerializeField] private Collider2D col;

    public Board board { get; set; }
    public Vector3Int[] cells { get; set; }

    public GameObject itemPrefab;
    [HideInInspector] public List<GameObject> itemsList = new List<GameObject>();


    private Vector3 prevPosition;
    public bool isFalling = true;
    public bool isActive = true;

    private void Awake()
    {
        container = transform.GetChild(1).gameObject;
        frontSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        backSR = transform.GetChild(2).GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (isFalling && isActive)
        {
            Fall(new Vector3(0f, -Time.deltaTime*2.5f, 0f));

        }
    }

    private void Fall(Vector3 translation)
    {
        Vector3 newPosition = transform.position;
        newPosition.y += translation.y;

        transform.position = newPosition;
    }
    public void PopBubble()
    {
        frontSR.enabled = false;
        backSR.enabled = false;
        col.enabled = false;
    }
    public void ReformBubble()
    {
        frontSR.enabled = true;
        backSR.enabled = true;
        col.enabled = true;
    }

    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        if (!isActive) return;
        prevPosition = transform.position;
        isFalling = false;
        AudioManager.instance?.PlaySFX(SfxAudioClip.bubblePop);
        Expand();
    }

    private void OnMouseDrag()
    {
        if (Time.timeScale == 0) return;
        if (!isActive) return;

        if (!isFalling)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //Vector3 newPosition = worldPosition + dragOffset;
            Vector3 newPosition = worldPosition;
            newPosition.x = Mathf.Round(newPosition.x) - 0.5f;
            newPosition.y = Mathf.Round(newPosition.y) - 0.5f;

            bool isValid = board.IsValidPosition(itemsList);
            if (isValid)
            {
                for (int i = 0; i < itemsList.Count; i++)
                {
                    itemsList[i].transform.localScale = Vector3.one * 0.7f;
                }
            }
            else
            {
                for (int i = 0; i < itemsList.Count; i++)
                {
                    itemsList[i].transform.localScale = Vector3.one;
                }
            }

            transform.position = newPosition;
        }
    }
    public abstract void Shrink();
    public abstract void Expand();
}
