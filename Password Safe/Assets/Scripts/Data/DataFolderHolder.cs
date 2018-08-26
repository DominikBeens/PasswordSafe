using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataFolderHolder : MonoBehaviour
{

    public DataFolder myDataFolder;

    public InputField nameInputField;

    public GameObject optionsPanel;
    public Animator anim;

    public Image customizableImage;

    public void OpenFolder()
    {
        StructureManager.instance.ResetInfoBlockScroll();
        StructureManager.currentDataFolder = myDataFolder;

        if (StructureManager.instance.infoBlockHolder.childCount > 0)
        {
            for (int i = 0; i < StructureManager.instance.infoBlockHolder.childCount; i++)
            {
                Destroy(StructureManager.instance.infoBlockHolder.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < myDataFolder.myDataBlocks.Count; i++)
        {
            GameObject newDataBlock = Instantiate(DataCreationManager.instance.infoBlock, StructureManager.instance.infoBlockHolder.position, Quaternion.identity);
            newDataBlock.transform.SetParent(StructureManager.instance.infoBlockHolder);
            newDataBlock.transform.localScale = Vector3.one;

            DataBlockHolder dataBlockHolder = newDataBlock.GetComponent<DataBlockHolder>();

            dataBlockHolder.myDataBlock = myDataFolder.myDataBlocks[i];

            for (int ii = 0; ii < dataBlockHolder.myDataBlock.myDataFields.Count; ii++)
            {
                if (dataBlockHolder.myDataBlock.myDataFields[ii].type == 0)
                {
                    GameObject newInfoDataField = Instantiate(DataCreationManager.instance.infoInputField, dataBlockHolder.inputFieldHolder.position, Quaternion.identity);
                    newInfoDataField.transform.SetParent(dataBlockHolder.inputFieldHolder);
                    newInfoDataField.transform.localScale = Vector3.one;

                    newInfoDataField.GetComponent<DataFieldHolder>().myDataField = dataBlockHolder.myDataBlock.myDataFields[ii];
                }
                else if (dataBlockHolder.myDataBlock.myDataFields[ii].type == 1)
                {
                    GameObject newTitleDataField = Instantiate(DataCreationManager.instance.titleInputField, dataBlockHolder.inputFieldHolder.position, Quaternion.identity);
                    newTitleDataField.transform.SetParent(dataBlockHolder.inputFieldHolder);
                    newTitleDataField.transform.localScale = Vector3.one;

                    newTitleDataField.GetComponent<DataFieldHolder>().myDataField = dataBlockHolder.myDataBlock.myDataFields[ii];
                }

            }

            dataBlockHolder.SetSize(dataBlockHolder.myDataBlock.myDataFields.Count);
            dataBlockHolder.Initialize();
        }

        StructureManager.instance.infoBlockPanel.SetActive(true);
    }

    public void OnValueChanged()
    {
        myDataFolder.folderName = nameInputField.text;
    }

    public void Initialize()
    {
        if (myDataFolder != null)
        {
            nameInputField.text = myDataFolder.folderName;

            customizableImage.color = new Color(myDataFolder.color.r / 255, myDataFolder.color.g / 255, myDataFolder.color.b / 255, myDataFolder.color.a / 255);
        }
    }

    public void CustomizeButton()
    {
        ColorPicker.instance.StartCustomizing(customizableImage, gameObject);
        StartCoroutine(CloseOptions());
    }

    public void ToggleOptionsButton()
    {
        if (!optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CloseOptions());
        }
    }

    public IEnumerator CloseOptions()
    {
        anim.SetTrigger("Close");

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        optionsPanel.SetActive(false);
    }

    public void DeleteFolder()
    {
        StructureManager.toDelete = gameObject;
        StructureManager.instance.OpenDeletePanel(0);
    }
}
