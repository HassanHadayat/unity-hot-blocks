using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject restartBtn;
    public GameObject score;
    public GameObject pauseBtn;
    public TextMeshProUGUI scoreText;
    public GameObject gameEndPanel;
    public TextMeshProUGUI gameEndYourScoreText;
    public TextMeshProUGUI gameEndHighestScoreText;
    public GameObject UICanvas;


    public GameObject restartBtnTargetPos;
    public GameObject scoreTargetPos;
    public GameObject pauseBtnTargetPos;

    private void Start()
    {
        // Move the RectTransform to the target position with an ease-inbounce effect
        restartBtn.GetComponent<RectTransform>().DOLocalMove(restartBtnTargetPos.transform.localPosition, 0.5f).SetEase(Ease.InBounce);
        pauseBtn.GetComponent<RectTransform>().DOLocalMove(pauseBtnTargetPos.transform.localPosition, 0.5f).SetEase(Ease.InBounce);

        score.GetComponent<RectTransform>().DOLocalMove(scoreTargetPos.transform.localPosition, 1f).SetEase(Ease.InBounce);
    }
    public void GameEnd()
    {
        restartBtn.SetActive(false);
        score.SetActive(false);
        pauseBtn.SetActive(false);
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
    public void UpdateScore()
    {
        scoreText.text = (ScoringSystem.instance.currScore).ToString();
    }

    public void OnPressedDown_Btn(Transform btnTextTransform)
    {
        btnTextTransform.localPosition = new Vector3(btnTextTransform.localPosition.x, btnTextTransform.localPosition.y - 10f, btnTextTransform.localPosition.z);
    }
    public void OnPressedUp_Btn(Transform btnTextTransform)
    {
        btnTextTransform.localPosition = new Vector3(btnTextTransform.localPosition.x, btnTextTransform.localPosition.y + 10f, btnTextTransform.localPosition.z);
    }

    public void OnClick_HomeBtn()
    {
        AudioManager.instance?.PlayUI();
        SceneManager.LoadScene("MainMenuScene");
    }
    public void OnClick_RestartBtn()
    {
        AudioManager.instance?.PlayUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnClick_PauseBtn()
    {
        AudioManager.instance?.PlayUI();
        Time.timeScale = 0f;
    }
    public void OnClick_ResumeBtn()
    {
        AudioManager.instance?.PlayUI();
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
