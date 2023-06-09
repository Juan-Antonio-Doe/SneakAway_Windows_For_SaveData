using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manager que se encarga de guardar y cargar la informaci�n de los objetos en el juego.
/// </summary>

public static class JsonManager {

    //Generamos un jObject para guardar la informaci�n serializada m�s abajo
    static JObject jSaveGame = new JObject();

#if UNITY_EDITOR
    [MenuItem("Tools/JSON/Delete save")]
    public static void DeleteSave() {
        string saveFilePath = Application.persistentDataPath + "/jsonSneakAway.sav";
        if (File.Exists(saveFilePath)) {
            File.Delete(saveFilePath);
        }
    }
#endif

    #region Save/Load Player & Enemies Data Methods
    public static void SaveGame(PlayerManager curPlayer, EnemyV2[] enemies) {
        /// Enemies

        //Combinamos los objetos serializados (es decir los jSonString generados) y lo guardamos en el archivo de guardado
        for (int i = 0; i < enemies.Length; i++) {
            //Guardamos en una referencia el enemigo actual que estamos leyendo
            EnemyV2 curEnemy = enemies[i];
            //Generamos un jObject pasandole el enemigo concreto serializado
            JObject serializedEnemy = curEnemy.Serialize();
            //En el objecto jSon archivo de guardado, a�adimos la informaci�n que queremos de los objetos serializados
            jSaveGame.Add(curEnemy.name, serializedEnemy);
        }

        /// Player

        //Generamos un jObject pasandole el enemigo concreto serializado
        JObject serializedPlayer = curPlayer.Serialize();
        //En el objecto jSon archivo de guardado, a�adimos la informaci�n que queremos de los objetos serializados
        jSaveGame.Add(curPlayer.name, serializedPlayer);

        //Ruta donde queremos guardar la informaci�n
        string saveFilePath = Application.persistentDataPath + "/jsonSneakAway.sav";

        //Creamos un array de bytes para guardar el array que nos devuelve el m�todo Encrypt para que pueda ser usado
        byte[] encryptSavegame = Encrypt(jSaveGame.ToString());
        //Escribimos esta informaci�n en el archivo de guardado, ya encriptada la informaci�n en su ruta 
        File.WriteAllBytes(saveFilePath, encryptSavegame);
        //Muestra la ruta del archivo por consola
        Debug.Log("Saving to: " + saveFilePath);
    }

    public static void LoadGame(PlayerManager curPlayer, EnemyV2[] enemies) {
        //Ruta de donde queremos leer la informaci�n
        string saveFilePath = Application.persistentDataPath + "/jsonSneakAway.sav";
        //Muestra la ruta del archivo por consola
        Debug.Log("Loading from: " + saveFilePath);

        if (File.Exists(saveFilePath)) {
            //Creamos un array con la informaci�n encriptada recibida
            byte[] decryptedSavegame = File.ReadAllBytes(saveFilePath);
            //Creamos un array donde guardar la informaci�n desencriptada recibida
            string jsonString = Decrypt(decryptedSavegame);

            //Generamos un jObject al que le pasamos la informaci�n del jSon
            JObject jSaveGame = JObject.Parse(jsonString);

            /// Enemies

            for (int i = 0; i < enemies.Length; i++) {
                //Cargamos en una referencia el enemigo actual que estamos leyendo
                EnemyV2 curEnemy = enemies[i];
                //Generamos un string para cargar la informaci�n sacada del archivo de guardado para esa instancia
                string enemyJsonString = jSaveGame[curEnemy.name].ToString();
                //Llamamos al m�todo que deserializa la informaci�n obtenida
                curEnemy.Deserialize(enemyJsonString);
            }

            /// Player

            //Generamos un string para cargar la informaci�n sacada del archivo de guardado para esa instancia
            string playerJsonString = jSaveGame[curPlayer.name].ToString();
            //Llamamos al m�todo que deserializa la informaci�n obtenida
            curPlayer.Deserialize(playerJsonString);
        }
        else {
            Debug.Log("Save file not found in " + saveFilePath);
        }
    }
    #endregion

    #region Encryption/Decryption methods
    /*
     * PARA ENCRIPTAR Y DESENCRIPTAR LA INFORMACI�N DEL ARCHIVO DE GUARDADO
     */

    //Clave generada para la encriptaci�n en formato bytes, 16 posiciones
    static byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    //Vector de inicializaci�n para la clave
    static byte[] _initializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    //Encriptamos los datos del archivo de guardado que le pasaremos en un string
    static byte[] Encrypt(string message) {
        //Usamos esta librer�a que nos permitir� a trav�s de una referencia crear un encriptador de la informaci�n
        AesManaged aes = new AesManaged();
        //Para usar este encriptador le pasamos tanto la clave como el vector de inicializaci�n que hemos creado nosotros arriba
        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _initializationVector);
        //Lugar en memoria donde guardamos la informaci�n encriptada
        MemoryStream memoryStream = new MemoryStream();
        //Con esta referencia podremos escribir en el MemoryStream de arriba la informaci�n ya encriptada usando el encriptador con sus claves que ya hab�amos creado
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        //Con el StreamWriter podemos escribir en el archivo la informaci�n encriptada, que se habr� guardado en el MemoryStream
        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        //Usando todo lo anterior, guardamos en el archivo de guardado el json que le pasamos por par�metro, haciendo el siguiente proceso: recibimos el string, lo encriptamos, queda guardado en la memoria reservada para la encriptaci�n
        streamWriter.WriteLine(message);

        //Una vez hemos usado estas referencias las cerramos para evitar problemas de guardado o corrupci�n del archivo o de la propia encriptaci�n
        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        //Por �ltimo el m�todo devolver� esta informaci�n que reside en el hueco de memoria con la informaci�n encriptada, convertida esta informaci�n en array de bytes
        return memoryStream.ToArray();
    }

    //Generamos un m�todo que nos devuelva la informaci�n del archivo de guardado desencriptada
    static string Decrypt(byte[] message) {
        //Usamos esta librer�a que nos permitir� a trav�s de una referencia crear un desencriptador de la informaci�n
        AesManaged aes = new AesManaged();
        //Para usar este desencriptador le pasamos tanto la clave como el vector de inicializaci�n que hemos creado nosotros arriba
        ICryptoTransform decrypter = aes.CreateDecryptor(_key, _initializationVector);
        //Lugar en memoria donde guardamos la informaci�n desencriptada
        MemoryStream memoryStream = new MemoryStream(message);
        //Con esta referencia podremos escribir en el MemoryStream de arriba la informaci�n ya desencriptada usando el desencriptador con sus claves que ya hab�amos creado
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        //Con el StreamReader podemos leer del archivo la informaci�n desencriptada, que se habr� guardado en el MemoryStream
        StreamReader streamReader = new StreamReader(cryptoStream);

        //Usando todo lo anterior, cargamos del archivo de guardado el json que le pasamos por par�metro, haciendo el siguiente proceso: recibimos el string, lo desencriptamos, queda guardado en la memoria reservada para la desencriptaci�n
        string decryptedMessage = streamReader.ReadToEnd();

        //Una vez hemos usado estas referencias las cerramos para evitar problemas de guardado o corrupci�n del archivo o de la propia encriptaci�n
        streamReader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        //Por �ltimo el m�todo devolver� esta informaci�n que reside en el hueco de memoria con la informaci�n desencriptada, convertida esta en un string
        return decryptedMessage;
    }
    #endregion
}
