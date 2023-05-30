using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// They search for the player by moving through all the waypoints in the scene.
/// </summary>

internal class EnemyLook : EnemyState {
    public EnemyLook(GameObject npc, EnemyV2 enemy, NavMeshAgent agent, PlayerManager player, VisionCone fov)
        : base(npc, enemy, agent, player, fov) {

        currentState = STATE.Look;
        agent.speed = 4f;
        agent.isStopped = false;
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Update() {
        if (CanSeePlayer()) {
            nextState = new EnemyChase(npc, enemy, agent, player, fov);

            stage = STAGES.Exit;
            return;
        }

        ChooseRandomLocationToMoveTo();

        base.Update();
    }

    public override void Exit() {
        base.Exit();
    }

    // Choose a random location to move to.
    private void ChooseRandomLocationToMoveTo() {
        //Debug.Log($"Waypoints: {enemy.AllWaypoints.Count}");

        if (enemy.AllWaypoints.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = enemy.AllWaypoints[enemy.CurrentLookingWaypoint].position;

        if (agent.remainingDistance < agent.stoppingDistance) {
            // Choose the next point in the array as the destination,
            enemy.CurrentLookingWaypoint = Random.Range(0, enemy.AllWaypoints.Count - 1);
        }
    }
}