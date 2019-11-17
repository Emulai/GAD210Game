using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas mainMenu = null;
    [SerializeField]
    private ScrollRect loadList = null;
    [SerializeField]
    private RawImage loadImage = null;
    [SerializeField]
    private TMP_Text loadDetails = null;
    [SerializeField]
    private Button fileButton = null;

    private DirectoryInfo info = null;
    private string file = "";
    private SaveFormat fileToLoad = null;
    private GameManager manager = null;

    // Start is called before the first frame update
    void Start()
    {
        info = new DirectoryInfo(Application.persistentDataPath + "/SavedGames");

        manager = FindObjectOfType<GameManager>();
        manager.IsRunning = true;

        LoadFiles();
    }

    private void LoadFiles() {
        foreach (Transform child in loadList.content.transform) {
            Destroy(child.gameObject);
        }

        if (info != null) {
            FileInfo[] fileInfo = info.GetFiles("*.stdm");
            foreach (FileInfo file in fileInfo) {
                Button button = Instantiate(fileButton, loadList.content);
                button.GetComponentInChildren<TMP_Text>().text =  file.Name.Split('.')[0];

                button.onClick.AddListener(delegate { 
                    LoadFileDetails(file.Name.Split('.')[0]); 
                });
            }
        }
    }

    private void LoadFileDetails(string filename) {
        file = Application.persistentDataPath + "/SavedGames/" + filename + ".stdm";
        string image = Application.persistentDataPath + "/SaveImages/" + filename + ".png";

        string loadJson = File.ReadAllText(file);

        if (fileToLoad != null) {
            fileToLoad = null;
        }
        fileToLoad = JsonUtility.FromJson<SaveFormat>(loadJson);

        FileInfo[] fileInfo = info.GetFiles(filename + ".stdm");

        loadDetails.text = "Scene: " + fileToLoad.sceneName;
        loadDetails.text += "\nLast Played: " + fileInfo[0].LastWriteTime;

        byte[] bytes = File.ReadAllBytes(image);
        Texture2D tex = new Texture2D(200, 200);
        tex.filterMode = FilterMode.Trilinear;
        tex.LoadImage(bytes);

        loadImage.texture = tex;
    }

    public void LoadGame() {
        manager.LoadGameLevel(fileToLoad);
    }

    public void DeleteGame() {
        File.Delete(file);

        LoadFiles();
    }

    public void BackToMain() {
        gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    void OnEnable() {
        LoadFiles();
    }
}
