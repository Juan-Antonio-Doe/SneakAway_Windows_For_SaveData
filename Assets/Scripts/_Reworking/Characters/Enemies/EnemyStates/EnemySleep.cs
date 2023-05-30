using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Decreases the cone of vision gradually and remains there for the indicated amount of time, 
/// then returns to the Idle state.
/// </summary>

internal class EnemySleep : EnemyState {

    private float sleepingTime = 1f;

    public EnemySleep(GameObject npc, EnemyV2 enemy, NavMeshAgent agent, PlayerManager player, VisionCone fov)
        : base(npc, enemy, agent, player, fov) {

        currentState = STATE.Sleep;
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

        // -- If he sees a dead body, since he is half asleep, he will ignore it, thinking it is alive. --


        if (fov.ViewRadius > fov.MinViewRadius) {   // Gradually reduce the vision radius.
            fov.ViewRadius -= Time.deltaTime;
        }
        else if (sleepingTime > 0) {    // If vision is reduced, start a counter.
            sleepingTime -= Time.deltaTime;
        }
        else {  // If the counter ends, return to the Idle state.
            nextState = new EnemyIdle(npc, enemy, agent, player, fov);

            stage = STAGES.Exit;
            return;
        }

        base.Update();
    }

    public override void Exit() {
        sleepingTime = enemy.MaxSleepingTime;   // Reset the counter.

        base.Exit();
    }
}