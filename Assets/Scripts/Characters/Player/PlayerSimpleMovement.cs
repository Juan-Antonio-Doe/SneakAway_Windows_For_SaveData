using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

/// <summary>
/// Manage the movement of the player.
/// </summary>

[AddComponentMenu("Scripts/ESI/Characters/Player/Player Simple Movement")]
public class PlayerSimpleMovement : MonoBehaviour {

    [field: SerializeField, GetComponent, ReadOnlyField] public NavMeshAgent Player { get; private set; }

    // No se pueden usar propiedades en estas variables porque se pierden las referencias al reiniciar la escena.
    [field: SerializeField] private Image joystick { get; set; } // Image of the joystick (background)
    [field: SerializeField] private Image knob { get; set; } // Image of the center of the joystick (knob)

    private bool lockMovement { get; set; } = false;

    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando est�n en una escena
            if (joystick == null) {
                joystick = GameObject.Find("Joystick (SimpleInput)").GetComponent<Image>();
            }
            if (knob == null) {
                knob = GameObject.Find("Knob Inner").GetComponent<Image>();
            }
        }
#endif
    }

    // Update is called once per frame
    void Update() {
        // Update the alpha of the knob to match the joystick's alpha
        knob.color = new Color(knob.color.r, knob.color.g, knob.color.b, joystick.color.a);

        if (lockMovement)
            return;

        // Move the player
        Vector3 scaledMovement = Player.speed * Time.deltaTime * new Vector3(
            SimpleInput.GetAxisRaw("Horizontal"), 0, SimpleInput.GetAxisRaw("Vertical"));

        Player.transform.LookAt(Player.transform.position + scaledMovement, Vector3.up);
        Player.Move(scaledMovement);
    }

    public void EnableInput() {
        joystick.gameObject.SetActive(true);
        lockMovement = false;
    }

    public void DisableInput() {
        joystick.gameObject.SetActive(false);
        lockMovement = true;
    }
}
