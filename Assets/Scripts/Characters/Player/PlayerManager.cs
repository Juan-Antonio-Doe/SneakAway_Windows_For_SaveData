using Newtonsoft.Json.Linq;
using Nrjwolf.Tools.AttachAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Manage the player.
/// </summary>

[AddComponentMenu("Scripts/ESI/Characters/Player/Player Manager")]
public class PlayerManager : MonoBehaviour {

    [field: Header("AutoAttach on Editor properties")]
    [field: SerializeField, FindObjectOfType, ReadOnlyField] public PowerUpsManager powerUpsManager { get; private set; }
    [field: SerializeField, FindObjectOfType, ReadOnlyField] private GameOverUI_Manager gameOverManager { get; set; }
    [field: SerializeField, FindObjectOfType, ReadOnlyField] public InputHandler input { get; private set; }
    [field: SerializeField, GetComponent, ReadOnlyField] public Animator anim { get; private set; }
    [field: SerializeField, GetComponent, ReadOnlyField] public PlayerSimpleMovement touchInput { get; private set; }
    [field: SerializeField, GetComponent, ReadOnlyField] public NavMeshAgent agent { get; private set; }

    //[field: Header("Other properties")]

    public bool dead { get; set; }
    public bool hasPowerUp { get; set; }

    float defaultSpeed { get; set; }

    [field: Header("Debug")]
    [field: SerializeField] private Text _debugText { get; set; }

    private void Start() {
        defaultSpeed = agent.speed;
    }

    private void Update() {
        if (PauseManager.onPause)
            return;

        if (hasPowerUp) {
            if (input.playerActions.UsePowerUp_0.WasPressedThisFrame()) {
                //Debug.Log("Used: PowerUp 0");
                //_debugText.text = $"Used: PowerUp 0 \nInput: {input.playerActions.UsePowerUp_0} \n";
                powerUpsManager.InstantiateTrapPowerUp(0, transform.position);
            }
            if (input.playerActions.UsePowerUp_1.WasPressedThisFrame()) {
                //_debugText.text += $"Used: PowerUp 1 \nInput: {input.playerActions.UsePowerUp_1}";
                //Debug.Log("Used: PowerUp 1");
                powerUpsManager.ActivateEMPPowerUp(1);
            }
            if (input.playerActions.UsePowerUp_2.WasPressedThisFrame()) {
                //_debugText.text += $"Used: PowerUp 2 \nInput: {input.playerActions.UsePowerUp_2}";
                //Debug.Log("Used: PowerUp 2");
                powerUpsManager.ActivateMapPowerUp(2);
            }
        }

        // Move slower when carrying a corpse
        if (input.MoveCorpseIsPressed) {
            agent.speed = defaultSpeed / 2;
        }
        else {
            agent.speed = defaultSpeed;
        }
    }

    public void KillPlayer() {
        dead = true;
        touchInput.DisableInput();
        anim.SetTrigger("Kill");
        StartCoroutine(WaitAndShowGameOver(2f));
    }

    IEnumerator WaitAndShowGameOver(float seconds) {
        yield return new WaitForSeconds(seconds);
        gameOverManager.ShowGameOver();
    }

    #region Save/Load Methods
    public class PlayerSaveData {
        //Variables para serializar
        //public float[] position;
        public Vector3 pos;

        //Constructor de la clase
        public PlayerSaveData(Transform transform) {
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
        PlayerSaveData data = new PlayerSaveData(transform);

        //Creamos un string que guardará el jSon
        string jsonString = JsonUtility.ToJson(data);
        //Creamos un objeto en el jSon
        JObject retVal = JObject.Parse(jsonString);
        //Al ser un método de tipo, debe devolver este tipo
        return retVal;
    }

    //Tendremos que deserializar la información recibida
    public void Deserialize(string jsonString) {
        PlayerSaveData data = new PlayerSaveData(transform);
        //La información recibida del archivo de guardado sobreescribirá los campos oportunos del jsonString
        JsonUtility.FromJsonOverwrite(jsonString, data);

        // Restauramos los datos guardados en el objeto
        transform.position = data.pos;
    }
    #endregion
}
