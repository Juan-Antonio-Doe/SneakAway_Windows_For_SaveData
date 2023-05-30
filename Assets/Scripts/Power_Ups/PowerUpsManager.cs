using Cinemachine;
using Nrjwolf.Tools.AttachAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Manage the power ups.
/// </summary>

[AddComponentMenu("Scripts/ESI/LevelManagement/PowerUps Manager")]
public class PowerUpsManager : MonoBehaviour {

    //public static PowerUpsManager instance { get; private set; }

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, FindObjectOfType, ReadOnlyField] private EnemyManager _enemies { get; set; }
    [field: SerializeField, FindObjectOfType, ReadOnlyField] private CinemachineVirtualCamera _virtualCamera { get; set; }

    [field: Header("Scripts properties")]
    [field: SerializeField] private float _mapPU_camTargetSizeExpansion { get; set; } = 13f;
    [field: Header("Power Ups obtained by the player")]
    [field: SerializeField, ReadOnlyField] public PowerUp[] powerUpsUnlocked { get; private set; } = new PowerUp[6];
    
    [field: Header("Debug")]
    [field: SerializeField] private List<GameObject> _enemyCams { get; set; } = new List<GameObject>();
    [field: SerializeField, ReadOnlyField] private List<Animator> _enemyCamAnimators { get; set; } = new List<Animator>();
    [field: SerializeField, ReadOnlyField] private List<GameObject> _enemyCamVisionCones { get; set; } = new List<GameObject>();
    [field: SerializeField, Tooltip("Text for debug on Build")] private Text _debugText { get; set; }

    private bool mapInUse;

//#if UNITY_EDITOR && !PlayMode
    // This is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando están en una escena
            if (!Application.isPlaying) {
                // Assing the enemyCams components.
                if (_enemyCams.Count > 0) {
                    foreach (GameObject enemyCam in _enemyCams) {
                        if (enemyCam != null) {
                            var enemyCamAnimator = enemyCam.GetComponent<Animator>();
                            var enemyCamVisionCone = enemyCam.GetComponentInChildren<VisionCone>().gameObject;

                            if (!_enemyCamAnimators.Contains(enemyCamAnimator))
                                _enemyCamAnimators.Add(enemyCamAnimator);

                            if (!_enemyCamVisionCones.Contains(enemyCamVisionCone))
                                _enemyCamVisionCones.Add(enemyCamVisionCone);
                        }
                    }
                }
            }
        }
#endif
    }
//#endif

    public void AddPowerUp(int index, PowerUp powerUp) {
        powerUpsUnlocked[index] = powerUp;
    }

    public void RemovePowerUp(int index) {
        powerUpsUnlocked[index] = null;
    }

    /// <summary>
    /// Instantiate a trap on the position given.
    /// </summary>
    public void InstantiateTrapPowerUp(int index, Vector3 position) {
        //_debugText.text = $"Trap {_enemyCams.Count}: Test";
        if (ValidatePowerUp(index))
            return;
        
        GameObject powerUpTrap = powerUpsUnlocked[index].prefab;

        /*AudioSource audioPU = powerUp.GetComponent<AudioSource>();
        if (audioPU != null)
            audioPU.Play();*/

        Instantiate(powerUpTrap, new Vector3(position.x, powerUpTrap.transform.position.y, position.z), powerUpTrap.transform.rotation);
        RemovePowerUp(index);
    }

    /// <summary>
    /// Active the EMP power up.
    /// </summary>
    public void ActivateEMPPowerUp(int index) {
        /*_debugText.text = $"EMP {ValidatePowerUp(index)}: {_enemyCamAnimators.Count} cameras disabled. \n" +
            $"{_enemyCams.Count} cameras in total. \n Animator: {_enemyCamAnimators[0]}";*/
        //_debugText.text += $"EMP {ValidatePowerUp(index)}: Test \n";

        if (ValidatePowerUp(index))
            return;

        _enemies.CommunicationsDisabled = true;

        //_debugText.text += $"EMP2 {_enemyCamAnimators.Count} + {_enemyCamVisionCones.Count} \n";

        // Disable the cameras.
        for (int i = 0; i < _enemyCamAnimators.Count; i++) {
            _enemyCamAnimators[i].enabled = false;
            _enemyCamVisionCones[i].SetActive(false);
        }

        RemovePowerUp(index);
    }

    /// <summary>
    /// Active the Map power up.
    /// </summary>
    public void ActivateMapPowerUp(int index) {
        //Debug.Log($"Map power up activated: !{ValidatePowerUp(index)}");
        if (ValidatePowerUp(index))
            return;

        //Debug.Log("Map power up activated");

        if (!mapInUse)
            StartCoroutine(MapPowerUpEffect(index));
    }

    // Check if the index given is valid and if the power up is not null.
    private bool ValidatePowerUp(int index) {
        //Debug.Log($"EMP activo: {index} vs {powerUpsUnlocked.Length} vs {powerUpsUnlocked[index]}");

        //_debugText.text += $"Validate: Index {index}/{powerUpsUnlocked.Length} - {powerUpsUnlocked[index]} \n";

        if (index >= powerUpsUnlocked.Length || powerUpsUnlocked[index] == null)
            return true;
        
        return false;
    }

    IEnumerator MapPowerUpEffect(int index) {
        mapInUse = true;
        float initialSize = _virtualCamera.m_Lens.OrthographicSize;
        //float targetSize = 13;
        float targetSize = _mapPU_camTargetSizeExpansion;
        float duration = 1; // Transition time in seconds.
        float elapsedTime = 0;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            _virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / duration);
            yield return null;
        }

        _virtualCamera.m_Lens.OrthographicSize = targetSize;

        yield return new WaitForSeconds(powerUpsUnlocked[index].duration);

        elapsedTime = 0;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            _virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(targetSize, initialSize, elapsedTime / duration);
            yield return null;
        }

        _virtualCamera.m_Lens.OrthographicSize = initialSize;
        mapInUse = false;
    }
}
