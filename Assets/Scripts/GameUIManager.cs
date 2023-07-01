using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject restartBtn;
    public GameObject Score;
    public GameObject PauseBtn;
    public TextMeshProUGUI scoreText;
    public GameObject gameEndPanel;
    public TextMeshProUGUI gameEndYourScoreText;
    public TextMeshProUGUI gameEndHighestScoreText;
    public GameObject UICanvas;
   
    public void GameEnd()
    {
        restartBtn.SetActive(false);
        Score.SetActive(false);
        PauseBtn.SetActive(false);
        Time.timeScale = 0f;
        gameEndPanel.SetActive(true);
        gameEndHighestScoreText.text = (PlayerPrefs.GetInt("HighestScore", 0)).ToString();
        gameEndYourScoreText.text = scoreText.text;

        int newScore = int.Parse(scoreText.text);
        if (newScore > PlayerPrefs.GetInt("HighestScore", 0))
        {
            PlayerPrefs.SetInt("HighestScore", newScore);
        }
    }
    public void UpdateScore(int score)
    {
        int newScore = int.Parse(scoreText.text) + score;
        scoreText.text = (newScore).ToString();
    }
    public void OnClick_HomeBtn()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void OnClick_RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnClick_PauseBtn()
    {
        Time.timeScale = 0f;
    }
    public void OnClick_ResumeBtn()
    {
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
