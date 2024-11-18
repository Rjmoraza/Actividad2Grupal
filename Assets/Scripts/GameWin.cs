using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    [SerializeField]
    [TextArea(5, 30)]
    private string winStoryText;
    [SerializeField] private TMP_Text textPanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private GameObject exitButton;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WriteStory());
    }


    public void OnExitButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }


    IEnumerator WriteStory()
    {
        // random time between 0.1 and 1
        foreach (char letter in winStoryText.ToCharArray())
        {
            // Add the letter to the text
            // Wait a certain amount of time
            // Repeat
            textPanel.text += letter;
            // random pitch between 0.8 and 1.2
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(typingSound);
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        }
        exitButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(exitButton);

    }
}
