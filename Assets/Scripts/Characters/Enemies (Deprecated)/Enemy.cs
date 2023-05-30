using Nrjwolf.Tools.AttachAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/**
 * First attempt to create a generic enemy class.
 * Deprecated in favor of EnemyV2 and EnemyStates. [State Machine Pattern]
 * 
 * Currently unused.
 */

public class Enemy : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, GetComponent] public NavMeshAgent agent { get; private set; }
    [field: SerializeField, FindObjectOfType] public EnemyManager enemies { get; private set; }
    [field: SerializeField, GetComponent, ReadOnlyField] public FieldOfView myFoV { get; private set; }
    [field: SerializeField, GetComponentInChildren, ReadOnlyField] public Slider staminaBar { get; private set; }

    [field: Header("Other Editor properties")]
    [field: SerializeField] public Transform waypointsParent { get; private set; }
    [field: SerializeField, ReadOnlyField] public List<Transform> waypoints { get; set; } = new List<Transform>();

//#if UNITY_EDITOR && !PlayMode
    // This is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    private void OnValidate() {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage && prefabConnected) {
            //Variables que solo se verificaran cuando están en una escena
            if (!Application.isPlaying)
                if (waypointsParent != null && (waypoints.Count == 0 || waypoints.Count != waypointsParent.childCount)) {

                    waypoints.Clear();
                    foreach (Transform child in waypointsParent) {
                        waypoints.Add(child);
                    }
                }
        }
#endif
    }
//#endif

    /*#if UNITY_EDITOR
        [ContextMenu("AddWaypoints")]
        void AddWaypoints() {
            if (waypointsParent != null)
                foreach (Transform child in waypointsParent) {
                    waypoints.Add(child);
                }
            else
                Debug.LogWarning("WaypointsParent is null");
        }
    #endif*/

    [field: Header("Other properties")]
    [field: SerializeField, ReadOnlyField] public float currentStamina { get; private set; } = 3f;
    [field: SerializeField] public float maxStamina { get; private set; } = 3f;

    private bool isBeingNeutralized;
    [field: SerializeField, ReadOnlyField] public bool isDead { get; set; }
    [field: SerializeField, ReadOnlyField] public bool canMoveCorpse { get; set; }

    private int currentWaypointIndex { get; set; } = 0;
    private bool invertPatrol { get; set; }
    [field: SerializeField] public float waitEndPatrolTime { get; private set; } = 1f;

    List<Transform> allWaypoints { get; set; } = new List<Transform>();
    int currentLookingWaypoint { get; set; } = 0;

    // Start is called before the first frame update
    void Start() {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;

        allWaypoints = enemies.allWaypoints;
        currentLookingWaypoint = UnityEngine.Random.Range(0, allWaypoints.Count - 1);

        //agent.autoBraking = false;
        /*if (waypoints.Count != 0)
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
                StartCoroutine(PatrolCo(waitEndPatrolTime));*/
        agent.autoBraking = false;

        GotoNextPoint();
    }

    // Update is called once per frame
    void Update() {
        Neutralization();

        Decisions();
    }

    private void Decisions() {
        if (enemies.player.dead || isDead) {
            if (canMoveCorpse) {
                if (transform.parent == null) {
                    if (enemies.player.input.MoveCorpseIsPressed) {
                        transform.parent = enemies.player.transform;
                    }
                }
            }
            if (transform.parent != null) {
                if (!enemies.player.input.MoveCorpseIsPressed) {
                    transform.parent = null;
                }
            }
            return;
        }

        /*if (enemies.currentStatus == EnemyManager.AlertStatus.off) {
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
                GotoNextPoint();
        }

        // If the enemy is currently searching, choose a random spot to move to.
        if (enemies.currentStatus == EnemyManager.AlertStatus.looking) {
            ChooseRandomLocationToMoveTo();
        }
        else if (enemies.currentStatus == EnemyManager.AlertStatus.alert) {
            AgentMoveToPlayer();
        }*/
    }

    private void Neutralization() {
        if (isBeingNeutralized) {
            if (currentStamina > 0) {
                currentStamina -= Time.deltaTime;
                staminaBar.value = currentStamina;
            }
            else {
                currentStamina = 0;
                staminaBar.value = 0;
                isBeingNeutralized = false;
                //StartCoroutine(DeathCo());
                KillEnemy();
                //enemies.SetEnemyStatus(EnemyManager.AlertStatus.off, null);
            }
        }
    }

    void GotoNextPoint() {
        // Returns if no points have been set up
        if (waypoints.Count == 0)
            return;

        if ((currentWaypointIndex + 1) > waypoints.Count - 1) {
            invertPatrol = true;
        }
        else if (currentWaypointIndex - 1 < 0) {
            invertPatrol = false;
        }

        //Debug.Log(currentWaypointIndex);

        // Set the agent to go to the currently selected destination.
        agent.destination = waypoints[currentWaypointIndex].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        //currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;

        if (!invertPatrol)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        else
            currentWaypointIndex = (currentWaypointIndex - 1) % waypoints.Count;
    }

