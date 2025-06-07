using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coninuara : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float typingSpeed = 1f;
    public string fullText;
    public float delayAfterText = 2f; 
    public string nextSceneName;
    private void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(delayAfterText);
        SceneManager.LoadScene(nextSceneName);
    }
}
