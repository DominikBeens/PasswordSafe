using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureManager : MonoBehaviour
{

    public static StructureManager instance;
    public static DataFolder currentDataFolder;

    public List<FolderSave> folderSaves = new List<FolderSave>();

    public GameObject infoBlockPanel;

    [Header("Delete Panel")]
    public GameObject deletePanel;
    public Text deletePanelHeaderText;
    public Animator deletePanelAnim;
    [Space(10)]
    public GameObject clearDataPanel;

    public static GameObject toDelete;

    [Header("Options Panel")]
    public GameObject optionsPanel;

    [Header("Change Pass Panel")]
    public GameObject changePassPanel;
    public InputField oldPassText;
    public InputField newPassText;

    [Header("Color Picker")]
    public GameObject colorPickerPanel;

    [Header("Customize Picker")]
    public GameObject customizePanel;
    [Space(10)]
    public Button customizeHomeHeaderButton;
    public Image homeHeaderBackground;
    [Space(10)]
    public Button customizeHomeBackgroundButton;
    public Image homeBackground;
    [Space(10)]
    public Button customizeNewFolderHeaderButton;
    public Image newFolderBackground;
    [Space(10)]
    public Button customizeNewInfoBlockHeaderButton;
    public Image newInfoBlockBackground;
    [Space(10)]
    public Button customizeOptionsBackgroundButton;
    public Image optionsBackground;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (customizePanel.activeInHierarchy)
        {
            ChangeCustomizeButtonColors(customizeHomeHeaderButton, homeHeaderBackground);
            ChangeCustomizeButtonColors(customizeHomeBackgroundButton, homeBackground);
            ChangeCustomizeButtonColors(customizeNewFolderHeaderButton, newFolderBackground);
            ChangeCustomizeButtonColors(customizeNewInfoBlockHeaderButton, newInfoBlockBackground);
            ChangeCustomizeButtonColors(customizeOptionsBackgroundButton, optionsBackground);
        }
    }

    public void CreateNewFolder()
    {
        DataCreationManager.instance.CreateNewFolder();
    }

    public void CreateNewInfoBlock()
    {
        DataCreationManager.instance.CreateNewInfoBlock();
    }

    public void HomeButton()
    {
        infoBlockPanel.SetActive(false);

        if (currentDataFolder != null)
        {
            List<InfoBlockSave> newInfoBlockSaves = new List<InfoBlockSave>();
            for (int i = 0; i < currentDataFolder.myInfoBlockSaves.Count; i++)
            {
                newInfoBlockSaves.Add(SaveManager.instance.CreateInfoBlockSave(DataCreationManager.instance.infoBlockHolder.GetChild(i).GetComponent<InfoBlock>()));
            }
            currentDataFolder.myInfoBlockSaves = newInfoBlockSaves;
        }

        SaveManager.instance.SaveAllFolders();
        currentDataFolder = null;
        ResetPanelScroll();
    }

    public void OpenDeletePanel(int i)
    {
        if (!deletePanel.activeInHierarchy)
        {
            if (i == 0)
            {
                deletePanelHeaderText.text = "Are you sure you want to delete this folder and all of its contents?";
            }
            else if (i == 1)
            {
                deletePanelHeaderText.text = "Are you sure you want to delete the entire info block?";
            }
            else if (i == 2)
            {
                deletePanelHeaderText.text = "Are you sure you want to delete this info field?";
            }

            deletePanel.SetActive(true);
        }
    }

    public void CloseDeletePanelButton(bool b)
    {
        if (b)
        {
            if (toDelete != null)
            {
                if (toDelete.GetComponent<DataFolder>() != null)
                {
                    for (int i = 0; i < folderSaves.Count; i++)
                    {
                        if (folderSaves[i].iD == toDelete.GetComponent<DataFolder>().iD)
                        {
                            folderSaves.Remove(folderSaves[i]);
                            Destroy(toDelete);
                            toDelete = null;

                            deletePanel.SetActive(false);
                            return;
                        }
                    }
                }
                else if (toDelete.GetComponent<InfoBlock>() != null)
                {
                    for (int i = 0; i < currentDataFolder.myInfoBlockSaves.Count; i++)
                    {
                        if (currentDataFolder.myInfoBlockSaves[i].iD == toDelete.GetComponent<InfoBlock>().iD)
                        {
                            currentDataFolder.myInfoBlockSaves.Remove(currentDataFolder.myInfoBlockSaves[i]);
                            Destroy(toDelete);
                            toDelete = null;

                            deletePanel.SetActive(false);
                            return;
                        }
                    }

                    // This is for removing the infoblock out of the saves if you wanna do that more quickly, i dont remember if this worked, think it did
                    //for (int i = 0; i < folderSaves.Count; i++)
                    //{
                    //    for (int ii = 0; ii < folderSaves[i].infoBlockSaves.Count; ii++)
                    //    {
                    //        if (folderSaves[i].infoBlockSaves[ii].iD == toDelete.GetComponent<InfoBlock>().iD)
                    //        {
                    //            folderSaves[i].infoBlockSaves.Remove(folderSaves[i].infoBlockSaves[ii]);
                    //            Destroy(toDelete);
                    //            toDelete = null;
                    //            return;
                    //        }
                    //    }
                    //}
                }
                else if (toDelete.GetComponent<DataInputField>() != null)
                {
                    toDelete.GetComponent<DataInputField>().DeleteInputField();
                }
            }
        }
        else
        {
            if (toDelete != null)
            {
                if (toDelete.GetComponent<DataFolder>() != null)
                {
                    toDelete.GetComponent<DataFolder>().CloseOptionsButton();
                }
                else if (toDelete.GetComponent<InfoBlock>() != null)
                {
                    toDelete.GetComponent<InfoBlock>().CloseOptionsButton();
                }
                toDelete = null;
            }
        }

        deletePanel.SetActive(false);
    }

    public void ToggleOptionsPanelButton()
    {
        optionsPanel.SetActive(!optionsPanel.activeInHierarchy);
        customizePanel.SetActive(false);
    }

    public void ToggleCustomizePanelButton()
    {
        customizePanel.SetActive(!customizePanel.activeInHierarchy);
        optionsPanel.SetActive(false);
    }

    public void ToggleChangePassPanelButton()
    {
        changePassPanel.SetActive(!changePassPanel.activeInHierarchy);
        optionsPanel.SetActive(false);
    }

    public void ToggleClearDataPanelButton()
    {
        clearDataPanel.SetActive(!clearDataPanel.activeInHierarchy);
        optionsPanel.SetActive(false);
    }

    public void CustomizeButton(Image image)
    {
        ColorPicker.imageToChangeColor = image;
        customizePanel.SetActive(false);
        colorPickerPanel.SetActive(true);
    }

    private void ChangeCustomizeButtonColors(Button button, Image imageToChange)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = imageToChange.color;
        button.colors = colorBlock;
    }

    private void ResetPanelScroll()
    {
        DataCreationManager.instance.folderHolder.GetComponent<RectTransform>().offsetMin = new Vector2(0, DataCreationManager.instance.folderHolderDefaultPos.x);
        DataCreationManager.instance.folderHolder.GetComponent<RectTransform>().offsetMax = new Vector2(0, DataCreationManager.instance.folderHolderDefaultPos.y);
    }

    public void ConfirmNewPassButton()
    {
        if (oldPassText.text == SaveManager.Decrypt(SaveManager.instance.saveData.password))
        {
            if (newPassText.text != null)
            {
                SaveManager.instance.saveData.password = SaveManager.Encrypt(newPassText.text);

                oldPassText.text = null;
                newPassText.text = null;
                ToggleChangePassPanelButton();
            }
        }
    }

    public void CloseClearDataPanelButton(bool b)
    {
        if (b)
        {
            folderSaves = new List<FolderSave>();

            for (int i = 0; i < DataCreationManager.instance.folderHolder.childCount; i++)
            {
                Destroy(DataCreationManager.instance.folderHolder.GetChild(i).gameObject);
            }

            infoBlockPanel.SetActive(false);
        }

        clearDataPanel.SetActive(false);
    }
}
