using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureManager : MonoBehaviour
{

    public static StructureManager instance;
    public static DataFolder currentDataFolder;

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

    [Header("Data Saving Panel")]
    public GameObject dataSavePanel;

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

    public void CreateDataFolder()
    {
        DataCreationManager.instance.CreateNewDataFolder(null);
    }

    public void CreateDataBlock()
    {
        DataCreationManager.instance.CreateNewDataBlock(null);
    }

    public void HomeButton()
    {
        infoBlockPanel.SetActive(false);
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
                if (toDelete.GetComponent<DataFolderHolder>() != null)
                {
                    for (int i = 0; i < TestSaver.saveData.dataFolders.Count; i++)
                    {
                        if (TestSaver.saveData.dataFolders[i].iD == toDelete.GetComponent<DataFolderHolder>().myDataFolder.iD)
                        {
                            TestSaver.saveData.dataFolders.Remove(TestSaver.saveData.dataFolders[i]);
                            Destroy(toDelete);
                            toDelete = null;

                            deletePanel.SetActive(false);
                            return;
                        }
                    }
                }
                else if (toDelete.GetComponent<DataBlockHolder>() != null)
                {
                    for (int i = 0; i < currentDataFolder.myDataBlocks.Count; i++)
                    {
                        if (currentDataFolder.myDataBlocks[i].iD == toDelete.GetComponent<DataBlockHolder>().myDataBlock.iD)
                        {
                            currentDataFolder.myDataBlocks.Remove(currentDataFolder.myDataBlocks[i]);
                            Destroy(toDelete);
                            toDelete = null;

                            deletePanel.SetActive(false);
                            return;
                        }
                    }
                }
                else if (toDelete.GetComponent<DataFieldHolder>() != null)
                {
                    toDelete.GetComponent<DataFieldHolder>().DeleteDataField();
                }
            }
        }
        else
        {
            if (toDelete != null)
            {
                if (toDelete.GetComponent<DataFolderHolder>() != null)
                {
                    toDelete.GetComponent<DataFolderHolder>().ToggleOptionsButton();
                }
                else if (toDelete.GetComponent<DataBlockHolder>() != null)
                {
                    toDelete.GetComponent<DataBlockHolder>().ToggleOptionsButton();
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
        dataSavePanel.SetActive(false);
    }

    public void ToggleDataSavePanelButton()
    {
        dataSavePanel.SetActive(!dataSavePanel.activeInHierarchy);
        optionsPanel.SetActive(false);
    }

    public void CustomizeButton(Image image)
    {
        ColorPicker.imageToChangeColor = image;
        customizePanel.SetActive(false);
        colorPickerPanel.SetActive(true);
    }

    public void ResetColorsButton()
    {
        // change colors
        homeHeaderBackground.color = TestSaver.saveData.defaultHomeHeaderBackgroundColor;
        homeBackground.color = TestSaver.saveData.defaultHomeBackgroundColor;
        newFolderBackground.color = TestSaver.saveData.defaultNewDataFolderBackgroundColor;
        newInfoBlockBackground.color = TestSaver.saveData.defaultNewDataBlockBackgroundColor;

        //inside folder
        optionsBackground.color = TestSaver.saveData.defaultHomeBackgroundColor;

        //save
        TestSaver.saveData.homeHeaderBackgroundColor = homeHeaderBackground.color;
        TestSaver.saveData.homeBackgroundColor = homeBackground.color;
        TestSaver.saveData.newDataFolderBackgroundColor = newFolderBackground.color;
        TestSaver.saveData.newDataBlockBackgroundColor = newInfoBlockBackground.color;
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
        if (oldPassText.text == TestSaver.saveData.password)
        {
            if (newPassText.text != null)
            {
                TestSaver.saveData.password = newPassText.text;

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
            TestSaver.saveData.dataFolders = new List<DataFolder>();

            for (int i = 0; i < DataCreationManager.instance.folderHolder.childCount; i++)
            {
                Destroy(DataCreationManager.instance.folderHolder.GetChild(i).gameObject);
            }

            infoBlockPanel.SetActive(false);
        }

        clearDataPanel.SetActive(false);
    }
}
