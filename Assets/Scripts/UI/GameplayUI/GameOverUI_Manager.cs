using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the Game Over UI.
/// </summary>

[AddComponentMenu("Scripts/ESI/UI/Game Over UI Manager")]
public class GameOverUI_Manager : MonoBehaviour {

    //public static GameOverUI_Manager instance { get; private set; }

    [field: Header("Script properties")]
    [field: SerializeField] private GameObject gameOverUI { get; set; }
    [field: SerializeField] private GameObject[] objectsToDisable { get; set; }

    /*private void Awake() {
        if (instance == null)
            instance = this;
    }*/

    // Start is called before the first frame update
    void Start() {
        gameOverUI.SetActive(false);
    }

    public void ShowGameOver() {
        Time.timeScale = 0;
        PauseManager.SetOnPause(true);
        UIUtility.EnableDisableObjects(false, objectsToDisable);
        gameOverUI.SetActive(true);
    }
}
