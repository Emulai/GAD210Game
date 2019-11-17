using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseMenu = null;
    [SerializeField]
    private Canvas loadMenu = null;
    private bool isPaused = false;

    void Awake() {
        isPaused = false;
    }

    void Start() {
        isPaused = false;
    }

    void Update()
    {
        // If player hits 'Escape'
        if (Input.GetButtonDown("Pause")) {
            // Pause if unpaused & show menu
            if (!isPaused) {
                Time.timeScale = 0.0f;
                pauseMenu.gameObject.SetActive(true);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                isPaused = true;
            }
            // Unpause if paused & hide menu
            else {
                Time.timeScale = 1.0f;
                pauseMenu.gameObject.SetActive(false);
                loadMenu.gameObject.SetActive(false);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                isPaused = false;
            }
        }
    }

    public bool IsPaused {
        get { return isPaused; }
        set { isPaused = value; }
    }
}
