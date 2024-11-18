using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        player.isPaused = false;
    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }


    public void ExitToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
