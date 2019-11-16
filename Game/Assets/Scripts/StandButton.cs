using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StandButton : Switch, IInteractive
{
    [SerializeField]
    float openTime = 0.0f;
    [SerializeField]
    private GameObject timerScreen = null;
    [SerializeField]
    private Color doorOpen = Color.green;
    [SerializeField]
    private Color doorClosed = Color.red;

    private float timer = 0.0f;
    private TMP_Text timerText = null;
    private Image timerPanel = null;
    private Coroutine coroutine = null;

    void Start() {
        Canvas c = timerScreen.GetComponentInChildren<Canvas>();
        timerText = c.GetComponentInChildren<TMP_Text>();
        timerPanel = c.GetComponentInChildren<Image>();
    }

    // Handles countdown timer until door is closed 
    void Update() {
        if (isActive) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
            }

            timer -= Time.deltaTime;
            timerPanel.color = Color.Lerp(doorClosed, doorOpen, (timer / openTime));

            if (timer <= 0.0f) {
                isActive = false;
                timer = 0.0f;
                timerPanel.color = doorClosed;
                coroutine = StartCoroutine(ChangeColour());
            }

            timerText.text = (timer * 100.0f).ToString("0:00");

        }
    }

    public void Activate(PlayerController activator) {
        isActive = true;
        timer = openTime;
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

    public float Timer {
        get { return timer; }
        set { timer = value; }
    }
}
