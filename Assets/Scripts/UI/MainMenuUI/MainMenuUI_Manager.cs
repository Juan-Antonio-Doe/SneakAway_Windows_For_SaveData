using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

/**
 * Purpose:
 * Manage the Main Menu UI.
 */

[AddComponentMenu("Scripts/ESI/UI/Main Menu UI Manager")]
public class MainMenuUI_Manager : MonoBehaviour {

    //[field: Header("AutoAttach on Editor properties")]


    [field: Header("Script properties")]
    [field: Header("Menus")]
    [field: SerializeField] private GameObject _mainMenu { get; set; }
    [field: SerializeField] private GameObject _settingMenu { get; set; }

    [field: Header("UI Components")]
    [field: SerializeField] private Text _scoreText { get; set; }
    private string _defaultScoreText { get; set; }

    [field: SerializeField] private Slider _volumeSlider { get; set; }
    [field: SerializeField] private AudioMixer _audioMixer { get; set; }
    [field: SerializeField] private Slider _brightnessSlider { get; set; }
    [field: SerializeField, Tooltip("Light to use for change the brightness")] private Light directionalLight { get; set; }
    [field: SerializeField] private Text _legalText { get; set; }

    public enum MenuState { MainMenu, SettingMenu }
    private MenuState _currentMenuState { get; set; } = MenuState.MainMenu;

    const string MIXER_MASTER = "MasterVol";

    #region Unity Methods
    private void Start() {
        _mainMenu.SetActive(true);
        _defaultScoreText = _scoreText.text;
        string legalText = _legalText.text;
        _legalText.text = $"© {legalText} - Versión: {Application.version}";

        _settingMenu.SetActive(false);
        
        StartCoroutine(LoadData());
    }
    #endregion

    #region General Methods
    // Manage the menu state
    private void ChangeMenuEnum(MenuState nextState) {
        switch (nextState) {
            case MenuState.MainMenu:
                _mainMenu.SetActive(true);
                _settingMenu.SetActive(false);
                break;
            case MenuState.SettingMenu:
                _mainMenu.SetActive(false);
                _settingMenu.SetActive(true);
                break;
        }
        _currentMenuState = nextState;
    }

    // Allow to change the menu state throw UI buttons.
    public void ChangeToMainMenu() => ChangeMenuEnum(MenuState.MainMenu);
    public void ChangeToSettingMenu() => ChangeMenuEnum(MenuState.SettingMenu);

    public void LoadLevel(string levelName) {
        LoadScene.Load(levelName);
    }

    public void LoadFirtLevel() {
        LoadScene.Load(3);
    }

    public void QuitApplication() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    #endregion

    #region SettingMenu Methods
    public void SetVolume(float val) {
        _audioMixer.SetFloat(MIXER_MASTER, Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat("musicVol", val);
    }

    public void SetBrightness(float val) {
        directionalLight.intensity = val;
        PlayerPrefs.SetFloat("brightness", val);
    }
    #endregion

    #region DataManagement Methods
    IEnumerator LoadData() {
        yield return null;

        /*if (PlayerPrefs.HasKey("TotalScore"))
            _scoreText.text = string.Format("{0:D10}", PlayerPrefs.GetInt("TotalScore"));*/
        /*else
            _scoreText.text = _defaultScoreText;*/

        int score = DataBaseSQLManager.LoadScore();

        _scoreText.text = score == null || score == 0 ? _defaultScoreText : string.Format("{0:D10}", score);

        _volumeSlider.value = PlayerPrefs.GetFloat("musicVol", 0.5f);
        _audioMixer.SetFloat(MIXER_MASTER, Mathf.Log10(_volumeSlider.value) * 20);
        _brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 1f);
        directionalLight.intensity = _brightnessSlider.value;
    }
    #endregion
}
