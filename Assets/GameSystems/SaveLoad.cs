using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad
{
    public static void SaveData<T>(T data, string path)
    {
        try
        {
            string json = JsonUtility.ToJson(data);

            if (json.Equals("{}"))
            {
                Debug.Log("[Save Error] Json returned null, try again.");
                return;
            }
            FileStream fileStream = new(path, FileMode.Create);
            BinaryFormatter formatter = new();

            formatter.Serialize(fileStream, json);
            fileStream.Close();
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("[Save Error] File was not found: " + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("[Save Error] Directory was not found: " + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("[Save Error] File could not be opened:" + e.Message);
        }
    }

    public static T LoadData<T>(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                FileStream stream = new(path, FileMode.Open);
                BinaryFormatter formatter = new();

                string json = formatter.Deserialize(stream) as string;

                T data = JsonUtility.FromJson<T>(json);

                stream.Close();
                return data;
            }

        }
        catch (FileNotFoundException e)
        {
            Debug.Log("[Load Error] File was not found: " + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("[Load Error] Directory was not found: " + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("[Load Error] File could not be opened:" + e.Message);
        }
        return default;
    }
}
