using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Chase the player.
/// </summary>

internal class EnemyChase : EnemyState {

    private float alertTimer { get; set; } = 0f;    // Timer to look for the player.

    public EnemyChase(GameObject npc, EnemyV2 enemy, NavMeshAgent agent, PlayerManager player, VisionCone fov)
        : base(npc, enemy, agent, player, fov) {

        currentState = STATE.Chase;
        agent.speed = 4.5f;
        agent.isStopped = false;
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Update() {
        // Restore the view radius.
        if (enemy.LazyGuard)
            if (fov.ViewRadius < fov.MaxViewRadius)
                fov.ViewRadius += Time.deltaTime * 8f;

        if (!CanSeePlayer()) {
            
            if (alertTimer < enemy.MaxAlertTimer) {
                alertTimer += Time.deltaTime;
            }
            else {
                alertTimer = 0;
                nextState = new EnemyLook(npc, enemy, agent, player, fov);
                
                stage = STAGES.Exit;
                return;
            }
        }

        agent.destination = player.transform.position;

        base.Update();
    }

    public override void Exit() {
        agent.speed = 4f;
        base.Exit();
    }
}