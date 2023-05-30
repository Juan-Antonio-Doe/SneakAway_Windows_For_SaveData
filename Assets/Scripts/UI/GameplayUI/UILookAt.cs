using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * Purpose:
 * Makes the object face the camera.
 */

[AddComponentMenu("Scripts/ESI/UI/Gameplay UI/Look At")]
public class UILookAt : MonoBehaviour {

    [field: SerializeField, FindObjectOfType] public Camera mainCamera { get; private set; }

    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected && !EditorApplication.isPlaying) {
            //Variables que solo se verificaran cuando están en una escena
            if (mainCamera == null)
                mainCamera = FindObjectOfType<Camera>();
        }
#endif
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.LookAt(transform.position + mainCamera.transform.rotation * 
            Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
