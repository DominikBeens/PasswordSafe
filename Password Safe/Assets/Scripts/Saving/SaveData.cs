using System.Collections.Generic;

[System.Serializable]
public class SaveData
{

    public List<DataFolder> dataFolders = new List<DataFolder>();

    #region Colors
    public SaveManager.SerializableColor defaultHomeHeaderBackgroundColor = new SaveManager.SerializableColor(34, 30, 54, 255);
    public SaveManager.SerializableColor homeHeaderBackgroundColor = new SaveManager.SerializableColor(34, 30, 54, 255);
                
    public SaveManager.SerializableColor defaultHomeBackgroundColor = new SaveManager.SerializableColor(53, 53, 68, 255);
    public SaveManager.SerializableColor homeBackgroundColor = new SaveManager.SerializableColor(53, 53, 68, 255);
                
    public SaveManager.SerializableColor defaultNewDataFolderBackgroundColor = new SaveManager.SerializableColor(24, 21, 39, 255);
    public SaveManager.SerializableColor newDataFolderBackgroundColor = new SaveManager.SerializableColor(24, 21, 39, 255);
                
    public SaveManager.SerializableColor defaultNewDataBlockBackgroundColor = new SaveManager.SerializableColor(24, 21, 39, 255);
    public SaveManager.SerializableColor newDataBlockBackgroundColor = new SaveManager.SerializableColor(24, 21, 39, 255);
    #endregion
}
