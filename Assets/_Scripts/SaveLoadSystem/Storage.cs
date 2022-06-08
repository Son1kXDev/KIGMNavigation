using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Storage
{
    private string filePath;
    private BinaryFormatter formatter;

    public Storage()
    {
        var directory = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try { filePath = directory + "/settings.save"; } catch { }
        InitBinaryFormatter();
    }

    private void InitBinaryFormatter()
    {
        formatter = new BinaryFormatter();
        var selector = new SurrogateSelector();

        var v3Surrogate = new Vector3SerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);

        formatter.SurrogateSelector = selector;
    }

    public object Load(object saveDataByDefault)
    {
        if (!File.Exists(filePath))
        {
            if (saveDataByDefault != null)
            {
                Save(saveDataByDefault);
            }
            return saveDataByDefault;
        }

        var file = File.Open(filePath, FileMode.Open);
        try
        {
            var savedData = formatter.Deserialize(file);
            file.Close();
            return savedData;
        }
        catch (System.Exception) { Debug.Log("error"); return saveDataByDefault; }
    }

    public void Save(object saveData)
    {
        var file = File.Create(filePath);

        formatter.Serialize(file, saveData);
        file.Close();
    }
}