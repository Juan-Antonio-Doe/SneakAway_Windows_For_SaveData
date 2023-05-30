using Newtonsoft.Json.Linq;
using Nrjwolf.Tools.AttachAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Manage the enemy behaviour.
/// </summary>

[AddComponentMenu("Scripts/ESI/Characters/Enemies/Enemy V2")]
public class EnemyV2 : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, GetComponent] private NavMeshAgent agent { get; set; }
    [field: SerializeField, FindObjectOfType] public EnemyManager enemies { get; private set; }
    //[field: SerializeField, GetComponent, ReadOnlyField] public FieldOfView myFoV { get; private set; }
    [field: SerializeField, GetComponent, ReadOnlyField] public VisionCone myFoV { get; private set; }

    [field: SerializeField, GetComponentInChildren, ReadOnlyField] private Slider _staminaBar { get; set; }
    public Slider StaminaBar { get { return _staminaBar; } }

    [field: SerializeField, GetComponent] private SphereCollider _sphereCollider { get; set; }
    public SphereCollider SphereCollider { get { return _sphereCollider; } }
    [field: SerializeField, GetComponent] private Animator _Anim { get; set; }
    public Animator Anim { get { return _Anim; } }
    [field: SerializeField, GetComponent] private CapsuleCollider _capsuleCollider { get; set; }
    public CapsuleCollider CapsuleCollider { get { return _capsuleCollider; } }

    [field: Header("Other Editor properties")]
    [field: SerializeField, Tooltip("Parent of the empty objects to use as waypoints.")
        ] private Transform waypointsParent { get; set; }
    [field: SerializeField, ReadOnlyField] private List<Transform> waypoints = new List<Transform>();   // Normal patrol waypoints.
    public List<Transform> Waypoints { get { return waypoints; } }

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
                // Assing the waypoints on the inspector.
                if (waypointsParent != null && (waypoints.Count == 0 || waypoints.Count != waypointsParent.childCount)) {

                    waypoints.Clear();
                    foreach (Transform child in waypointsParent) {
                        waypoints.Add(child);
                    }
                }
                else if (waypointsParent == null && waypoints.Count > 0) {
                    waypoints.Clear();
                }
        }
