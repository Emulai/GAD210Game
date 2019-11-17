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
    public float playerHealth;

    public List<Vector3> boxPositions = new List<Vector3>();
    public List<Quaternion> boxRotations = new List<Quaternion>();

    public List<float> standTimers = new List<float>();
    public List<bool> standActives = new List<bool>();

    // public List<Vector3> platformPositions = new List<Vector3>();
    // public List<int> platformTarget = new List<int>();
    public Vector3[] platformPositions = null;
    public Vector3[] platformChildPositions = null;
    public int[] platformTarget = null;

    public List<bool> turretActives = new List<bool>();
    public List<Vector3> turretPositions = new List<Vector3>();
    public List<Quaternion> turretRotations = new List<Quaternion>();

    public List<float> bulletTTL = new List<float>();
    public List<Vector3> bulletPositions = new List<Vector3>();
    public List<Vector3> bulletVelocities = new List<Vector3>();

    public SaveFormat(
        PlayerInput input,
        TriggerBox[] boxes,
        StandButton[] standButtons,
        Platform[] platforms,
        TurretBehaviour[] turrets,
        EnemyBullet[] bullets)
    {
        sceneName = SceneManager.GetActiveScene().name;

        playerHasGun = input.HasGun;
        playerPosition = input.gameObject.transform.position;
        playerRotation = input.gameObject.transform.rotation;
        playerHealth = input.gameObject.GetComponent<PlayerController>().CurrentHealth;

        foreach(TriggerBox box in boxes) {
            boxPositions.Add(box.transform.position);
            boxRotations.Add(box.transform.rotation);
        }

        foreach (StandButton standButton in standButtons) {
            standTimers.Add(standButton.Timer);
            standActives.Add(standButton.IsActive);
        }

        platformPositions = new Vector3[platforms.Length];
        platformChildPositions = new Vector3[platforms.Length];
        platformTarget = new int[platforms.Length];
        for (int index = 0; index < platforms.Length; index++) {
            platformPositions[index] = platforms[index].transform.position;
            platformChildPositions[index] = platforms[index].gameObject.GetComponentInChildren<BoxCollider>().transform.position;
            platformTarget[index] = platforms[index].TargetIndex;
        }

        foreach (TurretBehaviour turret in turrets) {
            turretActives.Add(turret.IsActive);
            turretPositions.Add(turret.transform.position);
            turretRotations.Add(turret.transform.rotation);
        }

        foreach (EnemyBullet bullet in bullets) {
            bulletTTL.Add(bullet.TimeToLive);
            bulletPositions.Add(bullet.transform.position);
            bulletVelocities.Add(bullet.gameObject.GetComponent<Rigidbody>().velocity);
        }
    }
}
