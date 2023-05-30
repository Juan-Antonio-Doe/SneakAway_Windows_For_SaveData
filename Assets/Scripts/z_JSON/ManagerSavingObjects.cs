using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //Usar StreamWriter y StreamReader
using Newtonsoft.Json.Linq; //Para poder usar Json.net y estructuras de datos
using System.Security.Cryptography; //Libería para encriptación y desencriptación de información

/**
 * Manager que se encarga de guardar y cargar la información de los objetos en la escena.
 */

public class ManagerSavingObjects : MonoBehaviour {
    [field: SerializeField] private GameObject player;
    [field: SerializeField] private GameObject[] enemiesGO;

    [field: SerializeField, ReadOnlyField] private PlayerManager curPlayer;
    [field: SerializeField, ReadOnlyField] private EnemyV2[] enemies;

    private void OnValidate() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (enemiesGO == null || enemiesGO.Length == 0) {
            GameObject aux = GameObject.FindObjectOfType<EnemyManager>().gameObject;
            if (aux != null) {
                foreach (Transform child in aux.transform) {
                    if (child.gameObject.tag == "Enemy") {
                        enemiesGO = new GameObject[aux.transform.childCount];
                        for (int i = 0; i < enemiesGO.Length; i++) {
                            enemiesGO[i] = aux.transform.GetChild(i).gameObject;
                        }
                    }
                }
            }
        }

        if (player != null) {
            curPlayer = player.GetComponent<PlayerManager>();
        }
        if (enemiesGO != null && enemiesGO.Length > 0) {
            enemies = new EnemyV2[enemiesGO.Length];
            for (int i = 0; i < enemiesGO.Length; i++) {
                enemies[i] = enemiesGO[i].GetComponent<EnemyV2>();
            }
        }
    }

    private void Start() {
        Load();
    }

    public void Save() {
        JsonManager.SaveGame(curPlayer, enemies);
    }

    public void Load() {
        JsonManager.LoadGame(curPlayer, enemies);
    }

}
