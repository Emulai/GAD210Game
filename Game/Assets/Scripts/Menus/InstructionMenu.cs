using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas mainMenu = null;
    [SerializeField]
    private List<GameObject> instItems = new List<GameObject>();
    [SerializeField]
    private RawImage instImage = null;
    [SerializeField]
    private List<Texture2D> instImages = new List<Texture2D>();

    private int index = 0;

    void OnEnable() {
        // Setup first info and image
        instItems[index].gameObject.SetActive(true);
        instImage.texture = instImages[index];
    }

    public void Next() {
        // If within bounds, increment index and setup corresponding info and image
        if (index < instItems.Count - 1) {
            index++;

            instItems[index - 1].gameObject.SetActive(false);
            instItems[index].gameObject.SetActive(true);

            instImage.texture = instImages[index];
        }
        // Else turn off instruction screen
        else {
            gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
            instItems[index].gameObject.SetActive(false);

            index = 0;
        }
    }
}
