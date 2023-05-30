using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This script is used to manage the enemies in the scene.
/// </summary>
/// Updated for EnemyV2.

[AddComponentMenu("Scripts/ESI/Characters/Enemies/Enemy Manager")]
public class EnemyManager : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, FindObjectOfType, ReadOnlyField] public PlayerManager player { get; private set; }

    
    [field: Header("Script properties")]
    [field: SerializeField] public GameObject moveCorpseButton { get; private set; }

    [field: Header("Debug")]
    [field: SerializeField, ReadOnlyField] private bool _communicationsDisabled { get; set; }
    public bool CommunicationsDisabled { get { return _communicationsDisabled; } set { _communicationsDisabled = value; } }

    // Used for alert to others enemies.
    [field: SerializeField, ReadOnlyField] private bool _playerDetected { get; set; }
    public bool PlayerDetected { get { return _playerDetected; } set { _playerDetected = value; } }
    [field: SerializeField, ReadOnlyField] private bool _corpseDetected { get; set; }
    public bool CorpseDetected { get { return _corpseDetected; } set { _corpseDetected = value; } }
    [field: SerializeField, ReadOnlyField] private bool _playerDetectedByCam { get; set; }
    public bool PlayerDetectedByCam { 
        get { return _playerDetectedByCam; } 
        set { 
            _playerDetectedByCam = value;
            if (value) {
                lastPlayerPositionKnowed = player.transform.position;
                lastPlayerPositionAlreadyCheked = false;
            }
        } 
    }
    public Vector3 lastPlayerPositionKnowed { get; private set; }
    public bool lastPlayerPositionAlreadyCheked { get; set; }


    // This is used to store the Waypoint objects in the scene to create a list with just the Transform component.
    //List<GameObject> _auxWaypoints { get; set; } = new List<GameObject>();   
    [field: SerializeField, ReadOnlyField] public List<Transform> allWaypoints { get; private set; } = new List<Transform>();
    //private int _waypointsCount { get; set; } = 0;

    private void OnValidate() {

        /*_auxWaypoints = GameObject.FindGameObjectsWithTag("enemyWaypoint").ToList();

        if (_waypointsCount != _auxWaypoints.Count) {
            _waypointsCount = _auxWaypoints.Count;
            
            allWaypoints.Clear();
            
            foreach (GameObject go in _auxWaypoints) {
                allWaypoints.Add(go.transform);
            }
        }*/

#if UNITY_EDITOR
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
        if (!isValidPrefabStage /*&& prefabConnected*/) {
            ValidateAssings();
        }
#endif
    }

    private void ValidateAssings() {
        //Variables que solo se verificaran cuando están en una escena
        if (moveCorpseButton == null)
            moveCorpseButton = GameObject.Find("Corpse_Btn");

        // Get all the waypoints in the scene in a simplified way.
        if (allWaypoints.Count == 0)
            allWaypoints = GameObject.FindGameObjectsWithTag("enemyWaypoint").Select(x => x.transform).ToList();
    }

    private void Start() {
        moveCorpseButton.SetActive(false);
    }

    /*private IEnumerator Start() {
        moveCorpseButton.SetActive(false);
        auxWaypoints = GameObject.FindGameObjectsWithTag("enemyWaypoint").ToList();

        yield return null;
        
        foreach (GameObject go in auxWaypoints) {
            allWaypoints.Add(go.transform);
        }
    }*/

    #region Old version
    /*[SerializeField] public enum AlertStatus { off, looking, alert }
      [field: SerializeField, ReadOnlyField] public AlertStatus currentStatus { get; private set; }
      [field: SerializeField, Range(0.5f, 10f)] public float maxAlertTimer { get; private set; } = 5f;
      [field: SerializeField, ReadOnlyField] public float alertTimer { get; private set; }*/

    /*private void Update() {
        if (currentStatus == AlertStatus.alert) {
            if (alertTimer < maxAlertTimer) {
                alertTimer += Time.deltaTime;
            }
            else {
                alertTimer = 0;
                currentStatus = AlertStatus.looking;
            }
        }
    }

    public void SetEnemyStatus(AlertStatus status, Transform target) {
        this.currentStatus = status;
        this.target = target;
    }*/
    #endregion
}
