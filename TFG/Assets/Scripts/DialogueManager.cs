using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public string[] sentences;

    private int index = 0;
    private bool isTyping = false;

    public GameObject arrowIndicator;
    public string siguienteEscena;

    void Start()
    {
        StartCoroutine(TypeSentence(sentences[index]));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Mostrar texto completo instantáneamente
                StopAllCoroutines();
                dialogueText.text = sentences[index];
                isTyping = false;
            }
            else
            {
                NextSentence();
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        arrowIndicator.SetActive(false); // Ocultar flecha mientras escribe
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        arrowIndicator.SetActive(true); // Mostrar flecha al terminar
    }

    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            // Aquí cambia la escena
            LoadNextScene();
        }
    }
    void LoadNextScene()
    {
        SceneManager.LoadScene(siguienteEscena);
    }

}
