using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/**
 * Purpose:
 * Handle the PowerUp pick up.
 */

[RequireComponent(typeof(SphereCollider)), AddComponentMenu("Scripts/ESI/Items/PickUps/PowerUp PickUp")]
public class PowerUp_PickUp : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, FindObjectOfType, ReadOnlyField] public PowerUpsManager powerUpsManager { get; private set; }
    [field: SerializeField, FindObjectOfType, ReadOnlyField] public PlayerManager player { get; private set; }
    [field: SerializeField, GetComponent, ReadOnlyField] private AudioSource pickUpSound { get; set; }
    [field: GetComponent] private MeshRenderer meshRenderer { get; set; }

    [field: SerializeField, Tooltip("PowerUp ScriptableObject")] private PowerUp powerUp { get; set; }
    [field: SerializeField, Range(0, 6), Tooltip("Assing a unique ID for the PowerUp.")] private int powerUpIndex { get; set; }
    [field: SerializeField, Tooltip("Button that will active de PowerUp")] public GameObject powerUpButtonGO { get; private set; }
    private Button powerUpButton { get; set; }
    [field: SerializeField, ReadOnlyField] private Image powerUpButtonIcon { get; set; }
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
                // Assign the components automatically of the PowerUp Button assigned.
                if (powerUpButtonGO != null && powerUpButton == null) {
                    powerUpButtonIcon = powerUpButtonGO.transform?.GetChild(1).GetComponent<Image>();
                    powerUpButton = powerUpButtonGO.GetComponent<Button>();
                }
                // Unassign the components automatically when the PowerUp Button assigned is changed.
                if (powerUpButtonGO == null || powerUpButton.gameObject != powerUpButtonGO) {
                    powerUpButtonIcon = null;
                    powerUpButton = null;
                }
                // Assign the sprite of the PowerUp to the PowerUp Button.
                if (powerUpButtonIcon != null && powerUp?.sprite != null && powerUpButtonIcon.sprite != powerUp?.sprite) {
                    powerUpButtonIcon.sprite = powerUp?.sprite;
                }

                AssingComponents();
            }
        }
#endif
    }
//#endif

    private bool picked { get; set; }

    private void Awake() {
        AssingComponents();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !picked) {
            picked = true;
            meshRenderer.enabled = false;
            powerUpsManager.AddPowerUp(powerUpIndex, powerUp);
            powerUpButtonGO.SetActive(true);
            player.hasPowerUp = true;
            StartCoroutine(DestroyPickUp());
        }
    }

    IEnumerator DestroyPickUp() {
        pickUpSound.Play();
        yield return new WaitForSeconds(pickUpSound.clip.length);   // Wait for the sound to finish.
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void AssingComponents() {
        if (pickUpSound == null)
            pickUpSound = GetComponent<AudioSource>();
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
    }

}
