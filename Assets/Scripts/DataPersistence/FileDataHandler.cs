using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Utils;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // Use Path.Combine to account for differnt OS's having different parh separators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialization
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                CustomLog.Log(CustomLog.CustomLogType.SERIALISATION, "Deserialization successful");
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // Use Path.Combine to account for differnt OS's having different parh separators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try 
        {
            // Create the directory the file will be written on if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the data object into JSON
            string dataToStore = JsonUtility.ToJson(data, true);
            CustomLog.Log(CustomLog.CustomLogType.SERIALISATION, "Serialization successful");

            // Write the serialize data to the file
            // "using" to ensure stream is closed
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
