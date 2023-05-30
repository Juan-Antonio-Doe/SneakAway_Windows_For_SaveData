using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage the conditions for complete the level.
/// </summary>

[AddComponentMenu("Scripts/ESI/LevelManagement/Game Level Manager")]
public class GameLevelManager : MonoBehaviour {

    [field: Header("Complete level properties")]
    [field: SerializeField] private int _maxDocuments { get; set; } = 3;
    [field: SerializeField, ReadOnlyField] private int _documents { get; set; }
    public int Documents { 
        get { return _documents; }
        set {
            if (value < _maxDocuments) {
                _documents = value;
            }
            else if (value >= _maxDocuments) {
                _documents = _maxDocuments;
                ShowExitPoint();
            }
            DocumentTextUpdate();
        }
    }
    [field: SerializeField, Tooltip("Document Text field")] private Text documentsText { get; set; }

    [field: SerializeField, Tooltip("GameObject that is the exit point")] private GameObject exitPoint { get; set; }

    [field: Header("UI properties")]
    [field: SerializeField] private GameObject completedLevelUI { get; set; }
    [field: SerializeField] private Text scoreText { get; set; }
    [field: SerializeField] private Text timeText { get; set; }
    [field: SerializeField] private Text defeatedEnemiesText { get; set; }
    [field: SerializeField] private GameObject[] objectsToDisable { get; set; }

    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando están en una escena
            if (documentsText == null) {
                documentsText = GameObject.Find("DocumentsText").GetComponent<Text>();
            }
            if (exitPoint == null) {
                exitPoint = GameObject.Find("ExitPoint");
            }
            if (completedLevelUI == null) {
                completedLevelUI = GameObject.Find("CompletedLevelCanvas");
                if (scoreText == null)
                    scoreText = GameObject.Find("LevelScoreText").GetComponent<Text>();
                if (timeText == null)
                    timeText = GameObject.Find("TimeValueStat").GetComponent<Text>();
                if (defeatedEnemiesText == null)
                    defeatedEnemiesText = GameObject.Find("EnemyValueStat").GetComponent<Text>();
            }
        }
#endif
    }

    private IEnumerator Start() {
        exitPoint.SetActive(false);
        completedLevelUI.SetActive(false);
        _maxDocuments = GameObject.FindGameObjectsWithTag("Document").Length;
        DocumentTextUpdate();
        yield return null;
    }

    private void ShowExitPoint() {
        exitPoint.SetActive(true);
    }

    public void IncreaseDocuments() {
        Documents++;
    }

    public void ShowCompletedLevelUI() {
        completedLevelUI.SetActive(true);
        scoreText.text = string.Format("{0:D10}", ScoreManager.Score);  // 10 digits.
        timeText.text = ScoreManager.TimeFormatted;
        defeatedEnemiesText.text = ScoreManager.EnemiesDefeated.ToString();
        UIUtility.EnableDisableObjects(false, objectsToDisable);
        PauseManager.SetOnPause(true);

        SaveData();
    }

    public void LoadNextLevel() {
        LoadScene.LoadNextLevel();
    }

    void DocumentTextUpdate() {
        documentsText.text = string.Format("{0}/{1} ", _documents, _maxDocuments);
    }

    void SaveData() {
        //PlayerPrefs.SetString($"LastLevel", LoadScene.CurrentNameScene());

        string sceneName = LoadScene.CurrentNameScene();
        int number = 0;
        string[] parts = sceneName.Split('_');

        // Si el indice de la escena actual es menor que el ndice desde el que empiezan los niveles numerados...
        if (LoadScene.CurrentIndexScene() >= 3)
            number = int.Parse(parts[parts.Length - 1]);
        else
            return;

        /*try {
            number = int.Parse(parts[parts.Length - 1]);
        }
        catch (Exception) {
            Debug.LogError($"Error al extraer el nmero de la escena actual: {sceneName}");
            return;
        }*/

        /*Debug.Log($"Escena actual: {sceneName} \n" +
                       $"Nmero de la escena actual: {number}");*/

        // Si el nmero de la escena actual es mayor al que se tiene guardado, se actualiza
        if (number >= PlayerPrefs.GetInt("UnlokedLeves", 0))
            PlayerPrefs.SetInt("UnlokedLeves", number + 1);

        /*Debug.Log($"UnlockedLevels: {PlayerPrefs.GetInt("UnlokedLeves", 0)} \n" +
            $"Number: {number}");*/
    }

}
