using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI highestScoreTxt;

    public GameObject[] sound;
    public GameObject[] music;

    public GameObject topLeftBtns;
    public GameObject topLeftBtnsTargetPos;
    public GameObject playBtn;
    public GameObject playBtnTargetPos;
    public GameObject highestScore;
    public GameObject highestScoreTargetPos;

    private bool isSound;
    private bool isMusic;


    private void Awake()
    {
        InitializeSettings();
    }
    private void Start()
    {
        // Move the RectTransform to the target position with an ease-inbounce effect
        playBtn.GetComponent<RectTransform>().DOLocalMove(playBtnTargetPos.transform.localPosition, 1f).SetEase(Ease.InQuart);
        topLeftBtns.GetComponent<RectTransform>().DOLocalMove(topLeftBtnsTargetPos.transform.localPosition, 1f).SetEase(Ease.InQuart);

        highestScore.GetComponent<RectTransform>().DOLocalMove(highestScoreTargetPos.transform.localPosition, 1.2f).SetEase(Ease.InQuart);
    }
    private void InitializeSettings()
    {
        highestScoreTxt.text = PlayerPrefs.GetInt("HighestScore", 0).ToString();

        isSound = PlayerPrefs.GetInt("isSound", 1) == 0 ? false : true;
        isMusic = PlayerPrefs.GetInt("isMusic", 1) == 0 ? false : true;

        if (isSound)
        {
            sound[0].SetActive(false);
            sound[1].SetActive(true);
        }
        else
        {
            sound[0].SetActive(true);
            sound[1].SetActive(false);
        }

        if (isMusic)
        {
            music[0].SetActive(false);
            music[1].SetActive(true);
        }
        else
        {
            music[0].SetActive(true);
            music[1].SetActive(false);
        }
    }

    public void OnPressedDown_PlayBtn(Transform playTransform)
    {
        playTransform.localPosition = new Vector3(playTransform.localPosition.x, playTransform.localPosition.y - 15f, playTransform.localPosition.z);
    }
    public void OnPressedUp_PlayBtn(Transform playTransform)
    {
        playTransform.localPosition = new Vector3(playTransform.localPosition.x, playTransform.localPosition.y + 15f, playTransform.localPosition.z);
    }
    public void OnClick_PlayBtn()
    {
        AudioManager.instance?.PlayUI();

        SceneManager.LoadScene("GameScene");
    }
    public void PlayClickAudio()
    {
        AudioManager.instance?.PlayUI();
    }
    public void OnClick_QuitBtn()
    {
        AudioManager.instance?.PlayUI();

        Application.Quit();
    }

    public void OnClick_SoundBtn()
    {
        AudioManager.instance?.PlayUI();

        isSound = !isSound;
        PlayerPrefs.SetInt("isSound", (isSound ? 1 : 0));
    }

    public void OnClick_MusicBtn()
    {
        AudioManager.instance?.PlayUI();

        isMusic = !isMusic;
        PlayerPrefs.SetInt("isMusic", (isMusic ? 1 : 0));
        if (isMusic)
            AudioManager.instance?.PlayBackgroundMusic();
        else
            AudioManager.instance?.StopBackgroundMusic();
    }
}
