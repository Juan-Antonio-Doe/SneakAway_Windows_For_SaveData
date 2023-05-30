using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility for common functionalities in UI.
/// </summary>

public class UIUtility {

    /// <summary>
    /// Enable or disable a list of GameObjects.
    /// </summary>
    public static void EnableDisableObjects(bool condition, GameObject[] objectsToDisable) {
        foreach (GameObject obj in objectsToDisable) {
            obj.SetActive(condition);
        }
    }

}
