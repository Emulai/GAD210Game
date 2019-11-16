using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private List<Switch> switches = new List<Switch>();

    [Header("Debug")]
    [SerializeField]
    private bool debug = false;

    private Animator anim = null;
    public List<bool> truths = new List<bool>();

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        truths.Clear();

        foreach (Switch s in switches) {

            truths.Add(s.IsActive);

            if (debug) {
                Debug.DrawLine(transform.position, s.gameObject.transform.position, Color.green);
            }
        }

        anim.SetBool("Open", !truths.Contains(false));
    }
}
