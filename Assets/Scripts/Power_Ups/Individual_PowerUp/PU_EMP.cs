using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Purpose:
 * Unused after the implementation of the PowerUp as scriptableObject.
 */

public class PU_EMP : MonoBehaviour {

    [field: Header("EMP Power Up")]
    [field: SerializeField] public PowerUp empPowerUp { get; private set; }

    public bool isActivated { get; set; }
}
