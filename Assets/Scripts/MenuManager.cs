using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private string gameSceneName = "Mapa";

   public void OnStartButtonPress()
    {
        // Load Game scene
       SceneManager.LoadScene(gameSceneName);
       Debug.Log("Start button pressed");
    }

    public void OnExitButtonPress()
    {
        // Exit game
        Application.Quit();
        // if we are in the editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Debug.Log("Exit button pressed");
    }
}
