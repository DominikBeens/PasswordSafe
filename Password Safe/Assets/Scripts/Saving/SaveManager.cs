//using System.Collections.Generic;
//using System.IO;
//using System.Xml.Serialization;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Text;
//using System.Security.Cryptography;
//using System;

//public class SaveManager : MonoBehaviour
//{

//    public static SaveManager instance;

//    public SaveData saveData;

//    public DataCreationManager dataCreationManager;
//    public LoginManager loginManager;

//    private void Awake()
//    {
//        #region Singleton
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else if (instance != null)
//        {
//            Destroy(this);
//        }
//        #endregion

//        if (System.IO.File.Exists(Application.persistentDataPath + "/SaveData.xml"))
//        {
//            saveData = Load();
//        }
//        else
//        {
//            saveData = ScriptableObject.CreateInstance<SaveData>();
//            Save(saveData);
//        }

//        if (saveData.password != null)
//        {
//            loginManager.enterPasswordPanel.SetActive(true);
//        }
//        else
//        {
//            loginManager.createPasswordPanel.SetActive(true);
//        }
//    }

//    public void Save(SaveData toSave)
//    {
//        var serializer = new XmlSerializer(typeof(SaveData));
//        using (var stream = new FileStream(Application.persistentDataPath + "/SaveData.xml", FileMode.Create))
//        {
//            DataToSave();
//            serializer.Serialize(stream, toSave);
//        }
//    }

//    public SaveData Load()
//    {
//        var serializer = new XmlSerializer(typeof(SaveData));
//        using (var stream = new FileStream(Application.persistentDataPath + "/SaveData.xml", FileMode.Open))
//        {
//            return serializer.Deserialize(stream) as SaveData;
//        }
//    }

//    public void DataToSave()
//    {
//        //SaveOpenedFolder();
//        //SaveAllFolders();

//        saveData.dataFolders = StructureManager.instance.myDataFolders;

//        //SaveCustomizables();
//    }

//    public void SetLoadedSaveData()
//    {
//        StructureManager.instance.myDataFolders = saveData.dataFolders;

//        for (int i = 0; i < saveData.dataFolders.Count; i++)
//        {
//            dataCreationManager.CreateNewDataFolder(saveData.dataFolders[i]);
//        }

//        //LoadCustomizables();
//    }

//    //#region Saving
//    //public FolderSave CreateFolderSave(DataFolder folder)
//    //{
//    //    Color32 customizableImageColor = folder.customizableImage.color;

//    //    FolderSave save = new FolderSave()
//    //    {
//    //        name = Encrypt(folder.folderName.text),
//    //        iD = folder.iD,

//    //        colorR = customizableImageColor.r,
//    //        colorG = customizableImageColor.g,
//    //        colorB = customizableImageColor.b
//    //    };

//    //    List<InfoBlockSave> infoBlockSaves = new List<InfoBlockSave>();

//    //    if (folder.myInfoBlockSaves.Count != 0)
//    //    {
//    //        for (int i = 0; i < folder.myInfoBlockSaves.Count; i++)
//    //        {
//    //            infoBlockSaves.Add(folder.myInfoBlockSaves[i]);
//    //        }

//    //        save.infoBlockSaves = infoBlockSaves;
//    //    }

//    //    return save;
//    //}

//    //public InfoBlockSave CreateInfoBlockSave(InfoBlock infoBlock)
//    //{
//    //    Color32 customizableImageColor = infoBlock.customizableImage.color;

//    //    InfoBlockSave save = new InfoBlockSave()
//    //    {
//    //        iD = infoBlock.iD,
//    //        name = Encrypt(infoBlock.nameText.text),

//    //        colorR = customizableImageColor.r,
//    //        colorG = customizableImageColor.g,
//    //        colorB = customizableImageColor.b
//    //    };

//    //    List<InputFieldSave> inputFieldSaves = new List<InputFieldSave>();

//    //    for (int i = 1; i < infoBlock.inputFieldHolder.childCount; i++)
//    //    {
//    //        InputFieldSave inputFieldSave = new InputFieldSave()
//    //        {
//    //            iD = infoBlock.inputFieldHolder.GetChild(i).GetComponent<DataInputField>().iD,
//    //            text = Encrypt(infoBlock.inputFieldHolder.GetChild(i).GetComponent<InputField>().text),
//    //            type = (int)infoBlock.inputFieldHolder.GetChild(i).GetComponent<InputFieldType>().type
//    //        };

//    //        inputFieldSaves.Add(inputFieldSave);
//    //    }

//    //    save.inputFieldSaves = inputFieldSaves;

//    //    return save;
//    //}

//    //public void SaveAllFolders()
//    //{
//    //    List<FolderSave> saves = new List<FolderSave>();

//    //    for (int i = 0; i < dataCreationManager.folderHolder.childCount; i++)
//    //    {
//    //        saves.Add(CreateFolderSave(dataCreationManager.folderHolder.GetChild(i).GetComponent<DataFolder>()));
//    //    }

//    //    StructureManager.instance.folderSaves = saves;
//    //}

