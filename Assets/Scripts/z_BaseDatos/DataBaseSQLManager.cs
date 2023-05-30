using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manage the connection with the database saving and loading the score.
/// </summary>

public static class DataBaseSQLManager {

    static string rutaDB;
    static string strConexion;
    static string DBFileName = "Scores.db";

    static IDbConnection dbConnection;
    static IDbCommand dbCommand;
    static IDataReader reader;

    /// <summary>
    /// Save the score in the database.
    /// </summary>
    public static void SaveScore(int score) {
        // Open the database.
        OpenDB(DBFileName);

        // Create the update statement.
        string sql = "UPDATE Scores SET Score = @score";

        // Prepare the statement.
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = sql;

        // Create a SqliteParameter object.
        SqliteParameter parameter = new SqliteParameter("@score", SqlDbType.Int);
        parameter.Value = score;

        // Add the parameter to the command.
        dbCommand.Parameters.Add(parameter);

        // Execute the statement.
        dbCommand.ExecuteNonQuery();

        CerrarDB();
    }

    /// <summary>
    /// Get the score from the database.
    /// </summary>
    public static int LoadScore() {
        // Open the database.
        OpenDB(DBFileName);

        // Create the select statement.
        string sql = "SELECT Score FROM Scores";

        // Prepare the statement.
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = sql;

        // Execute the statement.
        dbCommand.ExecuteNonQuery();

        // Get the score from the database.
        object scoreObject = dbCommand.ExecuteScalar();

        // Close the database.
        CerrarDB();

        // Check if the value is null.
        if (scoreObject == null) {
            return 0;
        }

        // Convert the value to an int.
        //Debug.Log($"##scoreObject: {scoreObject}");
        int score = Convert.ToInt32(scoreObject);

        return score;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Reset all the data from the tables in the database.
    /// </summary>
    [MenuItem("Tools/DataBases/Reset Data")]
    public static void ResetDatabase() {
        SaveScore(0);
        PlayerPrefs.DeleteKey("TotalScore");
    }
#endif

    #region DataBase Connection Methods
    //Método para cerrar la DB
    static void CerrarDB() {
        // Cerrar las conexiones
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    static void OpenDB(string DBFileName) {

#if UNITY_EDITOR
        string dbPath = string.Format(@"Assets/StreamingAssets/{0}", DBFileName);
#else
            // check if file exists in Application.persistentDataPath
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DBFileName);
       
            if (!File.Exists(filepath))
            {
                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->
           
#if UNITY_ANDROID
                var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DBFileName);  // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                var loadDb = Application.dataPath + "/Raw/" + DBFileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DBFileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
           
#elif UNITY_WINRT
                var loadDb = Application.dataPath + "/StreamingAssets/" + DBFileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#endif
           
                Debug.Log("Database written");
            }
       
            var dbPath = filepath;
#endif
        //_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

        //open db connection
        strConexion = "URI=file:" + dbPath;
        Debug.Log("Stablishing connection to: " + strConexion);
        dbConnection = new SqliteConnection(strConexion);
        dbConnection.Open();
    }
    #endregion
}
