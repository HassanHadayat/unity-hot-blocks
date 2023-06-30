using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI highestScoreTxt;

    public Sprite[] soundSprites;
    public Sprite[] musicSprites;

    public Image soundBtnImage;
    public Image musicBtnImage;


    private bool isSound;
    private bool isMusic;

    private void Awake()
    {
        InitializeSettings();
    }
    private void InitializeSettings()
    {
        highestScoreTxt.text = PlayerPrefs.GetInt("HighestScore", 0).ToString();

        isSound = PlayerPrefs.GetInt("isSound", 1) == 0 ? false : true;
        isMusic = PlayerPrefs.GetInt("isMusic", 1) == 0 ? false : true;

        soundBtnImage.sprite = soundSprites[(isSound ? 1 : 0)];
        musicBtnImage.sprite = musicSprites[(isMusic ? 1 : 0)];
    }

    public void OnClick_PlayBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClick_QuitBtn()
    {
        Application.Quit();
    }

    public void OnClick_SoundBtn()
    {
        isSound = !isSound;
        soundBtnImage.sprite = soundSprites[(isSound ? 1 : 0)];
        PlayerPrefs.SetInt("isSound", (isSound ? 1 : 0));
    }

    public void OnClick_MusicBtn()
    {
        isMusic = !isMusic;
        PlayerPrefs.SetInt("isMusic", (isMusic ? 1 : 0));
        musicBtnImage.sprite = musicSprites[(isMusic ? 1 : 0)];
    }
}
