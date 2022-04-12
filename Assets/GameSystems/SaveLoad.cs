using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad
{
    public enum Code
    {
        Success,
        Failed,
        NotFound,
        AlreadyExist,
        CannotOpen,
        Empty,
    }

    public static bool IsAlreadyExist(string path)
    {
        try
        {
            if (File.Exists(path)) return true;
            else return false;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
        catch (DirectoryNotFoundException)
        {
            return false;
        }
    }

    public static void CreateDataDerectory()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/PlayerData/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/PlayerData/");
        }
    }

    public static Code SaveData<T>(T data, string path)
    {
        try
        {
            string json = JsonUtility.ToJson(data);

            if (json.Equals("{}"))
            {
                Debug.Log("[Save Error] Json returned null, try again.");
            }

            string fullPath = Application.persistentDataPath + path;
            FileStream stream = new(fullPath, FileMode.Create);
            BinaryFormatter formatter = new();

            formatter.Serialize(stream, json);
            stream.Close();
            Debug.Log("Data Save path: " + fullPath);
            return Code.Success;
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("[Save Error] File was not found: " + e.Message);
            return Code.NotFound;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("[Save Error] Directory was not found: " + e.Message);
            return Code.NotFound;
        }
        catch (IOException e)
        {
            Debug.Log("[Save Error] File could not be opened: " + e.Message);
            return Code.CannotOpen;
        }
    }

    public static Code LoadData<T>(T data, string path)
    {
        try
        {
            string fullPath = Application.persistentDataPath + path;
            if (File.Exists(fullPath))
            {
                FileStream stream = new(fullPath, FileMode.Open);
                BinaryFormatter formatter = new();

                string json = formatter.Deserialize(stream) as string;

                JsonUtility.FromJsonOverwrite(json, data);

                stream.Close();
                Debug.Log("Data Load path: " + fullPath);
                return Code.Success;
            }
            else return Code.NotFound;

        }
        catch (FileNotFoundException e)
        {
            Debug.Log("[Load Error] File was not found: " + e.Message);
            return Code.NotFound;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("[Load Error] Directory was not found: " + e.Message);
            return Code.NotFound;
        }
        catch (IOException e)
        {
            Debug.Log("[Load Error] File could not be opened: " + e.Message);
            return Code.CannotOpen;
        }
    }
}
