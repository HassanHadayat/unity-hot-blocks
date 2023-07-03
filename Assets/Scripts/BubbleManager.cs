using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{

    public GameObject bubbleTetrominoPrefab;
    public GameObject bubblePowerupPrefab;
    public List<GameObject> bubbles;
    public TetrominoData[] tetrominoes;
    public PowerupData[] powerups;

    public Vector3 spawnPosition = new Vector3(-4.5f, 15f, 0f);

    private void Awake()
    {
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }

        for (int i = 0; i < powerups.Length; i++)
        {
            powerups[i].Initialize();
        }
    }

    private void Start()
    {
        InvokeRepeating("SpawnBubbleTetromino", 0f, 4f);
        InvokeRepeating("SpawnBubblePowerup", 20f, 20f);
    }
    private void SpawnBubbleTetromino()
    {
        float randomValue = Random.Range(-3.5f, 4.5f) - 0.5f;
        spawnPosition.x = randomValue;

        TetrominoData data = tetrominoes[Random.Range(0, tetrominoes.Length)];

        Vector3 spawnPos = spawnPosition;
        GameObject newBubble = Instantiate(bubbleTetrominoPrefab, spawnPos, Quaternion.identity, this.transform);

        newBubble.GetComponent<BubbleTetromino>().Initialize(GetComponent<Board>(), spawnPos, data);
        bubbles.Add(newBubble);
    }


    private void SpawnBubblePowerup()
    {
        float randomValue = Random.Range(-3.5f, 4.5f) - 0.5f;
        spawnPosition.x = randomValue;

        PowerupData data = powerups[Random.Range(0, powerups.Length)];
        
        Vector3 spawnPos = spawnPosition;
        GameObject newBubble = Instantiate(bubblePowerupPrefab, spawnPos, Quaternion.identity, this.transform);

        newBubble.GetComponent<BubblePowerup>().Initialize(GetComponent<Board>(), spawnPos, data);
        bubbles.Add(newBubble);
    }
}
