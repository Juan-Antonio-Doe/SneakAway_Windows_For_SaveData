using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage the score of a level.
/// </summary>

[AddComponentMenu("Scripts/ESI/LevelManagement/Score Manager")]
public class ScoreManager : MonoBehaviour {

    [field: Header("Score properties")]
    [field: SerializeField] private int _maxScore { get; set; } = 1000000;   // 1 millón por nivel.
    [field: SerializeField] private float _maxTime { get; set; } = 300f;    // 5 minutos por nivel.
    private string mins { get; set; }
    private string seconds { get; set; }
    [field: SerializeField] private Text timerText { get; set; }
    //[field: SerializeField] private int MaxObjectives { get; set; }
    [field: SerializeField, ReadOnlyField] private int _maxEnemies { get; set; }

    // Pesos en la puntuación
    [field: SerializeField, Range(0, 100)] private int _timeWeight { get; set; } = 80;
    //[field: SerializeField] private int ObjectivesWeight { get; set; }
    [field: SerializeField, Range(0, 100)] private int _enemiesWeight { get; set; } = 20;

    [field: Header("Debug")]
    [field: SerializeField, ReadOnlyField] private float _timeElapsed { get; set; } // Tiempo transcurrido en el nivel
    private static string _timeFormatted { get; set; }
    public static string TimeFormatted {
        get { return _timeFormatted; }
        set { _timeFormatted = value; }
    }
    //[field: SerializeField] private int ObjectivesCompleted { get; set; }
    [field: SerializeField, ReadOnlyField] private int _enemiesDefeatedDebug { get; set; }

    private static int _enemiesDefeated { get; set; }
    public static int EnemiesDefeated {
        get { return _enemiesDefeated; }
        set { _enemiesDefeated = value; }
    }
    [field: SerializeField, ReadOnlyField] private int _inspectorScore { get; set; } // Puntuación que se muestra en el inspector.
    private static int _score { get; set; }
    public static int Score {
        get { return _score; }
        set { _score = value; }
    }

    private int _timeScore { get; set; }

    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando están en una escena
            if (timerText == null)
                timerText = GameObject.Find("TimerText").GetComponent<Text>();
        }
#endif
    }

    private IEnumerator Start() {
        _maxEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _enemiesDefeated = _enemiesDefeatedDebug = 0;
        yield return null;
    }

    private void Update() {

        // El tiempo se detendrá si el juego está en pausa.
        if (!PauseManager.onPause) {
            _timeElapsed += Time.deltaTime;
            TimerTextUpdate();
        }
    }

    private int CalculateScore() {
        // Puntuación del tiempo
        _timeScore = (int)(_maxScore * _timeWeight * (_maxTime - _timeElapsed) / _maxTime);

        // Puntuación de los objetivos cumplidos
        //int objectivesScore = MaxScore * ObjectivesWeight * ObjectivesCompleted / MaxObjectives;

        // Puntuación de los enemigos neutralizados
        int enemiesScore = _maxScore * _enemiesWeight * _enemiesDefeated / _maxEnemies;

        _enemiesDefeatedDebug = _enemiesDefeated;

        return _inspectorScore = _score = _timeScore /*+ objectivesScore*/ + enemiesScore;
    }

    public void SaveData() {
        int sceneIndex = LoadScene.CurrentIndexScene();
        int sceneScore = CalculateScore();

        // Guardar la puntuación total de la escena en el playerPref restándole la puntuación anterior para actualizar adecuadamente el valor.
        //PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) - PlayerPrefs.GetInt($"Level_{sceneIndex}", 0) + sceneScore);

        // Guardar la puntuación total de la escena en la base de datos restándole la puntuación anterior para actualizar adecuadamente el valor.
        DataBaseSQLManager.SaveScore(DataBaseSQLManager.LoadScore() - PlayerPrefs.GetInt($"Level_{sceneIndex}", 0) + sceneScore);

        //DataBaseSQLManager.SaveScore(PlayerPrefs.GetInt("TotalScore", 0));

        // Guardar la puntuación de la escena en el playerPref.
        int currentScore = CalculateScore();

        if (currentScore > PlayerPrefs.GetInt($"Level_{sceneIndex}", 0)) {
            PlayerPrefs.SetInt($"Level_{sceneIndex}", currentScore);
            PlayerPrefs.SetInt($"Level_{sceneIndex}_TimeInt", _timeScore);
            PlayerPrefs.SetString($"Level_{sceneIndex}_TimeFormatted", timerText.text);
        }

    }

    void TimerTextUpdate() {

        /*mins = Mathf.Floor(_timeElapsed / 60).ToString("00");
        seconds = (_timeElapsed % 60).ToString("00");*/

        //timerText.text = string.Format("{0}:{1}", mins, seconds);
        timerText.text = _timeFormatted = FormatTime(_timeElapsed);
    }

    string FormatTime(float timeElapsed) {
        mins = Mathf.Floor(_timeElapsed / 60).ToString("00");
        seconds = (_timeElapsed % 60).ToString("00");

        return string.Format("{0}:{1}", mins, seconds);
    }
}