/*    private IEnumerator PatrolCo(float waitTime) {

        *//*if (currentWaypointIndex == 0)
            yield return new WaitForSeconds(waitTime);
        else if (currentWaypointIndex == waypoints.Count - 1)
            yield return new WaitForSeconds(waitTime);*//*

        //Debug.Log(currentWaypointIndex);

        // Set the agent to go to the currently selected destination.
        agent.destination = waypoints[currentWaypointIndex].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count - 1;

        yield return null;
    }*/

    private void ChooseRandomLocationToMoveTo() {
        if (allWaypoints.Count == 0)
            return;

        //Debug.Log(currentWaypointIndex);

        // Set the agent to go to the currently selected destination.
        agent.destination = allWaypoints[currentLookingWaypoint].position;

        if (agent.remainingDistance < agent.stoppingDistance) {
            // Choose the next point in the array as the destination,
            currentLookingWaypoint = UnityEngine.Random.Range(0, allWaypoints.Count - 1);
        }
    }

    private void AgentMoveToPlayer() {
        //agent.destination = enemies.target.position;
    }

    /*private Transform FindClosestWaypoint(Transform from) {
        // Default to the first element.
        Transform closestWaypoint = waypoints[0];
        float smallestDistance = Vector3.Distance(from.position, closestWaypoint.position);

        foreach (Transform waypoint in waypoints) {
            // Skip when element is already in the route.
            if (route.Contains(waypoint)) continue;

            // Update closest waypoint/distance.
            float distance = Vector3.Distance(from.position, waypoint.position);
            if (distance < smallestDistance) {
                closestWaypoint = waypoint;
                smallestDistance = distance;
            }
        }
    }*/

    public void KillEnemy() {
        StartCoroutine(DeathCo());
    }

    IEnumerator DeathCo() {
        if (isBeingNeutralized) {
            enemies.moveCorpseButton.SetActive(true);
        }
        isDead = true;
        agent.isStopped = true;
        myFoV.stopFoV = true;
        staminaBar.transform.parent.gameObject.SetActive(false);
        // ToDo: Change the enemy's material to a dead one.
        // ToDo: Enable mechanic for the player to pick up the enemy's body.
        tag = "Corpse";
        canMoveCorpse = true;
        yield return null;
    }

    /*private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OnTriggerEnterHandler();
        }
    }*/

    public void OnTriggerEnterHandler() {
        /*if (!enemies.player.dead && !isDead) {
            if (enemies.currentStatus == EnemyManager.AlertStatus.alert) {
                Debug.Log("Player is dead");
                enemies.player.KillPlayer();    // Execute death animation
            }
            else {
                isBeingNeutralized = true;
            }
        }*/

        if (isDead && !enemies.player.dead) {
            canMoveCorpse = true;
            enemies.moveCorpseButton.SetActive(true);
        }
    }

    /*private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            OnTriggerExitHandler();

        }
    }*/

    public void OnTriggerExitHandler() {
        if (isBeingNeutralized) {
            currentStamina = maxStamina;
        }
        isBeingNeutralized = false;

        canMoveCorpse = false;
        enemies.moveCorpseButton.SetActive(false);
    }
}