//    //public void SaveOpenedFolder()
//    //{
//    //    if (StructureManager.currentDataFolder != null)
//    //    {
//    //        List<InfoBlockSave> newInfoBlockSaves = new List<InfoBlockSave>();
//    //        for (int i = 0; i < StructureManager.currentDataFolder.myInfoBlockSaves.Count; i++)
//    //        {
//    //            newInfoBlockSaves.Add(CreateInfoBlockSave(dataCreationManager.infoBlockHolder.GetChild(i).GetComponent<InfoBlock>()));
//    //        }
//    //        StructureManager.currentDataFolder.myInfoBlockSaves = newInfoBlockSaves;
//    //    }
//    //}

//    //public void SaveCustomizables()
//    //{
//    //    Color32 color;

//    //    color = StructureManager.instance.homeHeaderBackground.color;
//    //    saveData.homeHeaderBackgroundColorR = color.r;
//    //    saveData.homeHeaderBackgroundColorG = color.g;
//    //    saveData.homeHeaderBackgroundColorB = color.b;

//    //    color = StructureManager.instance.homeBackground.color;
//    //    saveData.homeBackgroundColorR = color.r;
//    //    saveData.homeBackgroundColorG = color.g;
//    //    saveData.homeBackgroundColorB = color.b;

//    //    color = StructureManager.instance.newFolderBackground.color;
//    //    saveData.newFolderBackgroundColorR = color.r;
//    //    saveData.newFolderBackgroundColorG = color.g;
//    //    saveData.newFolderBackgroundColorB = color.b;

//    //    color = StructureManager.instance.newInfoBlockBackground.color;
//    //    saveData.newInfoBlockBackgroundColorR = color.r;
//    //    saveData.newInfoBlockBackgroundColorG = color.g;
//    //    saveData.newInfoBlockBackgroundColorB = color.b;

//    //    color = StructureManager.instance.optionsBackground.color;
//    //    saveData.optionsBackgroundColorR = color.r;
//    //    saveData.optionsBackgroundColorG = color.g;
//    //    saveData.optionsBackgroundColorB = color.b;
//    //}

//    //public void LoadCustomizables()
//    //{
//    //    Color32 color;

//    //    color = new Color32(System.Convert.ToByte(saveData.homeHeaderBackgroundColorR), System.Convert.ToByte(saveData.homeHeaderBackgroundColorG), System.Convert.ToByte(saveData.homeHeaderBackgroundColorB), 255);
//    //    StructureManager.instance.homeHeaderBackground.color = color;

//    //    color = new Color32(System.Convert.ToByte(saveData.homeBackgroundColorR), System.Convert.ToByte(saveData.homeBackgroundColorG), System.Convert.ToByte(saveData.homeBackgroundColorB), 255);
//    //    StructureManager.instance.homeBackground.color = color;

//    //    color = new Color32(System.Convert.ToByte(saveData.newFolderBackgroundColorR), System.Convert.ToByte(saveData.newFolderBackgroundColorG), System.Convert.ToByte(saveData.newFolderBackgroundColorB), 255);
//    //    StructureManager.instance.newFolderBackground.color = color;

//    //    color = new Color32(System.Convert.ToByte(saveData.newInfoBlockBackgroundColorR), System.Convert.ToByte(saveData.newInfoBlockBackgroundColorG), System.Convert.ToByte(saveData.newInfoBlockBackgroundColorB), 255);
//    //    StructureManager.instance.newInfoBlockBackground.color = color;

//    //    color = new Color32(System.Convert.ToByte(saveData.optionsBackgroundColorR), System.Convert.ToByte(saveData.optionsBackgroundColorG), System.Convert.ToByte(saveData.optionsBackgroundColorB), 255);
//    //    StructureManager.instance.optionsBackground.color = color;
//    //}

//    public void OnApplicationQuit()
//    {
//        Save(saveData);
//    }

//    public void OnApplicationPause(bool pause)
//    {
//        if (pause == true)
//        {
//            Save(saveData);
//        }
//    }
//    //#endregion

//    //#region Encryption
//    //// http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx

//    //public static string Encrypt(string toEncrypt)
//    //{
//    //    byte[] keyArray = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
//    //    // 256-AES key
//    //    byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
//    //    RijndaelManaged rDel = new RijndaelManaged()
//    //    {
//    //        BlockSize = 256,
//    //        Key = keyArray,
//    //        Mode = CipherMode.CBC,
//    //    };
//    //    rDel.IV = rDel.Key;

//    //    // better lang support
//    //    ICryptoTransform cTransform = rDel.CreateEncryptor();
//    //    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

//    //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
//    //}

//    //public static string Decrypt(string toDecrypt)
//    //{
//    //    byte[] keyArray = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
//    //    // AES-256 key
//    //    byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
//    //    RijndaelManaged rDel = new RijndaelManaged()
//    //    {
//    //        BlockSize = 256,
//    //        Key = keyArray,
//    //        Mode = CipherMode.CBC,
//    //    };
//    //    rDel.IV = rDel.Key;

//    //    // better lang support
//    //    ICryptoTransform cTransform = rDel.CreateDecryptor();
//    //    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

//    //    return Encoding.UTF8.GetString(resultArray);
//    //}
//    //#endregion
//}
