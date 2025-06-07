using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    public RectTransform creditsText;
    public float scrollSpeed = 50f;
    public float endPositionY = 1200f;
    public float delayAfterEnd = 3f;
    public string sceneToLoad;

    private bool finished = false;

    void Update()
    {
        if (!finished)
        {
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsText.anchoredPosition.y >= endPositionY)
            {
                finished = true;
                Invoke(nameof(LoadNextScene), delayAfterEnd);
            }
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
