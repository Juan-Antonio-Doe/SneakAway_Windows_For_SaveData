using Newtonsoft.Json.Linq;
using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manage the level button in the level select menu
/// </summary>

[AddComponentMenu("Scripts/ESI/LevelSelectMenu/LevelButton")]
public class LevelButton : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, FindObjectOfType] private LevelSelectMenu _levelSelectManager { get; set; }
    [field: SerializeField, GetComponentInChildren] private Text _levelText { get; set; }
    [field: SerializeField, GetComponent] private Button _button { get; set; }
    [field: SerializeField, GetComponent] private Image _image { get; set; }

    [field: Header("Button properties")]
    [field: SerializeField, Tooltip("Level to load.")] private int _level { get; set; }
    [field: SerializeField] private Sprite _lockedSprite { get; set; }
    private Sprite _defaultSprite { get; set; }
    [field: SerializeField, Tooltip("Word 'level' for translations")] private string _levelLabel { get; set; }
    [field: SerializeField] private Image _LockImage { get; set; }
    [field: SerializeField] private GameObject statsPanel { get; set; }
    private int _sceneIndex { get; set; }

    [field: Header("Stats properties")]
    [field: SerializeField] private Text _levelScoreValueText { get; set; }
    //private int _levelScore { get; set; }
    [field: SerializeField] private Text _levelTimeValueText { get; set; }
    //private string _levelTime { get; set; }

    void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando están en una escena
            AssignComponents();
        }
#endif
    }

    private void Awake() {
        AssignComponents();
    }

    public void Setup(int level, bool isUnlocked, int firtLevelScenesIndex) {
        _level = level;
        _sceneIndex = firtLevelScenesIndex + level - 1;
        _levelText.text = $"{_levelLabel}: {_level}";
        _button.interactable = isUnlocked;
        //_levelText.gameObject.SetActive(isUnlocked);

        // If the level is unlocked, the image is _defaultSprite, else the image is the locked sprite.
        //_image.sprite = isUnlocked ? _defaultSprite : _lockedSprite;
        _LockImage.enabled = !isUnlocked;
        statsPanel.SetActive(isUnlocked);

        LoadDataLevel();  // PlayerPrefs
    }

    public void OnClick() {
        _levelSelectManager.LoadLevel(_level);
    }

    private void LoadDataLevel() {
        //ToDo: Load the level data from the save file
        /*_levelScoreValueText.text = PlayerPrefs.GetInt(SceneManager.GetSceneByBuildIndex(_level).name, 0).ToString();
        _levelTimeValueText.text = PlayerPrefs.GetString(SceneManager.GetSceneByBuildIndex(_level).name + "Time", "00:00").ToString();*/
        _levelScoreValueText.text = string.Format("{0:D10}", PlayerPrefs.GetInt($"Level_{_sceneIndex}", 0));
        _levelTimeValueText.text = PlayerPrefs.GetString($"Level_{_sceneIndex}_TimeFormatted", "00:00");
    }

    /*public void ShowDataLevel() {
        //ToDo: Show the level data in the UI
        
    }*/

    private void AssignComponents() {
        if (_button == null)
            _button = GetComponent<Button>();
        if (_image == null)
            _image = GetComponent<Image>();
        
        _defaultSprite = _image.sprite;
    }
}
