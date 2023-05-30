using UnityEditor;
using UnityEngine;

/**
 * Purpose:
 * Proxy script for manage a specific trigger.
 */

public class FrontEnemyColliderProxy : MonoBehaviour {

    [field: SerializeField] private EnemyV2 enemyV2 { get; set; }

    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando están en una escena
            if (enemyV2 == null)
                enemyV2 = GetComponentInParent<EnemyV2>();
        }
#endif
    }

    private void Awake() {
        if (enemyV2 == null)
            enemyV2 = GetComponentInParent<EnemyV2>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!enemyV2.isDead && other.CompareTag("Player")) {    // Kill the player if the enemy is alive.
            enemyV2.enemies.player.KillPlayer();
        }
    }
}
