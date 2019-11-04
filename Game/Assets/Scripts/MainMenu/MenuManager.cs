using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Canvas mainMenu = null;
    [SerializeField]
    private Canvas loadMenu = null;

    private ScrollRect loadList = null;
    private RawImage loadImage = null;
    private TMP_Text loadText = null;

    // Start is called before the first frame update
    void Start()
    {
        loadList = loadMenu.GetComponentInChildren<ScrollRect>();
        loadImage = loadMenu.GetComponentInChildren<RawImage>();
        loadText = loadMenu.GetComponentInChildren<TMP_Text>();

        loadList.gameObject.SetActive(false);
        loadImage.gameObject.SetActive(false);
        loadText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame() {
        Debug.Log("Start New Game");
    }

    public void LoadGame() {
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/SavedGames");
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo) {
            Debug.Log(file.Name + " : " + file.CreationTime + " : " + file.LastWriteTime);
        }
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
