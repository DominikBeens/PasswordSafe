using System.Collections;
using System.Collections.Generic;
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
        ResetPanelScroll();
        StructureManager.currentDataFolder = myDataFolder;

        if (DataCreationManager.instance.infoBlockHolder.childCount > 0)
        {
            for (int i = 0; i < DataCreationManager.instance.infoBlockHolder.childCount; i++)
            {
                Destroy(DataCreationManager.instance.infoBlockHolder.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < myDataFolder.myDataBlocks.Count; i++)
        {
            GameObject newDataBlock = Instantiate(DataCreationManager.instance.infoBlock, DataCreationManager.instance.infoBlockHolder.position, Quaternion.identity);
            newDataBlock.transform.SetParent(DataCreationManager.instance.infoBlockHolder);

            DataBlockHolder dataBlock = newDataBlock.GetComponent<DataBlockHolder>();

            dataBlock.myDataBlock = myDataFolder.myDataBlocks[i];

            for (int ii = 0; ii < dataBlock.myDataBlock.myDataFields.Count; ii++)
            {
                if (dataBlock.myDataBlock.myDataFields[ii].type == 0)
                {
                    GameObject newInfoDataField = Instantiate(DataCreationManager.instance.infoInputField, dataBlock.inputFieldHolder.position, Quaternion.identity);
                    newInfoDataField.transform.SetParent(dataBlock.inputFieldHolder);

                    newInfoDataField.GetComponent<DataFieldHolder>().myDataField = dataBlock.myDataBlock.myDataFields[ii];
                }
                else if (dataBlock.myDataBlock.myDataFields[ii].type == 1)
                {
                    GameObject newTitleDataField = Instantiate(DataCreationManager.instance.titleInputField, dataBlock.inputFieldHolder.position, Quaternion.identity);
                    newTitleDataField.transform.SetParent(dataBlock.inputFieldHolder);

                    newTitleDataField.GetComponent<DataFieldHolder>().myDataField = dataBlock.myDataBlock.myDataFields[ii];
                }

                dataBlock.IncreaseSize();
            }

            dataBlock.Initialize();
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

            customizableImage.color = myDataFolder.color;
        }
    }

    public void CustomizeButton()
    {
        ColorPicker.imageToChangeColor = customizableImage;
        ColorPicker.dataObjectToSaveTo = gameObject;
        StructureManager.instance.colorPickerPanel.SetActive(true);

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

    public void ResetPanelScroll()
    {
        DataCreationManager.instance.infoBlockHolder.GetComponent<RectTransform>().offsetMin = new Vector2(0, DataCreationManager.instance.infoBlockHolderDefaultPos.x);
        DataCreationManager.instance.infoBlockHolder.GetComponent<RectTransform>().offsetMax = new Vector2(0, DataCreationManager.instance.infoBlockHolderDefaultPos.y);
    }
}
