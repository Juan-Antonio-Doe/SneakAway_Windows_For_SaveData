using Nrjwolf.Tools.AttachAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/**
 * First attempt to create a Field of View script for the enemies that can detect the player.
 * Deprecated because causes a bad performance.
 * 
 * Currently unused.
 */

public class FieldOfView : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, FindObjectOfType] public EnemyManager enemies { get; private set; }

    [field: Header("Other properties")]
    [field: SerializeField] private float viewRadius { get; set; } = 6f;
    public float ViewRadius { get { return viewRadius; } set { viewRadius = value; } }

    [field: Range(0, 360)]
    [field: SerializeField] private float viewAngle { get; set; } = 100f;
    public float ViewAngle { get { return viewAngle; } }

    [field: SerializeField] public LayerMask targetMask { get; private set; }
    [field: SerializeField] public LayerMask obstacleMask { get; private set; }

    [field: SerializeField, ReadOnlyField] public List<Transform> visibleTargets { get; private set; } = new List<Transform>();
    [field: SerializeField] public float meshResolution { get; private set; }
    [field: SerializeField] public int edgeResolveIterations { get; private set; }
    [field: SerializeField] public float edgeDistanceThreshold { get; private set; } = 0.01f;

    [field: SerializeField] public MeshFilter viewMeshFilter { get; private set; }
    private Mesh viewMesh;

    [field: SerializeField, ReadOnlyField] public bool stopFoV { get; set; }
    [field: SerializeField, ReadOnlyField] public bool alerted { get; set; }

    [field: SerializeField] private bool lazyGuard { get; set; }
    [field: SerializeField] private float minViewRadius { get; set; } = 1.5f;
    public float MinViewRadius { get { return minViewRadius; } set { minViewRadius = value; } }
    private float maxViewRadius { get; set; } = 6f;
    public float MaxViewRadius { get { return maxViewRadius; } }
    private float sleepingTime { get; set; } = 1f;
    [field: SerializeField] public float maxSleepingTime { get; private set; } = 1f;
    private bool isSpleeping { get; set; }
    private float awakeTime { get; set; } = 3f;
    [field: SerializeField] public float maxAwakeTime { get; private set; } = 3f;
    private bool isAwake { get; set; } = true;

    private bool _playerDetected;
    public bool PlayerDetected { get { return _playerDetected; } }


    // Start is called before the first frame update
    void Start() {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        viewMesh.MarkDynamic();

        //StartCoroutine(FindTargetsWithDelay(0.2f));

        /*if (lazyGuard) {
            maxViewRadius = viewRadius;
            sleepingTime = maxSleepingTime;
            awakeTime = maxAwakeTime;
        }*/
    }

    private void Update() {
        SleepingGuard();
    }

    // Called after player movement.
    void LateUpdate() {
        if (stopFoV) {
            viewMeshFilter.mesh = null;
            return;
        }
        
        DrawFieldOfView();
    }

    // Convert an angle to a direction.
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if(!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void DrawFieldOfView() {
        // Convert the number of rays will cast per degree to an integer.
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;   // Calculate the angle between each ray.
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            // If the ray hits an obstacle, cast a ray from the edge of the obstacle.
            if (i > 0) {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hitting != newViewCast.hitting || 
                    (oldViewCast.hitting && newViewCast.hitting && edgeDistanceThresholdExceeded)) {
                    
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero; // The first vertex is the player's position.

        for (int i = 0; i < vertexCount - 1; i++) {
            // InvverseTransformPoint: Convert a point from world space to local space.
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
        viewMesh.Optimize();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hitting == minViewCast.hitting && !edgeDistanceThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    // Show the field of view in runtime. Blocking the view with obstacles.
    ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    // Find all targets within the view radius.
    void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            /*If the angle between the direction to the target and the forward direction of
             *the object is less than half the view angle, then the target is in view.*/


            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // If the target is not in the way of an obstacle.
                /*if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask)) {
                    //target.GetComponent<Renderer>().material.color = Color.red;
                    visibleTargets.Add(target);
                    // Detect Player

                    if (target.CompareTag("Player")) {
                        Debug.Log("Player Detected");
                        enemies.SetEnemyStatus(EnemyManager.AlertStatus.alert, target);
                    }

                    if (enemies.currentStatus != EnemyManager.AlertStatus.alert) {
                        if (target != gameObject) { // Ignore the enemy itself.
                            if (gameObject.CompareTag("Enemy")) {   // If the (me) enemy is tagged "Enemy", and therefore, me not is Corpse.
                                if (target.CompareTag("Corpse")) {
                                    Debug.Log("Corpse Detected");
                                    enemies.SetEnemyStatus(EnemyManager.AlertStatus.looking, target);
                                }
                            }
                        }
                    }
                }*/
            }
        }
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while (!stopFoV) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void SleepingGuard() {
        //alerted = enemies.currentStatus == EnemyManager.AlertStatus.alert || enemies.currentStatus == EnemyManager.AlertStatus.looking;

        if (lazyGuard) {
            if (!alerted) {
                if (isAwake) {
                    if (viewRadius < maxViewRadius) {
                        viewRadius += Time.deltaTime;
                    }
                    else if (awakeTime > 0) {
                        awakeTime -= Time.deltaTime;
                    }
                    else {
                        isAwake = false;
                        isSpleeping = true;
                        sleepingTime = maxSleepingTime;
                    }
                }
                if (isSpleeping) {
                    if (viewRadius > minViewRadius) {
                        viewRadius -= Time.deltaTime * 2;
                    }
                    else if (sleepingTime > 0) {
                        sleepingTime -= Time.deltaTime;
                    }
                    else {
                        isSpleeping = false;
                        isAwake = true;
                        awakeTime = maxAwakeTime;
                    }
                }
            }
            else if (viewRadius < maxViewRadius) {
                viewRadius += Time.deltaTime * 8f;
            }
        }
    }

    // Store the raycast information.
    public struct ViewCastInfo {
        public bool hitting;
        public Vector3 point;
        public float distance;  // The distance from the origin to the hit point.
        public float angle;

        public ViewCastInfo(bool _hitting, Vector3 _point, float _distance, float _angle) {
            hitting = _hitting;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo {
        public Vector3 pointA, pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    // This is for the next version of this script.
    public void StopView() {
        stopFoV = true;
        viewMeshFilter.mesh = null;
        enabled = false;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") && !stopFoV) {

            Vector3 direction = enemies.player.transform.position - (transform.position + transform.up);
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < viewAngle / 2f) {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction.normalized, out hit, viewRadius, targetMask)) { //obstacleMask / targetMask
                    if (hit.collider.CompareTag("Player")) {
                        Debug.Log("Player Detected");
                        _playerDetected = true;
                        //enemies.SetEnemyStatus(EnemyManager.AlertStatus.alert, other.transform);
                    }
                    else if (_playerDetected) {
                        _playerDetected = false;
                    }
                }
                Debug.DrawRay(transform.position, direction.normalized * viewRadius, Color.red);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            _playerDetected = false;
        }
    }
}
