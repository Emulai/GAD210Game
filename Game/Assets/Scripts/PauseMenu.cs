using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager manager = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume() {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        manager.IsPaused = false;
    }

    public void Save() {

    }

    public void Load() {
        
    }

    public void MainMenu() {
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }
}
