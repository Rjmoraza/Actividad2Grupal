using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialScreen;
    [SerializeField]
    private TextMeshProUGUI txtTitle;
    //[SerializeField]
    //private TextMeshProUGUI txtMessage;
    //[SerializeField]
    //private Image tutorialImage; // Referencia a la imagen en el Canvas
    //[SerializeField]
    //private Sprite[] images;
    [SerializeField]
    private string[] titles;
    //[SerializeField]
    //private string[] messages;
    //[SerializeField]
    //private int maxScreens;
    private int indexTutorial = 0;
    private float displayTime = 10f; // Tiempo en segundos
    private Animator titleAnimator;
    //private Animator messageAnimator;
    [SerializeField]
    private Slider loadingBar;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private TextMeshProUGUI continueText;
    //private bool isTutorialComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        indexTutorial = 0;
        tutorialScreen.SetActive(true);
        UpdateTutorial();
        titleAnimator = txtTitle.GetComponent<Animator>();
        //messageAnimator = txtMessage.GetComponent<Animator>();
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
        Debug.Log("Pasa a la siguiente escena para iniciar el juego");
    }

    private void UpdateTutorial()
    {
        //tutorialImage.sprite = images[indexTutorial];
        txtTitle.text = titles[indexTutorial];
        //txtMessage.text = messages[indexTutorial];
        indexTutorial++;
        if (indexTutorial == titles.Length)
            indexTutorial = 0;
    }

    private IEnumerator ChangeTutorial()
    {
        while (true)
        {
            // Fade out
            titleAnimator.Play("FadeOut");
            //messageAnimator.Play("FadeOut");
            yield return new WaitForSeconds(1f); // Esperar a que termine la animación de fade out

            UpdateTutorial();

            // Fade in
            titleAnimator.Play("FadeIn");
            //messageAnimator.Play("FadeIn");
            yield return new WaitForSeconds(displayTime - 1f); // Esperar el tie
        }
    }


    // Ocultar la pantalla de tutorial después de mostrar todas las imágenes
    //tutorialScreen.SetActive(false);
    // Aquí puedes iniciar el juego
    //StartGame();

}
