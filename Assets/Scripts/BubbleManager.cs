using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{

    public GameObject bubbleTetrominoPrefab;
    public GameObject bubblePowerupPrefab;
    public TetrominoData[] tetrominoes;
    public PowerupData[] powerups;
    public float powerupSpawnTimer = 20f;
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
        InvokeRepeating("SpawnBubble", 0f, 3f);
    }
    private void SpawnBubble()
    {
        powerupSpawnTimer -= 3f;
        if(powerupSpawnTimer <= 0f)
        {
            // Spawn Bubble Powerup
            SpawnBubblePowerup();
            powerupSpawnTimer = 20f;
        }
        else
        {
            // Spawn Bubble Tetromino
            SpawnBubbleTetromino();
        }
    }
    private void SpawnBubbleTetromino()
    {
        float randomValue = Random.Range(-3.5f, 4.5f) - 0.5f;
        spawnPosition.x = randomValue;

        TetrominoData data = tetrominoes[Random.Range(0, tetrominoes.Length)];

        Vector3 spawnPos = spawnPosition;
        GameObject newBubble = Instantiate(bubbleTetrominoPrefab, spawnPos, Quaternion.identity, this.transform);

        newBubble.GetComponent<BubbleTetromino>().Initialize(GetComponent<Board>(), spawnPos, data);
        
    }


    private void SpawnBubblePowerup()
    {
        Debug.Log("Spawn Poerup");
        float randomValue = Random.Range(-3.5f, 4.5f) - 0.5f;
        spawnPosition.x = randomValue;

        PowerupData data = powerups[Random.Range(0, powerups.Length)];
        
        Vector3 spawnPos = spawnPosition;
        GameObject newBubble = Instantiate(bubblePowerupPrefab, spawnPos, Quaternion.identity, this.transform);

        newBubble.GetComponent<BubblePowerup>().Initialize(GetComponent<Board>(), spawnPos, data);

    }
}
