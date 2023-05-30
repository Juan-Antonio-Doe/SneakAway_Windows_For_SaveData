using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Father class for all enemy states.
/// </summary>

public class EnemyState {

    public enum STATE {
        Idle,
        Patrol,
        Chase,
        Look,
        Investigate,
        Sleep,
        Dead
    }

    public enum STAGES {
        Enter,
        Update,
        Exit
    }

    public STATE currentState { get; set; }
    protected STAGES stage { get; set; }
    protected GameObject npc { get; set; }  // The current enemy gameobject.
    protected EnemyV2 enemy { get; set; }
    protected PlayerManager player { get; set; }
    protected NavMeshAgent agent { get; set; }
    protected VisionCone fov { get; set; }
    protected EnemyState nextState { get; set; }

    public EnemyState(GameObject npc, EnemyV2 enemy, NavMeshAgent agent, PlayerManager player, VisionCone fov) {
        
        this.npc = npc;
        this.enemy = enemy;
        this.agent = agent;
        stage = STAGES.Enter;
        this.player = player;
        this.fov = fov;
    }

    #region State Methods
    public virtual void Enter() {
        stage = STAGES.Update;
    }

    public virtual void Update() {
        stage = STAGES.Update;
    }

    public virtual void Exit() {
        stage = STAGES.Exit;
    }

    /// <summary>
    /// This method is used to switch between the different methods that change the state.
    /// </summary>
    public EnemyState Process() {
        if (stage == STAGES.Enter) Enter();
        if (stage == STAGES.Update) Update();
        if (stage == STAGES.Exit) {
            Exit();
            return nextState; // It returns us the state that would touch next.
        }

        if (PlayerDetectedByCam() && (currentState != STATE.Chase) && (currentState != STATE.Investigate) 
            && (currentState != STATE.Dead) && !enemy.enemies.CommunicationsDisabled) {
            
            nextState = new EnemyInvestigate(npc, enemy, agent, player, fov);
            Exit();

            return nextState;
        }

        //Debug.Log($"Current This type: {this.GetType()}");

        // This would return us to the same state we are in if none of the above conditions are met.
        return this;
    }
    #endregion

    #region Mechanics Methods
    public bool CanSeePlayer() {
        if (enemy.enemies.CommunicationsDisabled)
            return fov.PlayerDetected;
        else
            return enemy.enemies.PlayerDetected;
    }
    public bool CorpseDetected() {
        if (enemy.enemies.CommunicationsDisabled)
            return fov.CorpseDetected;
        else
            return enemy.enemies.CorpseDetected;
    }
    public bool PlayerDetectedByCam() {
        return enemy.enemies.PlayerDetectedByCam;
    }

    // Check if the last player position detected by a camera has been reached by one of the enemies.
    public bool LastPlayerPositionChecked() {
        return enemy.enemies.lastPlayerPositionAlreadyCheked;
    }
    #endregion
}
