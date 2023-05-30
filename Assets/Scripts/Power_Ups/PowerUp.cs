using UnityEngine;

/// <summary>
/// PowerUp common properties.
/// Type, name, sprite, duration, cooldown, prefab to instantiate, etc.
/// </summary>

[CreateAssetMenu(fileName = "Name_PowerUp", menuName = "PowerUp", order = 80)]
public class PowerUp : ScriptableObject {

    public enum PowerUpType {
        Trap,
        EMP,
        Clock,
        Map,
        None
    }

    [field: SerializeField] public string puName { get; set; }
    [field: SerializeField] public PowerUpType powerUpType { get; set; }
    [field: SerializeField] public Sprite sprite { get; set; }  // Icon to show in the UI
    [field: SerializeField] public float duration { get; set; }
    [field: SerializeField] public float cooldown { get; set; }
    [field: SerializeField, Tooltip("Prefab with additional functionality.\n Use only if necessary.")
        ] public GameObject prefab { get; set; }
    public float cooldownTimer { get; set; }
    public bool isOnCooldown { get; set; }
    public bool isActive { get; set; }
    public bool isUnlocked { get; set; }

    public bool isUnlimited { get { return duration < 0; } private set { } }
    
}