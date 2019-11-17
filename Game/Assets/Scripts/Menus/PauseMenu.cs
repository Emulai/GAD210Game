using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private PauseManager manager = null;
    [SerializeField]
    private GameObject miniSavePanel = null;
    [SerializeField]
    private TMP_InputField saveName = null;
    [SerializeField]
    private Canvas loadMenu = null;

    [SerializeField]
    private TMP_Text gameOverText = null;
    [SerializeField]
    private Button resume = null;
    [SerializeField]
    private Button save = null;

    // Unpause game when Resume button pressed
    public void Resume() {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        manager.IsPaused = false;
    }

    // Open the MiniSave panel
    public void Save() {
        miniSavePanel.SetActive(!miniSavePanel.activeSelf);
        saveName.ActivateInputField();
    }

    // Save the game
    public void MiniSave() {
        // Ensure string name is somewhat valid
        if (!string.IsNullOrWhiteSpace(saveName.text)) {
            // Get all relevant scene data
            PlayerInput input = FindObjectOfType<PlayerInput>();
            TriggerBox[] boxes = FindObjectsOfType<TriggerBox>();
            StandButton[] standButtons = FindObjectsOfType<StandButton>();
            Platform[] platforms = FindObjectsOfType<Platform>().OrderBy(p => p.transform.parent.position.x).ToArray();
            TurretBehaviour[] turrets = FindObjectsOfType<TurretBehaviour>();
            EnemyBullet[] bullets = FindObjectsOfType<EnemyBullet>();

            // Send scene data to SaveFormat class
            SaveFormat save = new SaveFormat(
                input,
                boxes,
                standButtons,
                platforms,
                turrets,
                bullets
            );

            // Serialise to JSON
            string saveJson = JsonUtility.ToJson(save, true);

            string file = Application.persistentDataPath + "/SavedGames/" + saveName.text + ".stdm";

            // Write to file
            File.WriteAllText(file, saveJson);

            // Capture screenshot
            string path = Application.persistentDataPath + "/SaveImages/" + saveName.text + ".png";
            ScreenCapture.CaptureScreenshot(path);

            // Close panel and resume game
            miniSavePanel.SetActive(false);
            Resume();
        }
    }

    // Open the load menu
    public void Load() {
        gameObject.SetActive(false);
        loadMenu.gameObject.SetActive(true);
    }

    // Return to main menu
    public void MainMenu() {
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }

    // Display the game over menu
    public void GameOverMenu() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameOverText.gameObject.SetActive(true);
        resume.gameObject.SetActive(false);
        save.gameObject.SetActive(false);
    }

    // Display the game victory menu
    public void GameEndMenu() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Victory!";
        resume.gameObject.SetActive(false);
        save.gameObject.SetActive(false);
    }
}
