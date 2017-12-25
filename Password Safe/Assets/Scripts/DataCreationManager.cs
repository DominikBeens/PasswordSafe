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

    public void CreateNewFolder()
    {
        GameObject newFolder = Instantiate(folder, folderHolder.position, Quaternion.identity);
        newFolder.transform.SetParent(folderHolder);
        newFolder.GetComponent<DataFolder>().iD = newFolder.GetInstanceID();

        StructureManager.instance.folderSaves.Add(SaveManager.instance.CreateFolderSave(newFolder.GetComponent<DataFolder>()));
    }

    public void CreateFolderFromSave(FolderSave save)
    {
        GameObject newFolder = Instantiate(folder, folderHolder.position, Quaternion.identity);
        newFolder.transform.SetParent(folderHolder);

        DataFolder folderComponent = newFolder.GetComponent<DataFolder>();

        folderComponent.iD = save.iD;
        folderComponent.folderName.text = SaveManager.Decrypt(save.name);

        folderComponent.myInfoBlockSaves = save.infoBlockSaves;

        Color32 folderColor = new Color32(System.Convert.ToByte(save.colorR), System.Convert.ToByte(save.colorG), System.Convert.ToByte(save.colorB), 255);

        folderComponent.customizableImage.color = folderColor;
    }

    public void CreateNewInfoBlock()
    {
        GameObject newInfoBlock = Instantiate(infoBlock, infoBlockHolder.position, Quaternion.identity);
        newInfoBlock.transform.SetParent(infoBlockHolder);
        newInfoBlock.GetComponent<InfoBlock>().iD = newInfoBlock.GetInstanceID();

        StructureManager.currentDataFolder.myInfoBlockSaves.Add(SaveManager.instance.CreateInfoBlockSave(newInfoBlock.GetComponent<InfoBlock>()));

        SaveManager.instance.SaveAllFolders();
    }

    public void CreateNewInputField(int type, InfoBlock infoBlock)
    {
        if (type == 0)
        {
            GameObject newInfoInputField = Instantiate(infoInputField, infoBlock.inputFieldHolder.position, Quaternion.identity);
            newInfoInputField.transform.SetParent(infoBlock.inputFieldHolder);

            newInfoInputField.GetComponent<DataInputField>().iD = newInfoInputField.GetInstanceID();

            InputFieldSave inputFieldSave = new InputFieldSave()
            {
                iD = newInfoInputField.GetComponent<DataInputField>().iD,
                text = newInfoInputField.GetComponent<InputField>().text,
                type = (int)newInfoInputField.GetComponent<InputFieldType>().type
            };

            infoBlock.myInputFieldSaves.Add(inputFieldSave);
        }
        else if (type == 1)
        {
            GameObject newTitleInputField = Instantiate(titleInputField, infoBlock.inputFieldHolder.position, Quaternion.identity);
            newTitleInputField.transform.SetParent(infoBlock.inputFieldHolder);

            newTitleInputField.GetComponent<DataInputField>().iD = newTitleInputField.GetInstanceID();

            InputFieldSave inputFieldSave = new InputFieldSave()
            {
                iD = newTitleInputField.GetComponent<DataInputField>().iD,
                text = newTitleInputField.GetComponent<InputField>().text,
                type = (int)newTitleInputField.GetComponent<InputFieldType>().type
            };

            infoBlock.myInputFieldSaves.Add(inputFieldSave);
        }

        infoBlock.IncreaseSize();
        StartCoroutine(infoBlock.CloseOptions());
    }
}
