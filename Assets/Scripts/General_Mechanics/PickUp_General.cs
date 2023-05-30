using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Purpose:
 * Performs the actions indicated in the inspector when the object is picked up. 
 * It works like the OnClick event of a button.
 */

[RequireComponent(typeof(SphereCollider)), AddComponentMenu("Scripts/ESI/Items/PickUps/General PickUp")]
public class PickUp_General : MonoBehaviour {

    [field: SerializeField] private bool destroyOnPickUp { get; set; } = true;
    [field: SerializeField] private UnityEvent onPickUp { get; set; }

    private bool alreadyPickedUp = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !alreadyPickedUp) {
            alreadyPickedUp = true;
            onPickUp.Invoke();
            if (destroyOnPickUp)
                Destroy(gameObject);
        }
    }

}
