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

    private GameManager manager = null;

    void Start() {
        manager = FindObjectOfType<GameManager>();
        manager.IsRunning = true;
    }

    public void NewGame() {
        SceneManager.LoadSceneAsync("Scenes/FirstLevel");
    }

    public void Load() {
        gameObject.SetActive(false);
        loadMenu.gameObject.SetActive(true);
    }

    public void RandomMode() {
        // Start Tech-Art project
        Debug.Log("RandomMode");
    }

    public void Sandbox() {
        SceneManager.LoadSceneAsync("Scenes/Sandbox");
    }

    public void Instructions() {
        Debug.Log("Load Instructions");
    }

    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
