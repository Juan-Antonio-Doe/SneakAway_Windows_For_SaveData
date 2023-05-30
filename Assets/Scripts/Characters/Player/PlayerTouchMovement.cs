using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System;

/**
 * First attempt to create a touch movement for the player using EnhancedTouch.
 * Deprecated because the touch input is completely unstable.
 * 
 * Currently unused.
 */

public class PlayerTouchMovement : MonoBehaviour {

    [field: SerializeField] public Vector2 joystickSize = new Vector2(150, 150);
    [field: SerializeField] public FloatingJoystick joystick { get; private set; }
    [field: SerializeField] public NavMeshAgent Player { get; private set; }

    Finger movementFinger;
    Vector2 movementAmount;

    private void OnEnable() {
        EnableTouch();
    }

    // Update is called once per frame
    void Update() {
        if (!PauseManager.onPause && !EnhancedTouchSupport.enabled) {
            EnableTouch();
            Debug.Log("Touch enabled");
        }
        else if (PauseManager.onPause) {
            if (EnhancedTouchSupport.enabled) {
                DisableTouch();
                Debug.Log("Touch disabled");
            }
            
            return;
        }

        Vector3 scaledMovement = Player.speed * Time.deltaTime * new Vector3(movementAmount.x, 0, movementAmount.y);

        Player.transform.LookAt(Player.transform.position + scaledMovement, Vector3.up);
        Player.Move(scaledMovement);
    }

    private void HandleFingerMove(Finger movedFinger) {
        if (movedFinger == movementFinger) {
            Vector2 knobPos;
            float maxMovement = joystickSize.x / 2;
            ETouch.Touch currentThouc = movedFinger.currentTouch;

            if (Vector2.Distance(currentThouc.screenPosition, joystick.RectTransform.anchoredPosition) > maxMovement) {
                knobPos = (currentThouc.screenPosition - joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else {
                knobPos = currentThouc.screenPosition - joystick.RectTransform.anchoredPosition;
            }

            joystick.Knob.anchoredPosition = knobPos;
            movementAmount = knobPos / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger lostFinger) {
        if (lostFinger == movementFinger) {
            DisableJoystick();
        }
    }

    private void DisableJoystick() {
        movementFinger = null;
        movementAmount = Vector2.zero;
        joystick.Knob.anchoredPosition = Vector2.zero;
        joystick.gameObject.SetActive(false);
    }

    private void HandleFingerDown(Finger touchedFinger) {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width/* / 2f*/) {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.RectTransform.sizeDelta = joystickSize;
            joystick.RectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition) {
        if (startPosition.x < joystickSize.x/* / 2*/) {
            startPosition.x = joystickSize.x/* / 2*/;
        }
        
        if (startPosition.y < joystickSize.y/* / 2*/) {
            startPosition.y = joystickSize.y/* / 2*/;
        }
        else if (startPosition.y > Screen.height - joystickSize.y/* / 2*/) {
            startPosition.y = Screen.height - joystickSize.y/* / 2*/;
        }

        return startPosition;
    }

    private void EnableTouch() {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void DisableTouch() {
        DisableJoystick();
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void OnDisable() {
        DisableTouch();
    }
}
