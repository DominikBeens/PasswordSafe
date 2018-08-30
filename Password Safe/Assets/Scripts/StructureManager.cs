using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureManager : MonoBehaviour
{

    public static StructureManager instance;

    public static DataFolder currentDataFolder;

    public GameObject infoBlockPanel;

    private Vector2 folderHolderDefaultPos;
    public Transform folderHolder;

    private Vector2 infoBlockHolderDefaultPos;
    public Transform infoBlockHolder;

    [Header("Delete Panel")]
    [SerializeField] private GameObject deletePanel;
    [SerializeField] private Text deletePanelHeaderText;

    public static GameObject toDelete;

    [Header("Customize Picker")]
    [SerializeField] private GameObject customizePanel;
    [Space(10)]
    [SerializeField] private Image homeHeaderBackgroundColorPreview;
    public Image homeHeaderBackground;
    [Space(10)]
    [SerializeField] private Image homeBackgroundColorPreview;
    public Image homeBackground;
    [Space(10)]
    [SerializeField] private Image newFolderBackgroundColorPreview;
    public Image newFolderBackground;
    [Space(10)]
    [SerializeField] private Image newInfoBlockBackgroundColorPreview;
    public Image newInfoBlockBackground;
    [Space(10)]
    [SerializeField] private Image optionsBackgroundColorPreview;
    public Image optionsBackground;

    [Header("Notification Panel")]
    public GameObject notificationPanel;
    public Text notificationMessage;
    [HideInInspector]
    public bool notificationIsActive;

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

        folderHolderDefaultPos = new Vector2(folderHolder.GetComponent<RectTransform>().offsetMin.y, folderHolder.GetComponent<RectTransform>().offsetMax.y);
        infoBlockHolderDefaultPos = new Vector2(infoBlockHolder.GetComponent<RectTransform>().offsetMin.y, infoBlockHolder.GetComponent<RectTransform>().offsetMax.y);

        notificationPanel.SetActive(false);
    }

    private void Update()
    {
        if (customizePanel.activeInHierarchy)
        {
            homeHeaderBackgroundColorPreview.color = homeHeaderBackground.color;
            homeBackgroundColorPreview.color = homeBackground.color;
            newFolderBackgroundColorPreview.color = newFolderBackground.color;
            newInfoBlockBackgroundColorPreview.color = newInfoBlockBackground.color;
            optionsBackgroundColorPreview.color = optionsBackground.color;
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
        ResetFolderScroll();
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
                    for (int i = 0; i < SaveManager.saveData.dataFolders.Count; i++)
                    {
                        if (SaveManager.saveData.dataFolders[i].iD == toDelete.GetComponent<DataFolderHolder>().myDataFolder.iD)
                        {
                            SaveManager.saveData.dataFolders.Remove(SaveManager.saveData.dataFolders[i]);
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

    public void ResetColorsButton()
    {
        // Sets the default colors.
        SetDefaultColorsFromSaveData(SaveManager.appSettings);
        // Saves the current set of colors.
        SaveManager.instance.SaveAppColors();
    }

    public void LoadColorsFromAppSettings(AppSettings appSettings)
    {
        homeHeaderBackground.color = new Color(appSettings.homeHeaderBackgroundColor.r,
                                               appSettings.homeHeaderBackgroundColor.g,
                                               appSettings.homeHeaderBackgroundColor.b,
                                               appSettings.homeHeaderBackgroundColor.a);

        homeBackground.color = new Color(appSettings.homeBackgroundColor.r,
                                         appSettings.homeBackgroundColor.g,
                                         appSettings.homeBackgroundColor.b,
                                         appSettings.homeBackgroundColor.a);

        newFolderBackground.color = new Color(appSettings.newDataFolderBackgroundColor.r,
                                              appSettings.newDataFolderBackgroundColor.g,
                                              appSettings.newDataFolderBackgroundColor.b,
                                              appSettings.newDataFolderBackgroundColor.a);

        newInfoBlockBackground.color = new Color(appSettings.newDataBlockBackgroundColor.r,
                                                 appSettings.newDataBlockBackgroundColor.g,
                                                 appSettings.newDataBlockBackgroundColor.b,
                                                 appSettings.newDataBlockBackgroundColor.a);
        //inside folder
        optionsBackground.color = new Color(appSettings.homeBackgroundColor.r,
                                            appSettings.homeBackgroundColor.g,
                                            appSettings.homeBackgroundColor.b,
                                            appSettings.homeBackgroundColor.a);
    }

    public void SetDefaultColorsFromSaveData(AppSettings appSettings)
    {
        homeHeaderBackground.color = new Color(appSettings.defaultHomeHeaderBackgroundColor.r,
                                               appSettings.defaultHomeHeaderBackgroundColor.g,
                                               appSettings.defaultHomeHeaderBackgroundColor.b,
                                               appSettings.defaultHomeHeaderBackgroundColor.a);

        homeBackground.color = new Color(appSettings.defaultHomeBackgroundColor.r,
                                         appSettings.defaultHomeBackgroundColor.g,
                                         appSettings.defaultHomeBackgroundColor.b,
                                         appSettings.defaultHomeBackgroundColor.a);

        newFolderBackground.color = new Color(appSettings.defaultNewDataFolderBackgroundColor.r,
                                              appSettings.defaultNewDataFolderBackgroundColor.g,
                                              appSettings.defaultNewDataFolderBackgroundColor.b,
                                              appSettings.defaultNewDataFolderBackgroundColor.a);

        newInfoBlockBackground.color = new Color(appSettings.defaultNewDataBlockBackgroundColor.r,
                                                 appSettings.defaultNewDataBlockBackgroundColor.g,
                                                 appSettings.defaultNewDataBlockBackgroundColor.b,
                                                 appSettings.defaultNewDataBlockBackgroundColor.a);
        //inside folder
        optionsBackground.color = new Color(appSettings.defaultHomeBackgroundColor.r,
                                            appSettings.defaultHomeBackgroundColor.g,
                                            appSettings.defaultHomeBackgroundColor.b,
                                            appSettings.defaultHomeBackgroundColor.a);
    }

    private void ResetFolderScroll()
    {
        folderHolder.GetComponent<RectTransform>().offsetMin = new Vector2(0, folderHolderDefaultPos.x);
        folderHolder.GetComponent<RectTransform>().offsetMax = new Vector2(0, folderHolderDefaultPos.y);
    }

    public void ResetInfoBlockScroll()
    {
        infoBlockHolder.GetComponent<RectTransform>().offsetMin = new Vector2(0, infoBlockHolderDefaultPos.x);
        infoBlockHolder.GetComponent<RectTransform>().offsetMax = new Vector2(0, infoBlockHolderDefaultPos.y);
    }

    //public void CloseClearDataPanelButton(bool b)
    //{
    //    if (b)
    //    {
    //        SaveManager.saveData.dataFolders = new List<DataFolder>();

    //        for (int i = 0; i < DataCreationManager.instance.folderHolder.childCount; i++)
    //        {
    //            Destroy(DataCreationManager.instance.folderHolder.GetChild(i).gameObject);
    //        }

    //        infoBlockPanel.SetActive(false);

    //        NewNotification("Cleared Local Data");
    //    }

    //    clearDataPanel.SetActive(false);
    //}

    public void NewNotification(string message)
    {
        if (!notificationIsActive)
        {
            notificationPanel.SetActive(true);

            notificationMessage.text = message;

            RectTransform panelTransform = notificationPanel.GetComponent<RectTransform>();
            RectTransform textTransform = notificationMessage.GetComponent<RectTransform>();

            LayoutRebuilder.ForceRebuildLayoutImmediate(textTransform);

            panelTransform.sizeDelta = new Vector2(textTransform.sizeDelta.x + 35, textTransform.sizeDelta.y + 20);
        }
    }
}
