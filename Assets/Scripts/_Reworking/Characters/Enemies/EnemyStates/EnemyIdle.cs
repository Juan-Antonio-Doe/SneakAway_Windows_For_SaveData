using UnityEngine;
using static EnemyState;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Handle the enemy's idle state.
/// If the enemy is lazy, start a counter and switch to the sleeping state. 
/// When he returns to this state, his vision gradually recovers.
/// </summary>

public class EnemyIdle : EnemyState {

    private float awakeTime = 3f;

    public EnemyIdle(GameObject npc, EnemyV2 enemy, NavMeshAgent agent, PlayerManager player, VisionCone fov) 
        : base(npc, enemy, agent, player, fov) {
        
        currentState = STATE.Idle;
    }

    public override void Enter() {
        if (enemy.Waypoints.Count > 0) {    // If the enemy has waypoints, switch to the patrol state.
            nextState = new EnemyPatrol(npc, enemy, agent, player, fov);

            stage = STAGES.Exit;
            return;
        }

        awakeTime = enemy.MaxAwakeTime;

        base.Enter();
    }

    public override void Update() {
        // If the player is in the enemy's vision, switch to the chase state.
        if (CanSeePlayer()) {
            nextState = new EnemyChase(npc, enemy, agent, player, fov);

            stage = STAGES.Exit;
            return;
        }

        if (CorpseDetected()) {
            nextState = new EnemyLook(npc, enemy, agent, player, fov);

            stage = STAGES.Exit;
            return;
        }

        // If "sleepGuard" is true, start a counter and switch to the sleeping state.
        if (enemy.LazyGuard) {
            if (fov.ViewRadius < fov.MaxViewRadius) {   // Gradually restore the vision radius.
                fov.ViewRadius += Time.deltaTime * 2f;
            }
            else if (awakeTime > 0) {   // If vision is restored, start a counter.
                awakeTime -= Time.deltaTime;
            }
            else {  // If the counter is over, switch to the sleeping state.
                nextState = new EnemySleep(npc, enemy, agent, player, fov);

                stage = STAGES.Exit;
                return;
            }
        }

        base.Update();
    }
}
