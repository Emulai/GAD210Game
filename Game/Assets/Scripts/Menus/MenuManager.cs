using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Load Menu")]
    [SerializeField]
    private Canvas loadMenu = null;

    [Header("Instructions")]
    [SerializeField]
    private Canvas instMenu = null;

    private GameManager manager = null;

    void Start() {
        manager = FindObjectOfType<GameManager>();
        manager.IsRunning = true;
    }

    // Load the first level new
    public void NewGame() {
        SceneManager.LoadSceneAsync("Scenes/FirstLevel");
    }

    // Open load game menu
    public void Load() {
        gameObject.SetActive(false);
        loadMenu.gameObject.SetActive(true);
    }

    // Start Tech-Art project
    public void RandomMode() {
        SceneManager.LoadSceneAsync("Scenes/Random");
    }

    // Load the dev level
    public void Sandbox() {
        SceneManager.LoadSceneAsync("Scenes/Sandbox");
    }

    // Load the instructions screen
    public void Instructions() {
        gameObject.SetActive(false);
        instMenu.gameObject.SetActive(true);
    }

    // Quit the game (using pre-compiler codes to determine method)
    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
