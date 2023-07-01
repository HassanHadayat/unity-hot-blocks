using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPage : MonoBehaviour
{
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public float loadingDelay = 2f;

    private void Start()
    {
        StartCoroutine(LoadComplete());
        InvokeRepeating("UpdateLoadingBarText", 0f, 0.5f);
    }

    private void UpdateLoadingBarText()
    {
        if (loadingText.text == "Loading.") loadingText.text = "Loading..";
        else if (loadingText.text == "Loading..") loadingText.text = "Loading...";
        else if (loadingText.text == "Loading...") loadingText.text = "Loading.";
    }
    private IEnumerator LoadComplete()
    {
        yield return new WaitForSeconds(loadingDelay);

        float elapsedTime = 0f;
        float totalLoadingTime = 3f; // Total time for the loading bar to complete

        while (elapsedTime < totalLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / totalLoadingTime);
            loadingBar.value = progress;

            yield return null;
        }

        //yield return new WaitForSeconds(2f);

        loadingText.text = "Loaded";
        SceneManager.LoadScene("MainMenuScene");
        //Destroy(gameObject);
    }
}
