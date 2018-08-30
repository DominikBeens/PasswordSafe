using Firebase.Storage;
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

    [SerializeField] private GameObject loadingDataOverlay;

    [System.Serializable]
    public struct SerializableColor
    {
        public float r, g, b, a;

        public SerializableColor(float r, float g, float b, float a)
        {
            this.r = r; this.g = g; this.b = b; this.a = a;
        }

        public SerializableColor(Color color)
        {
            r = color.r; g = color.g; b = color.b; a = color.a;
        }

        public Color GetColor()
        {
            return new Color(r, g, b, a);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InitFirebaseSDK();
        LoadAppSettings();
        LoginManager.instance.OpenLoginPanel(appSettings);
    }

    private void InitFirebaseSDK()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready to use.
            }
            else
            {
                // Firebase Unity SDK is not ready to use.
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    private void LoadAppSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/AppSettings.xml"))
        {
            appSettings = LoadAppSettingsFromFile();
        }
        else
        {
            appSettings = new AppSettings();
            SaveAppSettingsToFile(appSettings);
        }

        LoadColorsFromAppSettings();
    }

    private AppSettings LoadAppSettingsFromFile()
    {
        var serializer = new XmlSerializer(typeof(AppSettings));
        using (var stream = new FileStream(Application.persistentDataPath + "/AppSettings.xml", FileMode.Open))
        {
            return serializer.Deserialize(stream) as AppSettings;
        }
    }

    public void SaveAppSettingsToFile(AppSettings toSave)
    {
        var serializer = new XmlSerializer(typeof(AppSettings));
        using (var stream = new FileStream(Application.persistentDataPath + "/AppSettings.xml", FileMode.Create))
        {
            serializer.Serialize(stream, toSave);
        }
    }

    public void SaveToCloudButton(GameObject loadObject = null)
    {
        if (loadObject != null)
        {
            loadObject.SetActive(true);
        }

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://utility-logic-194015.appspot.com");
        StorageReference saveData_ref = storage_ref.Child(LoginManager.user.UserId);

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
                  StructureManager.instance.NewNotification("Failed To Save Data");
              }
              else
              {
                  // Metadata contains file metadata such as size, content-type, and download URL.
                  //StorageMetadata metadata = task.Result;

                  if (loadObject != null)
                  {
                      loadObject.SetActive(false);
                  }

                  StructureManager.instance.NewNotification("Saved Data");
              }
          });
    }

    public void LoadFromCloud()
    {
        loadingDataOverlay.SetActive(true);

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://utility-logic-194015.appspot.com");
        StorageReference saveData_ref = storage_ref.Child(LoginManager.user.UserId);

        const long maxAllowedSize = 1 * 1024 * 1024;
        saveData_ref.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.ToString());
                Debug.Log("Created new saveData");

                saveData = new SaveData();
                loadingDataOverlay.SetActive(false);
            }
            else
            {
                byte[] fileContents = task.Result;
                Debug.Log("Finished downloading!");

                saveData = (SaveData)ByteArrayToObject(fileContents);
                SetLoadedSaveData();
                loadingDataOverlay.SetActive(false);
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

    private object ByteArrayToObject(byte[] bytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(bytes, 0, bytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        object obj = binForm.Deserialize(memStream);

        return obj;
    }

    public void DeleteSavedDataButton(GameObject loadObject)
    {
        loadObject.SetActive(true);

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://utility-logic-194015.appspot.com");
        StorageReference saveData_ref = storage_ref.Child(LoginManager.user.UserId);

        saveData_ref.DeleteAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                for (int i = 0; i < StructureManager.instance.folderHolder.childCount; i++)
                {
                    Destroy(StructureManager.instance.folderHolder.GetChild(i).gameObject);
                }
                saveData.dataFolders = new List<DataFolder>();
                StructureManager.instance.infoBlockPanel.SetActive(false);

                loadObject.SetActive(false);
                StructureManager.instance.NewNotification("Successfully Deleted Saved Data");
            }
            else
            {
                StructureManager.instance.NewNotification("Failed To Delete Saved Data");
            }
        });
    }

    private void SetLoadedSaveData()
    {
        for (int i = 0; i < saveData.dataFolders.Count; i++)
        {
            DataCreationManager.instance.CreateNewDataFolder(saveData.dataFolders[i]);
        }
    }

    public void SaveAppColors()
    {
        appSettings.homeHeaderBackgroundColor = new SerializableColor(StructureManager.instance.homeHeaderBackground.color);
        appSettings.homeBackgroundColor = new SerializableColor(StructureManager.instance.homeBackground.color);
        appSettings.newDataFolderBackgroundColor = new SerializableColor(StructureManager.instance.newFolderBackground.color);
        appSettings.newDataBlockBackgroundColor = new SerializableColor(StructureManager.instance.newInfoBlockBackground.color);
    }

    private void LoadColorsFromAppSettings()
    {
        // Cache StructureManager instance for better readability.
        StructureManager sm = StructureManager.instance;

        sm.homeHeaderBackground.color = appSettings.homeHeaderBackgroundColor.GetColor();
        sm.homeBackground.color = appSettings.homeBackgroundColor.GetColor();
        sm.newFolderBackground.color = appSettings.newDataFolderBackgroundColor.GetColor();
        sm.newInfoBlockBackground.color = appSettings.newDataBlockBackgroundColor.GetColor();
        //inside folder
        sm.optionsBackground.color = appSettings.homeBackgroundColor.GetColor();
    }

    public void LoadDefaultColorsFromAppSettings()
    {
        // Cache StructureManager instance for better readability.
        StructureManager sm = StructureManager.instance;

        sm.homeHeaderBackground.color = appSettings.defaultHomeHeaderBackgroundColor.GetColor();
        sm.homeBackground.color = appSettings.defaultHomeBackgroundColor.GetColor();
        sm.newFolderBackground.color = appSettings.defaultNewDataFolderBackgroundColor.GetColor();
        sm.newInfoBlockBackground.color = appSettings.defaultNewDataBlockBackgroundColor.GetColor();
        //inside folder
        sm.optionsBackground.color = appSettings.defaultHomeBackgroundColor.GetColor();
    }

    public void OnApplicationQuit()
    {
        if (!LoginManager.instance.loginAccountPanel.activeInHierarchy && !LoginManager.instance.createAccountPanel.activeInHierarchy)
        {
            SaveToCloudButton();
            SaveAppSettingsToFile(appSettings);
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            if (!LoginManager.instance.loginAccountPanel.activeInHierarchy && !LoginManager.instance.createAccountPanel.activeInHierarchy)
            {
                SaveToCloudButton();
                SaveAppSettingsToFile(appSettings);
            }
        }
    }
}
