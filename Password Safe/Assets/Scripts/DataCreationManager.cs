using UnityEngine;

public class DataCreationManager : MonoBehaviour
{

    public static DataCreationManager instance;

    [Header("Data Prefabs")]
    public GameObject folder;
    public GameObject infoBlock;
    [Space(10)]
    public GameObject infoInputField;
    public GameObject titleInputField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CreateNewDataFolder(DataFolder dataFolder)
    {
        GameObject newDataFolder = Instantiate(folder, StructureManager.instance.folderHolder.position, Quaternion.identity);
        newDataFolder.transform.SetParent(StructureManager.instance.folderHolder);
        newDataFolder.transform.localScale = Vector3.one;

        if (dataFolder == null)
        {
            DataFolderHolder current = newDataFolder.GetComponent<DataFolderHolder>();
            current.myDataFolder = new DataFolder
            {
                iD = newDataFolder.GetInstanceID(),
                color = new SaveManager.SerializableColor(current.customizableImage.color * 255)
            };

            SaveManager.saveData.dataFolders.Add(current.myDataFolder);
        }
        else
        {
            newDataFolder.GetComponent<DataFolderHolder>().myDataFolder = dataFolder;
        }

        newDataFolder.GetComponent<DataFolderHolder>().Initialize();
    }

    public void CreateNewDataBlock(DataBlock dataBlock)
    {
        GameObject newDataBlock = Instantiate(infoBlock, StructureManager.instance.infoBlockHolder.position, Quaternion.identity);
        newDataBlock.transform.SetParent(StructureManager.instance.infoBlockHolder);
        newDataBlock.transform.localScale = Vector3.one;

        if (dataBlock == null)
        {
            DataBlockHolder current = newDataBlock.GetComponent<DataBlockHolder>();
            current.myDataBlock = new DataBlock
            {
                iD = newDataBlock.GetInstanceID(),
                color = new SaveManager.SerializableColor(current.customizableImage.color * 255)
            };

            StructureManager.currentDataFolder.myDataBlocks.Add(current.myDataBlock);
        }
        else
        {
            newDataBlock.GetComponent<DataBlockHolder>().myDataBlock = dataBlock;
        }

        newDataBlock.GetComponent<DataBlockHolder>().Initialize();
    }

    public void CreateNewDataField(int type, DataBlockHolder dataBlockHolder)
    {
        if (type == 0)
        {
            GameObject newInfoDataField = Instantiate(infoInputField, dataBlockHolder.inputFieldHolder.position, Quaternion.identity);
            newInfoDataField.transform.SetParent(dataBlockHolder.inputFieldHolder);
            newInfoDataField.transform.localScale = Vector3.one;

            newInfoDataField.GetComponent<DataFieldHolder>().myDataField = new DataField
            {
                iD = newInfoDataField.GetInstanceID(),
                type = (int)newInfoDataField.GetComponent<InputFieldType>().type
            };

            dataBlockHolder.myDataBlock.myDataFields.Add(newInfoDataField.GetComponent<DataFieldHolder>().myDataField);

        }
        else if (type == 1)
        {
            GameObject newTitleDataField = Instantiate(titleInputField, dataBlockHolder.inputFieldHolder.position, Quaternion.identity);
            newTitleDataField.transform.SetParent(dataBlockHolder.inputFieldHolder);
            newTitleDataField.transform.localScale = Vector3.one;

            newTitleDataField.GetComponent<DataFieldHolder>().myDataField = new DataField()
            {
                iD = newTitleDataField.GetComponent<DataFieldHolder>().myDataField.iD,
                type = (int)newTitleDataField.GetComponent<InputFieldType>().type
            };

            dataBlockHolder.myDataBlock.myDataFields.Add(newTitleDataField.GetComponent<DataFieldHolder>().myDataField);
        }

        dataBlockHolder.IncreaseSize();
        StartCoroutine(dataBlockHolder.CloseOptions());
    }
}
