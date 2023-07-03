using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public static ScoringSystem instance;
    public GameUIManager gameUIManager;
    public int currScore { get; private set; }

    public List<int> linePoints = new List<int> { 0, 40, 100, 300, 1200 };
    public bool isPoints2x { get; private set; }
    private float points2xTimer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(this);
    }
    private void Update()
    {
        if(isPoints2x)
        {
            points2xTimer -= Time.deltaTime;
            if(points2xTimer <= 0f)
            {
                stop2xPoints();
            }
        }
    }
    public void UpdateScore(int rowsCount, int piecesCount)
    {
        if (!isPoints2x)
            currScore += linePoints[rowsCount] + piecesCount;
        else
            currScore += (linePoints[rowsCount] + piecesCount) * 2;

        gameUIManager.UpdateScore();
    }
    public void start2xPoints()
    {
        points2xTimer = 15f;
        isPoints2x = true;
    }
    public void stop2xPoints()
    {
        isPoints2x = false;
        points2xTimer = 0f;
    }
}
