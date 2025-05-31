using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Maneja el sistema de di�logos del juego, mostrando frases una por una con efecto de tipeo
// Al finalizar, permite pasar a la siguiente escena
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;    // Texto del di�logo
    public float typingSpeed = 0.05f;        // Velocidad de tipeo por letra
    public string[] sentences;              // Arreglo de frases a mostrar

    private int index = 0;                  // �ndice de la frase actual
    private bool isTyping = false;          // Estado de tipeo en curso

    public GameObject arrowIndicator;       // Flecha que indica que se puede continuar
    public string siguienteEscena;          // Escena a cargar al finalizar el di�logo

    void Start()
    {
        StartCoroutine(TypeSentence(sentences[index])); // Inicia el primer di�logo
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = sentences[index]; // Muestra texto completo instant�neamente
                isTyping = false;
            }
            else
            {
                NextSentence(); // Avanza a la siguiente frase
            }
        }
    }

    // Corrutina que muestra el texto letra por letra
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        arrowIndicator.SetActive(false);
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        arrowIndicator.SetActive(true);
    }

    // Carga la siguiente frase o la escena si es la �ltima
    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(siguienteEscena);
    }
}
