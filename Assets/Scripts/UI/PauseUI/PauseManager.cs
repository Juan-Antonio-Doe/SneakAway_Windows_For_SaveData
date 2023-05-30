using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.Windows;

/// <summary>
/// Manage the Pause UI and the pause state of the game.
/// </summary>

[AddComponentMenu("Scripts/ESI/UI/Pause UI Manager")]
public class PauseManager : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, FindObjectOfType, ReadOnlyField] private InputHandler input { get; set; }

    [field: Header("General pause properties")]
    [field: SerializeField] private GameObject _pauseMenu { get; set; }
    [field: SerializeField] private GameObject _settingsMenu { get; set; }
    [field: SerializeField, ReadOnlyField] public static bool onPause { get; private set; } = false;

    [field: SerializeField] private GameObject[] objectsToDisable { get; set; }

    [field: Header("Setting menu")]
    [field: SerializeField] private Slider _volumeSlider { get; set; }
    [field: SerializeField] private AudioMixer _audioMixer { get; set; }
    [field: SerializeField] private Slider _brightnessSlider { get; set; }
    [field: SerializeField] private Light directionalLight { get; set; }

    const string MIXER_MASTER = "MasterVol";

    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando están en una escena
            if (directionalLight == null)
                directionalLight = FindObjectOfType<Light>(true);
        }
#endif
    }

    void Start() {
        StartCoroutine(LoadData());
        EnableDisableObjects(false);
    }

    // Update is called once per frame
    void Update() {
        if (input.PauseWasPressedThisFrame)
            Pause();
    }

    public void Pause() {
        if (!onPause) {
            onPause = true;
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
            UIUtility.EnableDisableObjects(false, objectsToDisable);
        }
        else {
            onPause = false;
            EnableDisableObjects(false);
            UIUtility.EnableDisableObjects(true, objectsToDisable);
            Time.timeScale = 1;
        }
    }

    #region General Methods
    public void LoadLevel(string levelName=null) {
        ResetBeforeChangeLevel();
        // if levelName is null or empty, load the current scene. Else, load the scene with the name passed.
        LoadScene.Load(levelName == null || levelName.Equals("") ? LoadScene.CurrentNameScene() : levelName);
    }

    private void ResetBeforeChangeLevel() {
        onPause = false;
        EnableDisableObjects(false);
        Time.timeScale = 1;
    }

    private void EnableDisableObjects(bool active = true) {
        _pauseMenu.SetActive(active);
        _settingsMenu.SetActive(active);
    }

    /// <summary>
    /// Used for scripts with UI elements like pause menu for tell to other scripts that the game is paused.
    /// </summary>
    public static void SetOnPause(bool value) {
        onPause = value;
        if (value)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
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

        _volumeSlider.value = PlayerPrefs.GetFloat("musicVol", 0.5f);
        _audioMixer.SetFloat(MIXER_MASTER, Mathf.Log10(_volumeSlider.value) * 20);
        _brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 1f);
        directionalLight.intensity = _brightnessSlider.value;
    }
    #endregion
}
