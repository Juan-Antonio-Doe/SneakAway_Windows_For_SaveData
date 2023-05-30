using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Firts attempt to create a trigger to knock up enemies.
 * Deprecated because the trigger is not working well with the new SphereCollider attempt.
 * 
 * Currently unused.
 */

public class KnockUpTriggerEnemy : MonoBehaviour {

    [field: SerializeField, GetComponentInParent] private Enemy enemy { get; set; }

    private void Awake() {
        if (enemy == null)
            enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //enemy.OnTriggerEnterHandler();
            
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            //enemy.OnTriggerExitHandler();
        }
    }

}
