using System.Collections.Generic;

[System.Serializable]
public class SaveData
{

    public string password;

    public List<FolderSave> folderSaves = new List<FolderSave>();

    #region Colors
    public int homeHeaderBackgroundColorR;
    public int homeHeaderBackgroundColorG;
    public int homeHeaderBackgroundColorB;

    public int homeBackgroundColorR;
    public int homeBackgroundColorG;
    public int homeBackgroundColorB;

    public int newFolderBackgroundColorR;
    public int newFolderBackgroundColorG;
    public int newFolderBackgroundColorB;

    public int newInfoBlockBackgroundColorR;
    public int newInfoBlockBackgroundColorG;
    public int newInfoBlockBackgroundColorB;

    public int optionsBackgroundColorR;
    public int optionsBackgroundColorG;
    public int optionsBackgroundColorB;
    #endregion
}
