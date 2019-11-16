using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string tester;
    private static GameManager instance;   
    private SaveFormat fileToLoad;
    private Coroutine co;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void LoadGameLevel(SaveFormat file) {
        fileToLoad = file;

        StartCoroutine(LoadGameLevelCoroutine());
    }

    private IEnumerator LoadGameLevelCoroutine() {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("Scenes/" + fileToLoad.sceneName);

        while (!sceneLoad.isDone) {
            yield return null;
        }

        PlayerInput input = FindObjectOfType<PlayerInput>();
        input.HasGun = fileToLoad.playerHasGun;
        input.gameObject.GetComponent<Rigidbody>().MovePosition(fileToLoad.playerPosition);
        input.transform.rotation = fileToLoad.playerRotation;

        TriggerBox[] boxes = FindObjectsOfType<TriggerBox>();
        for (int index = 0; index < fileToLoad.boxPositions.Count; index++) {
            boxes[index].transform.position = fileToLoad.boxPositions[index];
            boxes[index].transform.rotation = fileToLoad.boxRotations[index];
        }

        StandButton[] standButtons = FindObjectsOfType<StandButton>();
        for (int index = 0; index < fileToLoad.standTimers.Count; index++) {
            standButtons[index].Timer = fileToLoad.standTimers[index];
            standButtons[index].IsActive = fileToLoad.standActives[index];
        }

        Platform[] platforms = FindObjectsOfType<Platform>();
        for (int index = 0; index < fileToLoad.platformPositions.Count; index++) {
            platforms[index].transform.position = fileToLoad.platformPositions[index];
            platforms[index].pathIndex = fileToLoad.platformTarget[index];
        }
    }

    public static GameManager Instance {
        get { return instance; }
    }
}
