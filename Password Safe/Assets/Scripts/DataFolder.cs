using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataFolder : MonoBehaviour
{

    public int iD;
    public InputField folderName;

    public List<InfoBlockSave> myInfoBlockSaves = new List<InfoBlockSave>();

    public GameObject optionsPanel;
    public Animator anim;

    public Image customizableImage;

    public void OpenFolder()
    {
        ResetPanelScroll();
        StructureManager.currentDataFolder = GetComponent<DataFolder>();

        if (DataCreationManager.instance.infoBlockHolder.childCount > 0)
        {
            for (int i = 0; i < DataCreationManager.instance.infoBlockHolder.childCount; i++)
            {
                Destroy(DataCreationManager.instance.infoBlockHolder.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < myInfoBlockSaves.Count; i++)
        {
            GameObject newInfoBlock = Instantiate(DataCreationManager.instance.infoBlock, DataCreationManager.instance.infoBlockHolder.position, Quaternion.identity);
            newInfoBlock.transform.SetParent(DataCreationManager.instance.infoBlockHolder);

            InfoBlock infoBlock = newInfoBlock.GetComponent<InfoBlock>();

            infoBlock.iD = myInfoBlockSaves[i].iD;
            infoBlock.nameInputField.text = SaveManager.Decrypt(myInfoBlockSaves[i].name);
            infoBlock.myInputFieldSaves = myInfoBlockSaves[i].inputFieldSaves;

            infoBlock.customizableImage.color = new Color32(System.Convert.ToByte(myInfoBlockSaves[i].colorR), 
                                                            System.Convert.ToByte(myInfoBlockSaves[i].colorG), 
                                                            System.Convert.ToByte(myInfoBlockSaves[i].colorB), 255);

            for (int ii = 0; ii < infoBlock.myInputFieldSaves.Count; ii++)
            {
                if (infoBlock.myInputFieldSaves[ii].type == 0)
                {
                    GameObject newInfoInputField = Instantiate(DataCreationManager.instance.infoInputField, infoBlock.inputFieldHolder.position, Quaternion.identity);
                    newInfoInputField.transform.SetParent(infoBlock.inputFieldHolder);

                    newInfoInputField.GetComponent<InputField>().text = SaveManager.Decrypt(infoBlock.myInputFieldSaves[ii].text);
                    newInfoInputField.GetComponent<DataInputField>().iD = infoBlock.myInputFieldSaves[ii].iD;
                }
                else if (infoBlock.myInputFieldSaves[ii].type == 1)
                {
                    GameObject newTitleInputField = Instantiate(DataCreationManager.instance.titleInputField, infoBlock.inputFieldHolder.position, Quaternion.identity);
                    newTitleInputField.transform.SetParent(infoBlock.inputFieldHolder);

                    newTitleInputField.GetComponent<InputField>().text = SaveManager.Decrypt(infoBlock.myInputFieldSaves[ii].text);
                    newTitleInputField.GetComponent<DataInputField>().iD = infoBlock.myInputFieldSaves[ii].iD;
                }

                infoBlock.IncreaseSize();
            }
        }

        StructureManager.instance.infoBlockPanel.SetActive(true);
    }

    public void CustomizeButton()
    {
        ColorPicker.imageToChangeColor = customizableImage;
        StructureManager.instance.colorPickerPanel.SetActive(true);

        StartCoroutine(CloseOptions());
    }

    public void OpenOptions()
    {
        if (!optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptionsButton()
    {
        StartCoroutine(CloseOptions());
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
