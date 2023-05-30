using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

/**
 * Purpose:
 * Handle the PowerUp Trap functionality.
 * Kill the enemy that trigger the trap.
 */

public class PU_Trap : MonoBehaviour {

    [field: SerializeField, GetComponent, ReadOnlyField] Animator animator;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && other.GetType() == typeof(CapsuleCollider)) {
            Debug.Log("Enemy hit by trap");
            other.GetComponent<EnemyV2>().KillEnemy(true);
            //Destroy(gameObject);
            if (animator != null) {
                animator.SetTrigger("ActivateTrap");
            }
        }
    }

    // Called by the animation event
    public void DestroyTrap() {
        Destroy(gameObject);
    }
}
