using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

/**
 * Deprecated along with the attempt to use EnhancedTouch (PlayerTouchMovement.cs).
 * 
 * Currently unused.
 */

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class FloatingJoystick : MonoBehaviour {

    [field: SerializeField, GetComponent, ReadOnlyField] public RectTransform RectTransform { get; private set; }
    [field: SerializeField] public RectTransform Knob { get; private set; }
}