#endif
    }
    //#endif

    [field: Header("Other properties")]

    [field: SerializeField] private float patrolSpeedModifier = 0f;
    public float PatrolSpeedModifier { get { return patrolSpeedModifier; } }
    [field: SerializeField, ReadOnlyField] private float _currentStamina { get; set; } = 3f;
    [field: SerializeField] private float _maxStamina { get; set; } = 3f;
    [field: SerializeField, Tooltip("Enable if you want the enemy to stay still and simulate falling asleep over time.")
        ] private bool _lazyGuard;
    public bool LazyGuard { get { return _lazyGuard; } }
    [field: SerializeField] private float _maxAwakeTime = 3f;
    public float MaxAwakeTime { get { return _maxAwakeTime; } }
    [field: SerializeField] private float _maxSleepingTime = 1f;
    public float MaxSleepingTime { get { return _maxSleepingTime; } }

    [field: SerializeField, Range(0.5f, 10f)] private float _maxAlertTimer { get; set; } = 5f;
    public float MaxAlertTimer { get { return _maxAlertTimer; } }

    private bool _isBeingNeutralized { get; set; }  // If the enemy is being neutralized.
    [field: SerializeField, ReadOnlyField] public bool isDead { get; set; }
    [field: SerializeField, ReadOnlyField] public bool canMoveCorpse { get; set; }

    private int _currentWaypointIndex = 0;
    public int CurrentWaypointIndex { get { return _currentWaypointIndex; } set { _currentWaypointIndex = value; } }
    private bool _invertPatrol;  // If the enemy is going to the next waypoint or to the previous one.
    public bool InvertPatrol { get { return _invertPatrol; } set { _invertPatrol = value; } }
    
    //[field: SerializeField] public float waitEndPatrolTime { get; private set; } = 1f;  // Unused for now.
    
    public enum PatrolTypeEnum { Cyclic, PingPong }
    [field: SerializeField, Tooltip("Type of route that the enemy will take." +
        "\n· Cyclic: The enemy will return to the first waypoint when he reaches the last one." +
        "\n· Ping Pong: The enemy will do the reverse path when it reaches the last waypoint.")
        ] private PatrolTypeEnum _patrolType { get; set; } = PatrolTypeEnum.PingPong;
    public PatrolTypeEnum PatrolType { get { return _patrolType; } }

    private List<Transform> allWaypoints = new List<Transform>();   // All the waypoints of the scene.
    public List<Transform> AllWaypoints { get { return allWaypoints; } }
    private int currentLookingWaypoint = 0; // The waypoint that the enemy is looking at.
    public int CurrentLookingWaypoint { get { return currentLookingWaypoint; } set { currentLookingWaypoint = value; } }

    private EnemyState currentState { get; set; }

    [field: SerializeField, Tooltip("GameObjects that will change their color when the enemy dies.")
        ] private MeshRenderer[] _objectsToChangeColor { get; set; }
    public MeshRenderer[] ObjectsToChangeColor { get { return _objectsToChangeColor; } }
    [field: SerializeField, Tooltip("Material with the Dead Color")] private Material _deathMaterialColor { get; set; }
    public Material DeathMaterialColor { get { return _deathMaterialColor; } }

    private void Start() {
        _currentStamina = _maxStamina;
        _staminaBar.maxValue = _maxStamina;
        _staminaBar.value = _maxStamina;
        _staminaBar.gameObject.SetActive(false);

        allWaypoints = enemies.allWaypoints;
        currentLookingWaypoint = Random.Range(0, allWaypoints.Count - 1);

        agent.autoBraking = false;  // Disable the speed reduction when the agent is close to the destination.

        currentState = new EnemyIdle(gameObject, this, agent, enemies.player, myFoV);
    }

    private void Update() {
        currentState = currentState.Process();

        //Debug.Log($"Current state: {currentState.GetType().Name}");

        // If the enemy is being neutralized, we reduce his stamina.
        if (_isBeingNeutralized && !isDead) {
            if (_currentStamina > 0) {
                _currentStamina -= Time.deltaTime;
                _staminaBar.value = _currentStamina;
            }
            else {
                KillEnemy();
            }
        }
    }

    public void KillEnemy(bool trapped=false) {
        _currentStamina = 0;
        _staminaBar.value = 0;
        _staminaBar.gameObject.SetActive(false);
        
        _isBeingNeutralized = false;
        isDead = true;

        currentState = new EnemyDead(gameObject, this, agent, enemies.player, myFoV);

        /*
         * Since TriggerEnter is only called once upon entering and the enemy can only die after entering, 
         * we enabling the MoveCorpse mechanic here for the first time.
         */
        if (!trapped) {
            canMoveCorpse = true;
            enemies.moveCorpseButton.SetActive(true);
        }
    }

    public void KillingEnemy() {
        _isBeingNeutralized = true;
        _staminaBar.gameObject.SetActive(true);
    }

    public void StopKillingEnemy() {
        if (_isBeingNeutralized) {
            _currentStamina = _maxStamina;
        }
        
        _isBeingNeutralized = false;
        _staminaBar.gameObject.SetActive(false);
    }

    #region Save/Load Methods
    public class EnemySaveData {
        //Variables para serializar
        public Vector3 pos;

        //Constructor de la clase
        public EnemySaveData(Transform transform) {
            //Rellenamos las variables con las que le pasamos por parámetro
            /*position[0] = transform.position.x;
            position[1] = transform.position.y;
            position[2] = transform.position.z;*/
            pos = transform.position;

        }
    }

    //Crearemos un objeto serializable capaz de ser guardado
    public JObject Serialize() {
        //Instanciamos la clase anidada pasándole por parámetro las variables que queremos guardar
        EnemySaveData data = new EnemySaveData(transform);

        //Creamos un string que guardará el jSon
        string jsonString = JsonUtility.ToJson(data);
        //Creamos un objeto en el jSon
        JObject retVal = JObject.Parse(jsonString);
        //Al ser un método de tipo, debe devolver este tipo
        return retVal;
    }

    //Tendremos que deserializar la información recibida
    public void Deserialize(string jsonString) {
        EnemySaveData data = new EnemySaveData(transform);
        //La información recibida del archivo de guardado sobreescribirá los campos oportunos del jsonString
        JsonUtility.FromJsonOverwrite(jsonString, data);

        // Restauramos los valores del enemigo
        transform.position = data.pos;
    }
    #endregion
}
