using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Disable some functions when the enemy is dead and handle the corpse movement.
/// </summary>

internal class EnemyDead : EnemyState {

    private Transform father;

    public EnemyDead(GameObject npc, EnemyV2 enemy, NavMeshAgent agent, PlayerManager player, VisionCone fov)
        : base(npc, enemy, agent, player, fov) {

        currentState = STATE.Dead;
        agent.speed = 0f;
        agent.isStopped = true;
    }

    public override void Enter() {
        fov.StopView();
        enemy.SphereCollider.enabled = false;   // Disable the collider for reducing the performance cost.

        npc.tag = "Corpse";
        npc.layer = LayerMask.NameToLayer("Corpse");
        enemy.Anim.SetTrigger("EnemyDead");
        //enemy.CapsuleCollider.enabled = false;
        agent.enabled = false;

        //Change the enemy's material to a dead one.
        foreach (MeshRenderer mesh in enemy.ObjectsToChangeColor) {
            mesh.material = enemy.DeathMaterialColor;
        }
        
        father = npc.transform.parent;

        ScoreManager.EnemiesDefeated++;

        base.Enter();
    }

    public override void Update() {
        /* 
         * We parent the enemy to the player so that the enemy moves 
         * at the same time as the player while the button "moveCorpse" is pressed.
         */
        if (enemy.canMoveCorpse) {
            if (npc.transform.parent == null || npc.transform.parent == father) {
                if (player.input.MoveCorpseIsPressed) {
                    npc.transform.parent = player.transform;
                }
            }
        }
        if (npc.transform.parent != null) {
            if (!player.input.MoveCorpseIsPressed) {
                npc.transform.parent = father;
            }
        }
        
        base.Update();
    }
}