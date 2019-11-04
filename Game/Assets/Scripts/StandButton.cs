using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StandButton : MonoBehaviour, IInteractive
{
    [SerializeField]
    float openTime = 0.0f;
    [SerializeField]
    private GameObject door = null;
    [SerializeField]
    private GameObject timerScreen = null;
    [SerializeField]
    private Color doorOpen = Color.green;
    [SerializeField]
    private Color doorClosed = Color.red;

    [Header("Debug")]
    [SerializeField]
    private bool debug = false;

    private Animator anim = null;
    private float timer = 0.0f;
    private bool open = false;
    private TMP_Text timerText = null;
    private Image timerPanel = null;
    private Coroutine coroutine = null;

    void Start() {
        anim = door.GetComponent<Animator>();
        Canvas c = timerScreen.GetComponentInChildren<Canvas>();
        timerText = c.GetComponentInChildren<TMP_Text>();
        timerPanel = c.GetComponentInChildren<Image>();
    }

    // Handles countdown timer until door is closed 
    void Update() {
        if (open) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
            }

            timer -= Time.deltaTime;
            timerPanel.color = Color.Lerp(doorClosed, doorOpen, (timer / openTime));

            if (timer <= 0.0f) {
                anim.SetTrigger("Close");
                open = false;
                timer = 0.0f;
                timerPanel.color = doorClosed;
                coroutine = StartCoroutine(ChangeColour());
            }

            timerText.text = (timer * 100.0f).ToString("0:00");

        }

        if (debug) {
            Debug.DrawLine(transform.position, door.gameObject.transform.position, Color.green);
            Debug.DrawLine(transform.position, timerScreen.gameObject.transform.position, Color.green);
        }
    }

    public void Activate(PlayerController activator) {
        anim.SetTrigger("Open");
        timer = openTime;
        open = true;
        timerPanel.color = doorOpen;
    }

    public string Info() {
        return "Press button";
    }

    private IEnumerator ChangeColour() {
        yield return new WaitForSeconds(1.0f);

        timerText.text = "";
        timerPanel.color = Color.black;
    }
}
