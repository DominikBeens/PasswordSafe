using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class TestSaver : MonoBehaviour
{

    public static TestSaver instance;

    public static SaveData saveData;

    public DataCreationManager dataCreationManager;
    public LoginManager loginManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (File.Exists(Application.persistentDataPath + "/SaveData.xml"))
        {
            saveData = Load();
        }
        else
        {
            saveData = new SaveData();
            Save(saveData);
        }

        if (saveData.password != null)
        {
            loginManager.enterPasswordPanel.SetActive(true);
        }
        else
        {
            loginManager.createPasswordPanel.SetActive(true);
        }
    }

    public void Save(SaveData toSave)
    {
        var serializer = new XmlSerializer(typeof(SaveData));
        using (var stream = new FileStream(Application.persistentDataPath + "/SaveData.xml", FileMode.Create))
        {
            //DataToSave();
            serializer.Serialize(stream, toSave);
        }
    }

    public SaveData Load()
    {
        var serializer = new XmlSerializer(typeof(SaveData));
        using (var stream = new FileStream(Application.persistentDataPath + "/SaveData.xml", FileMode.Open))
        {
            return serializer.Deserialize(stream) as SaveData;
        }
    }

    public void DataToSave()
    {

    }

    public void SetLoadedSaveData()
    {
        for (int i = 0; i < saveData.dataFolders.Count; i++)
        {
            dataCreationManager.CreateNewDataFolder(saveData.dataFolders[i]);
        }

        StructureManager.instance.homeHeaderBackground.color = saveData.homeHeaderBackgroundColor;
        StructureManager.instance.homeBackground.color = saveData.homeBackgroundColor;
        StructureManager.instance.newFolderBackground.color = saveData.newDataFolderBackgroundColor;
        StructureManager.instance.newInfoBlockBackground.color = saveData.newDataBlockBackgroundColor;

        //inside folder
        StructureManager.instance.optionsBackground.color = saveData.homeBackgroundColor;
    }

    public void OnApplicationQuit()
    {
        Save(saveData);
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            Save(saveData);
        }
    }
}
