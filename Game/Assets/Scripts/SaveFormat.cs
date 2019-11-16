using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveFormat
{
    public string sceneName = "";

    public bool playerHasGun = false;
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public List<Vector3> boxPositions = new List<Vector3>();
    public List<Quaternion> boxRotations = new List<Quaternion>();

    public List<float> standTimers = new List<float>();
    public List<bool> standActives = new List<bool>();

    public List<Vector3> platformPositions = new List<Vector3>();
    public List<int> platformTarget = new List<int>();

    public SaveFormat(
        PlayerInput input,
        TriggerBox[] boxes,
        StandButton[] standButtons,
        Platform[] platforms) 
    {
        sceneName = SceneManager.GetActiveScene().name;

        playerHasGun = input.HasGun;
        playerPosition = input.gameObject.transform.position;
        playerRotation = input.gameObject.transform.rotation;

        foreach(TriggerBox box in boxes) {
            boxPositions.Add(box.transform.position);
            boxRotations.Add(box.transform.rotation);
        }

        foreach (StandButton standButton in standButtons) {
            standTimers.Add(standButton.Timer);
            standActives.Add(standButton.IsActive);
        }

        foreach (Platform platform in platforms) {
            platformPositions.Add(platform.transform.position);
            platformTarget.Add(platform.TargetIndex);
        }
    }
}
