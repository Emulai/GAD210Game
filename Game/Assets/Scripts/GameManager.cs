using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseMenu = null;
    
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause")) {
            if (!isPaused) {
                Time.timeScale = 0.0f;
                pauseMenu.gameObject.SetActive(true);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                isPaused = true;
            }
            else {
                Time.timeScale = 1.0f;
                pauseMenu.gameObject.SetActive(false);

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
