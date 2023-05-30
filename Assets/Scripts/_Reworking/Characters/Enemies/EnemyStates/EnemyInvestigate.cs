using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Investigate the last position of the player seen by the cameras. 
/// The first to arrive notifies the rest and they all switch to the look state.
/// </summary>

internal class EnemyInvestigate : EnemyState {
    public EnemyInvestigate(GameObject npc, EnemyV2 enemy, NavMeshAgent agent, PlayerManager player, VisionCone fov)
        : base(npc, enemy, agent, player, fov) {

        currentState = STATE.Investigate;
        agent.speed = 4f;
        agent.isStopped = false;
        agent.autoBraking = false;
    }

    public override void Enter() {
        agent.ResetPath();  // Clear the path for avoid some weird behaviours.

        base.Enter();
    }

    public override void Update() {

        agent.destination = enemy.enemies.lastPlayerPositionKnowed;

        if (CanSeePlayer()) {
            nextState = new EnemyChase(npc, enemy, agent, player, fov);

            stage = STAGES.Exit;
            return;
        }

        /*
         * If corpse is detected or the agent has reached the last known position of the player 
         * or the last known position has already been checked by another enemy,
         * switch to the look state.
         */
        if (CorpseDetected() || (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance) 
            || LastPlayerPositionChecked()) {
            
            enemy.enemies.lastPlayerPositionAlreadyCheked = true;
            
            nextState = new EnemyLook(npc, enemy, agent, player, fov);

            stage = STAGES.Exit;
            return;
        }

        base.Update();
    }

    public override void Exit() {
        agent.ResetPath();
        base.Exit();
    }
}