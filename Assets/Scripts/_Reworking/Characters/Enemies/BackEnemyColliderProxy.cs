using UnityEditor;
using UnityEngine;

/**
 * Purpose:
 * Proxy script for manage a specific trigger.
 */

public class BackEnemyColliderProxy : MonoBehaviour {

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
        if (other.CompareTag("Player")) {
            if (!enemyV2.isDead) {
                enemyV2.KillingEnemy();
            }

            if (enemyV2.isDead && !enemyV2.enemies.player.dead) {   // If the enemy is dead and the player is alive.
                enemyV2.canMoveCorpse = true;
                enemyV2.enemies.moveCorpseButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            enemyV2.StopKillingEnemy();

            enemyV2.canMoveCorpse = false;
            enemyV2.enemies.moveCorpseButton.SetActive(false);
        }
    }

}
