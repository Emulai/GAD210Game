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
        // Clear out all existing file buttons
        foreach (Transform child in loadList.content.transform) {
            Destroy(child.gameObject);
        }

        // Get all files with save extension .stdm (STanDalone Mentos) and create a file button for each as child of scrollview
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

    // When a file button is clicked, load details
    private void LoadFileDetails(string filename) {
        // Load file and associated image
        file = Application.persistentDataPath + "/SavedGames/" + filename + ".stdm";
        string image = Application.persistentDataPath + "/SaveImages/" + filename + ".png";

        // Read as string var
        string loadJson = File.ReadAllText(file);

        if (fileToLoad != null) {
            fileToLoad = null;
        }

        // Convert into the SaveFormat using JsonUtility
        fileToLoad = JsonUtility.FromJson<SaveFormat>(loadJson);

        // Get file info
        FileInfo[] fileInfo = info.GetFiles(filename + ".stdm");

        // Display file info
        loadDetails.text = "Scene: " + fileToLoad.sceneName;
        loadDetails.text += "\nLast Played: " + fileInfo[0].LastWriteTime;

        // Load associated save image and display
        byte[] bytes = File.ReadAllBytes(image);
        Texture2D tex = new Texture2D(200, 200);
        tex.filterMode = FilterMode.Trilinear;
        tex.LoadImage(bytes);

        loadImage.texture = tex;
    }

    // Use the GameManager (singleton) to load the file
    public void LoadGame() {
        manager.LoadGameLevel(fileToLoad);
    }

    // Delete the save file (associated image not deleted, some are funny)
    public void DeleteGame() {
        File.Delete(file);

        LoadFiles();
    }

    // Return to main menu
    public void BackToMain() {
        gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    // When made active refresh file list
    void OnEnable() {
        LoadFiles();
    }
}
