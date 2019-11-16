using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
            Platform[] platforms = FindObjectsOfType<Platform>();

            SaveFormat save = new SaveFormat(
                input,
                boxes,
                standButtons,
                platforms
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
}
