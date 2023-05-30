using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manage the level select menu.
/// </summary>

[AddComponentMenu("Scripts/ESI/LevelSelectMenu/Level Select Manager")]
public class LevelSelectMenu : MonoBehaviour {

    //[field: Header("AutoAttach on Editor properties")]

    [field: Header("Level Select Menu properties")]
    [field: SerializeField] private LevelButton[] _levelButtons { get; set; }
    [field: SerializeField, ReadOnlyField] private int _totalLevels { get; set; }
    [field: SerializeField] private int _unlokedLevels { get; set; } = 0;

    [field: SerializeField, ReadOnlyField] private int _totalPages { get; set; } = 1;
    private int _currentPage { get; set; } = 0;
    [field: SerializeField] private int _levelsPerPage { get; set; } = 9;
    
    [field: SerializeField] private GameObject _backButton { get; set; }
    [field: SerializeField] private GameObject _nextButton { get; set; }

    [field: SerializeField] private Text _currentLevelText { get; set; }
    [field: SerializeField, Tooltip("Phrase 'Current level' for translations")] private string _currentLevelLabel { get; set; }

    [field: SerializeField, Tooltip("In the Build Settings menu, the Index of the first level scene.")
        ] private int _firtLevelScenesIndex { get; set; }

    private void OnEnable() {
        _levelButtons = GetComponentsInChildren<LevelButton>();
    }

    private IEnumerator Start() {
        // Number of levels based on the number of scenes in the build settings less the first level scene index.
        _totalLevels = SceneManager.sceneCountInBuildSettings - _firtLevelScenesIndex;
        _unlokedLevels = PlayerPrefs.GetInt("UnlokedLeves", 1);
        UpdateLastLevel();

        Refresh();
        yield return null;
    }

    public void NextPage() {
        _currentPage++;
        Refresh();
    }

    public void PreviousPage() {
        _currentPage--;
        Refresh();
    }

    public void Refresh() {
        _totalPages = _totalLevels / (_levelsPerPage + 1);

        int index = _currentPage * _levelsPerPage;

        for (int i = 0; i < _levelButtons.Length; i++) {
            int level = index + i + 1;

            if (level <= _totalLevels) {
                _levelButtons[i].gameObject.SetActive(true);
                _levelButtons[i].Setup(level, level <= _unlokedLevels, _firtLevelScenesIndex);
            }
            else {
                _levelButtons[i].gameObject.SetActive(false);
            }
        }

        CheckPageButtons();
    }

    public void LoadLevel(int level) {
        //ToDo_1: Load the level.
        if (level <= _unlokedLevels) {
            int sceneIndex = _firtLevelScenesIndex + level - 1;
            if (LoadScene.IsValidScene(sceneIndex))
                LoadScene.Load(sceneIndex);
            else
                Debug.LogError($"Scene {sceneIndex} is not valid.");
        }
        
        //ToDo_2: Show dialog with level data. Or update stats in the level select menu.

        // Test
        /*if (level == _unlokedLevels) {
            _unlokedLevels++;
            Refresh();
        }*/
    }

    public void ExitMenu() {
        LoadScene.Load("MainMenu");
    }

    private void CheckPageButtons() {
        _backButton.SetActive(_currentPage > 0);
        _nextButton.SetActive(_currentPage < _totalPages);
    }

    public void LoadData() {
        //ToDo: Load the data from the save file.
        
    }

    void UpdateLastLevel() {
        /*int displayLastLevel = _unlokedLevels;
        if (displayLastLevel > _totalLevels) {
            displayLastLevel = _totalLevels;
        }*/

        int displayLastLevel = Mathf.Min(_unlokedLevels, _totalLevels);

        _currentLevelText.text = $"{_currentLevelLabel}: {displayLastLevel}";
    }
}
