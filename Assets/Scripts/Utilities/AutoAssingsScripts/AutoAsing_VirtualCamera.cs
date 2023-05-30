using Cinemachine;
using Nrjwolf.Tools.AttachAttributes;
using UnityEditor;
using UnityEngine;

public class AutoAsing_VirtualCamera : MonoBehaviour {

    [field: GetComponent, SerializeField, ReadOnlyField] CinemachineVirtualCamera vcam { get; set; }
    [field: FindObjectOfType, SerializeField, ReadOnlyField] PlayerManager player { get; set; }
    [field: SerializeField] bool lockScript { get; set; }

    private void OnValidate() {
#if UNITY_EDITOR
        if (lockScript)
            return;

        /*UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected && !EditorApplication.isPlaying) {
            //Variables que solo se verificaran cuando están en una escena
            if (vcam != null) {
                
                Debug.Log("Virtual Camera should be asigned.");
            }
        }*/

        if (player != null) {
            if (vcam.Follow == null)
                vcam.Follow = player.transform;
            if (vcam.LookAt == null)
                vcam.LookAt = player.transform;
        }
#endif
    }

}
