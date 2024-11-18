using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialScreen;
    [SerializeField]
    private TextMeshProUGUI txtTitle;
    [SerializeField]
    private string[] titles;
    private int indexTutorial = 0;
    private float displayTime = 10f; // Tiempo en segundos
    private Animator titleAnimator;
    [SerializeField]
    private Slider loadingBar;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private TextMeshProUGUI continueText;
    [SerializeField]
    private Image imgKeyboard;
    [SerializeField]
    private Image imgMouse;
    [SerializeField] 
    private Image imgGamePad;

    // Start is called before the first frame update
    void Start()
    {
        indexTutorial = 0;
        tutorialScreen.SetActive(true);
        UpdateTutorial();
        titleAnimator = txtTitle.GetComponent<Animator>();

        StartCoroutine(ChangeTutorial());
        StartCoroutine(LoadingProgress());
    }

    private IEnumerator LoadingProgress()
    {
        float totalTime = 30f; // Tiempo total de carga en segundos
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            loadingBar.value = elapsedTime / totalTime;
            yield return null;
        }

        // Mostrar el mensaje de "Presione cualquier tecla para continuar"
        loadingText.gameObject.SetActive(false);
        continueText.gameObject.SetActive(true);

        // Esperar a que el usuario presione cualquier tecla
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Iniciar el juego
        StartGame();
    }

    private void StartGame()
    {
        tutorialScreen.SetActive(false);
        Debug.Log("Pasa a la siguiente escena para iniciar el juego");
        SceneManager.LoadScene("Mapa"); // Cambia "NombreDeLaSiguienteEscena" por el nombre de la escena que deseas cargar
    }

    private void UpdateTutorial()
    {
        //tutorialImage.sprite = images[indexTutorial];
        txtTitle.text = titles[indexTutorial];
        UpdateImages();
        //txtMessage.text = messages[indexTutorial];
        indexTutorial++;
        if (indexTutorial == titles.Length)
            indexTutorial = 0;
    }
    
    private void UpdateImages()
    {
        switch (indexTutorial)
        {
            case 0:
                imgKeyboard.enabled = true;
                imgMouse.enabled = true;
                imgGamePad.enabled = false;
                break;
            case 1:
                imgKeyboard.enabled = false;
                imgMouse.enabled = false;
                imgGamePad.enabled = true;
                break;
        }
    }

    private IEnumerator ChangeTutorial()
    {
        while (true)
        {
            // Fade out
            titleAnimator.Play("FadeOut");
            yield return new WaitForSeconds(1f); // Esperar a que termine la animación de fade out

            UpdateTutorial();

            // Fade in
            titleAnimator.Play("FadeIn");
            yield return new WaitForSeconds(displayTime - 1f); // Esperar el tie
        }
    }


}
