using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityGoogleDrive;

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

    private IEnumerator Get()
    {
        var request = GoogleDriveFiles.Get(saveData.googleDriveSaveFileId);
        yield return request.Send();

        print(request.ResponseData.Name);
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
        Save(saveData);

        if (saveData.googleDriveSaveFileId == null || saveData.googleDriveSaveFileId == "")
        {
            var file = new UnityGoogleDrive.Data.File() { Name = "PS_SaveData", Content = File.ReadAllBytes(Application.persistentDataPath + "/SaveData.xml") };
            var request = GoogleDriveFiles.Create(file);
            request.Send();
            StartCoroutine(ShowRequestProgress(request, loadObject));

            saveData.googleDriveSaveFileId = request.ResponseData.Id;
        }
        else
        {
            var file = new UnityGoogleDrive.Data.File() { Name = "PS_SaveData", Content = File.ReadAllBytes(Application.persistentDataPath + "/SaveData.xml") };
            var request = GoogleDriveFiles.Update(saveData.googleDriveSaveFileId, file);
            request.Send();
            StartCoroutine(ShowRequestProgress(request, loadObject));
        }
    }

    public void RestoreFromGoogleDriveButton(GameObject loadObject)
    {
        if (saveData.googleDriveSaveFileId == null || saveData.googleDriveSaveFileId == "")
        {
            return;
        }

        var request = GoogleDriveFiles.Download(saveData.googleDriveSaveFileId);
        request.Send().OnDone += RestoreFromGoogleDriveSaveFile;

        StartCoroutine(ShowRequestProgress(request, loadObject));
    }

    private void RestoreFromGoogleDriveSaveFile(UnityGoogleDrive.Data.File file)
    {
        //File.Replace(file.Name, "SaveData.xml", "SaveData.xml.bac");
        //print("done");
    }

    private IEnumerator ShowRequestProgress(GoogleDriveFiles.UpdateRequest request, GameObject loadObject)
    {
        loadObject.SetActive(true);

        while (request.IsRunning)
        {
            yield return null;
        }

        loadObject.SetActive(false);
    }

    private IEnumerator ShowRequestProgress(GoogleDriveFiles.CreateRequest request, GameObject loadObject)
    {
        loadObject.SetActive(true);

        while (request.IsRunning)
        {
            yield return null;
        }

        loadObject.SetActive(false);
    }

    private IEnumerator ShowRequestProgress(GoogleDriveFiles.GetRequest request, GameObject loadObject)
    {
        loadObject.SetActive(true);

        while (request.IsRunning)
        {
            yield return null;
        }

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
