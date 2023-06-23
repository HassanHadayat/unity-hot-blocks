using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{

    public GameObject bubblePrefab;
    public List<GameObject> bubbles;
    public TetrominoData[] tetrominoes;

    public Vector3 spawnPosition = new Vector3(-4.5f, 15f, 0f);

    private void Awake()
    {
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        InvokeRepeating("SpawnBubble", 0f, 5f);
        //SpawnBubble();
    }
    private void SpawnBubble()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        float randomValue = Random.Range(-4, 6) - 0.5f;
        spawnPosition.x = randomValue;

        Vector3 spawnPos = spawnPosition;
        GameObject newBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity, this.transform);
        newBubble.GetComponent<Bubble>().Initialize(GetComponent<Board>(), spawnPos, data);
        bubbles.Add(newBubble);
    }

}
