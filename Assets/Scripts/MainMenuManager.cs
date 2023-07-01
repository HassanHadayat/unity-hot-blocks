using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI highestScoreTxt;

    public GameObject[] sound;
    public GameObject[] music;


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
        SceneManager.LoadScene("GameScene");
    }

    public void OnClick_QuitBtn()
    {
        Application.Quit();
    }

    public void OnClick_SoundBtn()
    {
        isSound = !isSound;
        PlayerPrefs.SetInt("isSound", (isSound ? 1 : 0));
    }

    public void OnClick_MusicBtn()
    {
        isMusic = !isMusic;
        PlayerPrefs.SetInt("isMusic", (isMusic ? 1 : 0));
    }
}
