using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public string password;
    public string googleDriveSaveFileId;

    public List<DataFolder> dataFolders = new List<DataFolder>();

    #region Colors
    public Color defaultHomeHeaderBackgroundColor = new Color32(34, 30, 54, 255);
    public Color homeHeaderBackgroundColor = new Color32(34, 30, 54, 255);

    public Color defaultHomeBackgroundColor = new Color32(53, 53, 68, 255);
    public Color homeBackgroundColor = new Color32(53, 53, 68, 255);

    public Color defaultNewDataFolderBackgroundColor = new Color32(24, 21, 39, 255);
    public Color newDataFolderBackgroundColor = new Color32(24, 21, 39, 255);

    public Color defaultNewDataBlockBackgroundColor = new Color32(24, 21, 39, 255);
    public Color newDataBlockBackgroundColor = new Color32(24, 21, 39, 255);
    #endregion
}
