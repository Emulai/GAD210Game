using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField]
    private string nextScene = "";
    [SerializeField]
    private bool end = false;

    private GameManager manager = null;

    void Start() {
        manager = FindObjectOfType<GameManager>();
        manager.IsRunning = true;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // If an end-game level switcher, victory!
            if (end) {
                manager.GameEnd();
            }
            // Else load a new scene
            else {
                other.gameObject.GetComponent<PlayerInput>().IsControllable = false;
                SceneManager.LoadSceneAsync(nextScene);
            }
        }
    }

    public bool TheEnd {
        set { end = value; }
    }
}
