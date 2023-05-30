using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * First attempt to create a player controller using the horrible old Input Manager..
 * Deprecated in favor of the new Player Simple Movement script.
 * 
 * Currently unused.
 */

public class PlayerControllerTesting : MonoBehaviour {

    [field: SerializeField] public float moveSpeed { get; private set; } = 6;

    [field: SerializeField] public Rigidbody rb { get; private set; }
    [field: SerializeField] public Camera viewCamera { get; private set; }

    Vector3 velocity;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);

        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
