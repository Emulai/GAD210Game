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

    void Start() {
        instItems[index].gameObject.SetActive(true);
    }

    public void Next() {
        if (index < instItems.Count - 1) {
            index++;

            instItems[index - 1].gameObject.SetActive(false);
            instItems[index].gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }
}
