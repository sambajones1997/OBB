using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class ScoreClass : MonoBehaviour
{
    [SerializeField]
    public static int[] mHighScores = new int[5];

    public static void SaveScores()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/HighScores.OBB");
        binaryFormatter.Serialize(fileStream, mHighScores);
        fileStream.Close();
    }

    public static void Load(bool bLoadGraphics = true)
    {
        if (File.Exists(Application.persistentDataPath + "/HighScores.OBB"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/HighScores.OBB", FileMode.Open);
            mHighScores = (int[])binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
        }
        else
        {
            for (int i =0; i < 5; i++) mHighScores[i] = 0;
        }
    }
}
