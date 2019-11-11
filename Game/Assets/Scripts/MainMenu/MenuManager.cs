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
    [Header("Load Menu")]
    [SerializeField]
    private Canvas loadMenu = null;
    [SerializeField]
    private ScrollRect loadList = null;
    [SerializeField]
    private RawImage loadImage = null;
    [SerializeField]
    private TMP_Text loadDetails = null;
    [SerializeField]
    private Button fileButton = null;

    private DirectoryInfo info = null;

    // Start is called before the first frame update
    void Start()
    {
        info = new DirectoryInfo(Application.persistentDataPath + "/SavedGames");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame() {
        SceneManager.LoadSceneAsync("Scenes/FirstLevel");
    }

    public void LoadGame() {
        mainMenu.gameObject.SetActive(false);
        loadMenu.gameObject.SetActive(true);

        FileInfo[] fileInfo = info.GetFiles("*.stdm");
        foreach (FileInfo file in fileInfo) {
            Button button = Instantiate(fileButton, loadList.content);
            button.GetComponentInChildren<TMP_Text>().text =  file.Name.Split('.')[0];

            Debug.Log(file.Name + " : " + file.CreationTime + " : " + file.LastWriteTime);
        }
    }

    public void BackToMain() {
        loadMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
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
