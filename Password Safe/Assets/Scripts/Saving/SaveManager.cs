using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;

    public static SaveData saveData;
    public static AppSettings appSettings;

    public DataCreationManager dataCreationManager;
    public LoginManager loginManager;

    [System.Serializable]
    public struct SerializableColor
    {
        public float r, g, b, a;

        public SerializableColor(float r, float g, float b, float a)
        {
            this.r = r; this.g = g; this.b = b; this.a = a;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        CheckSavedData();
        LoginManager.instance.OpenLoginPanel(appSettings);
    }

    private void CheckSavedData()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.xml"))
        {
            saveData = Load("/SaveData.xml");
        }
        else
        {
            saveData = new SaveData();
            Save(saveData, "/SaveData.xml");
        }

        if (File.Exists(Application.persistentDataPath + "/AppSettings.xml"))
        {
            appSettings = LoadAppSettings("/AppSettings.xml");
        }
        else
        {
            appSettings = new AppSettings();
            SaveAppSettings(appSettings);
        }
    }

    public void Save(SaveData toSave, string path)
    {
        var serializer = new XmlSerializer(typeof(SaveData));
        using (var stream = new FileStream(Application.persistentDataPath + path, FileMode.Create))
        {
            serializer.Serialize(stream, toSave);
        }
    }

    public void SaveAppSettings(AppSettings toSave)
    {
        var serializer = new XmlSerializer(typeof(AppSettings));
        using (var stream = new FileStream(Application.persistentDataPath + "/AppSettings.xml", FileMode.Create))
        {
            serializer.Serialize(stream, toSave);
        }
    }

    public void SaveToCloudButton(GameObject loadObject)
    {
        loadObject.SetActive(true);

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://utility-logic-194015.appspot.com");
        StorageReference saveData_ref = storage_ref.Child("SaveData");

        // Data in memory
        byte[] saveDataBytes = ObjectToByteArray(saveData);

        // Upload the files
        saveData_ref.PutBytesAsync(saveDataBytes)
          .ContinueWith((Task<StorageMetadata> task) =>
          {
              if (task.IsFaulted || task.IsCanceled)
              {
                  Debug.Log(task.Exception.ToString());
                  loadObject.SetActive(false);
                  // Uh-oh, an error occurred!
              }
              else
              {
                  // Metadata contains file metadata such as size, content-type, and download URL.
                  StorageMetadata metadata = task.Result;
                  //string download_url = metadata.DownloadUrl.ToString();
                  //string download_url = saveData_ref.GetDownloadUrlAsync().Result.AbsolutePath;
                  //Debug.Log("Finished uploading...");
                  //Debug.Log("download url = " + download_url);
                  loadObject.SetActive(false);
                  StructureManager.instance.NewNotification("Saved Data");
              }
          });
    }

    private byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
        {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    private Object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        Object obj = (Object)binForm.Deserialize(memStream);

        return obj;
    }

    public void RestoreFromCloudButton(GameObject loadObject)
    {
        //loadObject.SetActive(true);

        //if (saveData.googleDriveSaveFileId == null || saveData.googleDriveSaveFileId == "")
        //{
        //    StartCoroutine(RestoreFromGoogleDriveSaveFileWithName(loadObject));
        //    return;
        //}

        //StartCoroutine(RestoreFromGoogleDriveSaveFileWithID(loadObject));
    }

    //private IEnumerator RestoreFromGoogleDriveSaveFileWithName(GameObject loadObject)
    //{
    //    var request = GoogleDriveFiles.List();
    //    yield return request.Send();

    //    string saveFileId = "";

    //    for (int i = 0; i < request.ResponseData.Files.Count; i++)
    //    {
    //        if (request.ResponseData.Files[i].Name == "PS_SaveData")
    //        {
    //            saveFileId = request.ResponseData.Files[i].Id;
    //            break;
    //        }
    //    }

    //    if (saveFileId == "")
    //    {
    //        loadObject.SetActive(false);
    //        StopCoroutine(RestoreFromGoogleDriveSaveFileWithName(loadObject));
    //    }

    //    var request2 = GoogleDriveFiles.Download(saveFileId);
    //    yield return request2.Send();

    //    string destination = Application.persistentDataPath + "/SaveData.xml";

    //    File.Delete(destination);
    //    File.WriteAllBytes(destination, request2.ResponseData.Content);

    //    saveData = Load();

    //    for (int i = 0; i < dataCreationManager.folderHolder.childCount; i++)
    //    {
    //        Destroy(dataCreationManager.folderHolder.GetChild(i).gameObject);
    //    }

    //    SetLoadedSaveData();

    //    loadObject.SetActive(false);
    //    StructureManager.instance.NewNotification("Restored Data");
    //}

    //private IEnumerator RestoreFromGoogleDriveSaveFileWithID(GameObject loadObject)
    //{
    //    var request = GoogleDriveFiles.Download(saveData.googleDriveSaveFileId);
    //    yield return request.Send();

    //    string destination = Application.persistentDataPath + "/SaveData.xml";

    //    File.Delete(destination);
    //    File.WriteAllBytes(destination, request.ResponseData.Content);

    //    saveData = Load();

    //    for (int i = 0; i < dataCreationManager.folderHolder.childCount; i++)
    //    {
    //        Destroy(dataCreationManager.folderHolder.GetChild(i).gameObject);
    //    }

    //    SetLoadedSaveData();

    //    loadObject.SetActive(false);
    //    StructureManager.instance.dataSavePanel.SetActive(false);
    //    StructureManager.instance.NewNotification("Restored Data");
    //}

    public SaveData Load(string path)
    {
        var serializer = new XmlSerializer(typeof(SaveData));
        using (var stream = new FileStream(Application.persistentDataPath + path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as SaveData;
        }
    }

    public AppSettings LoadAppSettings(string path)
    {
        var serializer = new XmlSerializer(typeof(AppSettings));
        using (var stream = new FileStream(Application.persistentDataPath + path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as AppSettings;
        }
    }

    private void InitFirebaseSDK()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set a flag here indiciating that Firebase is ready to use by your
                // application.
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void SetLoadedSaveData()
    {
        InitFirebaseSDK();

        StructureManager.instance.SetDefaultColorsFromSaveData(saveData);
        //StructureManager.instance.SetColorsFromSaveData(saveData);

        for (int i = 0; i < saveData.dataFolders.Count; i++)
        {
            dataCreationManager.CreateNewDataFolder(saveData.dataFolders[i]);
        }
    }

    public void SaveAppColors()
    {
        saveData.homeHeaderBackgroundColor = new SerializableColor(StructureManager.instance.homeHeaderBackground.color.r, StructureManager.instance.homeHeaderBackground.color.g, StructureManager.instance.homeHeaderBackground.color.b, StructureManager.instance.homeHeaderBackground.color.a);
        saveData.homeBackgroundColor = new SerializableColor(StructureManager.instance.homeBackground.color.r, StructureManager.instance.homeBackground.color.g, StructureManager.instance.homeBackground.color.b, StructureManager.instance.homeBackground.color.a);
        saveData.newDataFolderBackgroundColor = new SerializableColor(StructureManager.instance.newFolderBackground.color.r, StructureManager.instance.newFolderBackground.color.g, StructureManager.instance.newFolderBackground.color.b, StructureManager.instance.newFolderBackground.color.a);
        saveData.newDataBlockBackgroundColor = new SerializableColor(StructureManager.instance.newInfoBlockBackground.color.r, StructureManager.instance.newInfoBlockBackground.color.g, StructureManager.instance.newInfoBlockBackground.color.b, StructureManager.instance.newInfoBlockBackground.color.a);
    }

    public void OnApplicationQuit()
    {
        Save(saveData, "/SaveData.xml");
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            Save(saveData, "/SaveData.xml");
        }
    }
}
