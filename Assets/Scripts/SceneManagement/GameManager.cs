using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;


/**
 * Purpose:
 * Manage the game.
 * - FrameRate.
 */

[AddComponentMenu("Scripts/ESI/LevelManagement/Game Manager")]
public class GameManager : MonoBehaviour {

    private void Awake() {
        // For hide a Unity's bug.
        //DebugManager.instance.enableRuntimeUI = false;
    }

    void Start() {
        Application.targetFrameRate = 60;
        //NotificationManager.NotificationCreation();
    }

}
