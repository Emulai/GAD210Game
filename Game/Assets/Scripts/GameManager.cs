using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyBullet bullet = null;
    private static GameManager instance;   
    private SaveFormat fileToLoad;
    private Coroutine co;
    private bool gameRunning = true;

    void Awake() {
        // Singleton it
        if (instance == null) {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Start loading from file
    public void LoadGameLevel(SaveFormat file) {
        fileToLoad = file;
        gameRunning = true;

        StartCoroutine(LoadGameLevelCoroutine());
    }

    // Load from file
    private IEnumerator LoadGameLevelCoroutine() {
        // Grab scene name from file & load it
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("Scenes/" + fileToLoad.sceneName);

        // Wait till done loading
        while (!sceneLoad.isDone) {
            yield return null;
        }

        // Grab player info from file and set to scene's instance of player
        PlayerInput input = FindObjectOfType<PlayerInput>();
        PlayerController con = input.GetComponent<PlayerController>();
        input.HasGun = fileToLoad.playerHasGun;
        input.gameObject.GetComponent<Rigidbody>().MovePosition(fileToLoad.playerPosition);
        con.RotateX = fileToLoad.playerRotation.x;
        con.RotateY = fileToLoad.playerRotation.y;

        // Update all trigger boxes with corresponding info from file -- note that none of this is guaranteed to be in same order, but shouldn't matter much for most objects
        TriggerBox[] boxes = FindObjectsOfType<TriggerBox>();
        for (int index = 0; index < fileToLoad.boxPositions.Count; index++) {
            boxes[index].transform.position = fileToLoad.boxPositions[index];
            boxes[index].transform.rotation = fileToLoad.boxRotations[index];
        }

        // Update all stand buttons with corresponding info from file
        StandButton[] standButtons = FindObjectsOfType<StandButton>();
        for (int index = 0; index < fileToLoad.standTimers.Count; index++) {
            standButtons[index].Timer = fileToLoad.standTimers[index];
            standButtons[index].IsActive = fileToLoad.standActives[index];
        }

        // Update platforms with corresponding info from file
        Platform[] platforms = FindObjectsOfType<Platform>().OrderBy(p => p.transform.parent.position.x).ToArray();
        for (int index = 0; index < fileToLoad.platformPositions.Length; index++) {
            platforms[index].transform.position = fileToLoad.platformPositions[index];
            platforms[index].gameObject.GetComponentInChildren<BoxCollider>().transform.position = fileToLoad.platformChildPositions[index];
            platforms[index].pathIndex = fileToLoad.platformTarget[index];
        }

        // Update turrets with corresponding info from file
        TurretBehaviour[] turrets = FindObjectsOfType<TurretBehaviour>();
        for (int index = 0; index < fileToLoad.turretRotations.Count; index++) {
            turrets[index].IsActive = fileToLoad.turretActives[index];
            turrets[index].transform.position = fileToLoad.turretPositions[index];
            turrets[index].transform.rotation = fileToLoad.turretRotations[index];
        }

        // Delete all bullets currently in scene (in case of loading from active scene)
        EnemyBullet[] bullets = FindObjectsOfType<EnemyBullet>();
        foreach (EnemyBullet bullet in bullets) {
            Destroy(bullet);
        }

        // Create new bullets to match corresponding info from file
        for (int index = 0; index < fileToLoad.bulletPositions.Count; index++) {
            EnemyBullet b = Instantiate(bullet, fileToLoad.bulletPositions[index], Quaternion.identity);
            b.gameObject.GetComponent<Rigidbody>().velocity = fileToLoad.bulletVelocities[index];
            b.TimeToLive = fileToLoad.bulletTTL[index];
        }
    }

    // Call pause menu's game over screen
    public void GameOver() {
        if (gameRunning) {
            gameRunning = false;
            Time.timeScale = 0.0f;
            Object[] oarr = Resources.FindObjectsOfTypeAll(typeof(PauseMenu));
            PauseMenu pm = (PauseMenu)oarr[0];
            pm.gameObject.SetActive(true);
            pm.GameOverMenu();
        }
    }

    // Call pause menu's game victory screen
    public void GameEnd() {
        if (gameRunning) {
            gameRunning = false;
            Time.timeScale = 0.0f;
            Object[] oarr = Resources.FindObjectsOfTypeAll(typeof(PauseMenu));
            PauseMenu pm = (PauseMenu)oarr[0];
            pm.gameObject.SetActive(true);
            pm.GameEndMenu();
        }
    }

    public static GameManager Instance {
        get { return instance; }
    }

    public bool IsRunning {
        set { gameRunning = value; }
    }
}
