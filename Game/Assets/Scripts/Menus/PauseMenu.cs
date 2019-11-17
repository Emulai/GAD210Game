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

    public void Resume() {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        manager.IsPaused = false;
    }

    public void Save() {
        miniSavePanel.SetActive(!miniSavePanel.activeSelf);
        saveName.ActivateInputField();
    }

    public void MiniSave() {
        if (!string.IsNullOrWhiteSpace(saveName.text)) {
            PlayerInput input = FindObjectOfType<PlayerInput>();
            TriggerBox[] boxes = FindObjectsOfType<TriggerBox>();
            StandButton[] standButtons = FindObjectsOfType<StandButton>();
            Platform[] platforms = FindObjectsOfType<Platform>().OrderBy(p => p.transform.parent.position.x).ToArray();
            TurretBehaviour[] turrets = FindObjectsOfType<TurretBehaviour>();
            EnemyBullet[] bullets = FindObjectsOfType<EnemyBullet>();

            SaveFormat save = new SaveFormat(
                input,
                boxes,
                standButtons,
                platforms,
                turrets,
                bullets
            );

            string saveJson = JsonUtility.ToJson(save, true);

            string file = Application.persistentDataPath + "/SavedGames/" + saveName.text + ".stdm";

            File.WriteAllText(file, saveJson);

            string path = Application.persistentDataPath + "/SaveImages/" + saveName.text + ".png";
            ScreenCapture.CaptureScreenshot(path);

            miniSavePanel.SetActive(false);
            Resume();
        }
    }

    public void Load() {
        gameObject.SetActive(false);
        loadMenu.gameObject.SetActive(true);
    }

    public void MainMenu() {
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }

    public void GameOverMenu() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameOverText.gameObject.SetActive(true);
        resume.gameObject.SetActive(false);
        save.gameObject.SetActive(false);
    }
}
