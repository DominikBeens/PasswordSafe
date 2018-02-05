using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityGoogleDrive;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;

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

    public void SaveToGoogleDriveButton(GameObject loadObject)
    {
        loadObject.SetActive(true);

        Save(saveData);

        if (saveData.googleDriveSaveFileId == null || saveData.googleDriveSaveFileId == "")
        {
            StartCoroutine(SaveToGoogleDrive(loadObject, false));
        }
        else
        {
            StartCoroutine(SaveToGoogleDrive(loadObject, true));
        }
    }

    private IEnumerator SaveToGoogleDrive(GameObject loadObject, bool hasGoogleDriveSaveFile)
    {
        var file = new UnityGoogleDrive.Data.File() { Name = "PS_SaveData", Content = File.ReadAllBytes(Application.persistentDataPath + "/SaveData.xml") };

        if (!hasGoogleDriveSaveFile)
        {
            var request = GoogleDriveFiles.Create(file);
            yield return request.Send();

            saveData.googleDriveSaveFileId = request.ResponseData.Id;
        }
        else
        {
            var request = GoogleDriveFiles.Update(saveData.googleDriveSaveFileId, file);
            yield return request.Send();
        }

        loadObject.SetActive(false);
    }

    public void RestoreFromGoogleDriveButton(GameObject loadObject)
    {
        loadObject.SetActive(true);

        if (saveData.googleDriveSaveFileId == null || saveData.googleDriveSaveFileId == "")
        {
            StartCoroutine(RestoreFromGoogleDriveSaveFileWithName(loadObject));
            return;
        }

        StartCoroutine(RestoreFromGoogleDriveSaveFileWithID(loadObject));
    }

    private IEnumerator RestoreFromGoogleDriveSaveFileWithName(GameObject loadObject)
    {
        var request = GoogleDriveFiles.List();
        yield return request.Send();

        string saveFileId = "";

        for (int i = 0; i < request.ResponseData.Files.Count; i++)
        {
            if (request.ResponseData.Files[i].Name == "PS_SaveData")
            {
                saveFileId = request.ResponseData.Files[i].Id;
                break;
            }
        }

        if (saveFileId == "")
        {
            loadObject.SetActive(false);
            StopCoroutine(RestoreFromGoogleDriveSaveFileWithName(loadObject));
        }

        var request2 = GoogleDriveFiles.Download(saveFileId);
        yield return request2.Send();

        string destination = Application.persistentDataPath + "/SaveData.xml";

        File.Delete(destination);
        File.WriteAllBytes(destination, request2.ResponseData.Content);

        saveData = Load();

        for (int i = 0; i < dataCreationManager.folderHolder.childCount; i++)
        {
            Destroy(dataCreationManager.folderHolder.GetChild(i).gameObject);
        }

        SetLoadedSaveData();

        loadObject.SetActive(false);
    }

    private IEnumerator RestoreFromGoogleDriveSaveFileWithID(GameObject loadObject)
    {
        var request = GoogleDriveFiles.Download(saveData.googleDriveSaveFileId);
        yield return request.Send();

        string destination = Application.persistentDataPath + "/SaveData.xml";

        File.Delete(destination);
        File.WriteAllBytes(destination, request.ResponseData.Content);

        saveData = Load();

        for (int i = 0; i < dataCreationManager.folderHolder.childCount; i++)
        {
            Destroy(dataCreationManager.folderHolder.GetChild(i).gameObject);
        }

        SetLoadedSaveData();

        loadObject.SetActive(false);
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
