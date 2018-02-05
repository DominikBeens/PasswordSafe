using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataCreationManager : MonoBehaviour
{

    public static DataCreationManager instance;

    public Vector2 folderHolderDefaultPos;
    public Transform folderHolder;
    public GameObject folder;

    public Vector2 infoBlockHolderDefaultPos;
    public Transform infoBlockHolder;
    public GameObject infoBlock;

    public GameObject infoInputField;
    public GameObject titleInputField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        folderHolderDefaultPos = new Vector2(folderHolder.GetComponent<RectTransform>().offsetMin.y, folderHolder.GetComponent<RectTransform>().offsetMax.y);
        infoBlockHolderDefaultPos = new Vector2(infoBlockHolder.GetComponent<RectTransform>().offsetMin.y, infoBlockHolder.GetComponent<RectTransform>().offsetMax.y);
    }

    public void CreateNewDataFolder(DataFolder dataFolder)
    {
        GameObject newDataFolder = Instantiate(folder, folderHolder.position, Quaternion.identity);
        newDataFolder.transform.SetParent(folderHolder);
        newDataFolder.transform.localScale = Vector3.one;

        if (dataFolder == null)
        {
            newDataFolder.GetComponent<DataFolderHolder>().myDataFolder = new DataFolder
            {
                iD = newDataFolder.GetInstanceID(),
                color = newDataFolder.GetComponent<DataFolderHolder>().customizableImage.color
            };

            SaveManager.saveData.dataFolders.Add(newDataFolder.GetComponent<DataFolderHolder>().myDataFolder);
        }
        else
        {
            newDataFolder.GetComponent<DataFolderHolder>().myDataFolder = dataFolder;
        }

        newDataFolder.GetComponent<DataFolderHolder>().Initialize();
    }

    public void CreateNewDataBlock(DataBlock dataBlock)
    {
        GameObject newDataBlock = Instantiate(infoBlock, infoBlockHolder.position, Quaternion.identity);
        newDataBlock.transform.SetParent(infoBlockHolder);
        newDataBlock.transform.localScale = Vector3.one;

        if (dataBlock == null)
        {
            newDataBlock.GetComponent<DataBlockHolder>().myDataBlock = new DataBlock
            {
                iD = newDataBlock.GetInstanceID(),
                color = newDataBlock.GetComponent<DataBlockHolder>().customizableImage.color
            };

            StructureManager.currentDataFolder.myDataBlocks.Add(newDataBlock.GetComponent<DataBlockHolder>().myDataBlock);
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
