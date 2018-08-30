
[System.Serializable]
public class AppSettings 
{

    public string lastLoggedInEmail;

    #region Colors
    public SaveManager.SerializableColor defaultHomeHeaderBackgroundColor = new SaveManager.SerializableColor(0.1314447f, 0.1160251f, 0.2132353f, 1);
    public SaveManager.SerializableColor homeHeaderBackgroundColor = new SaveManager.SerializableColor(0.1314447f, 0.1160251f, 0.2132353f, 1);

    public SaveManager.SerializableColor defaultHomeBackgroundColor = new SaveManager.SerializableColor(0.2078431f, 0.2078431f, 0.2666667f, 1);
    public SaveManager.SerializableColor homeBackgroundColor = new SaveManager.SerializableColor(0.2078431f, 0.2078431f, 0.2666667f, 1);

    public SaveManager.SerializableColor defaultNewDataFolderBackgroundColor = new SaveManager.SerializableColor(0.09337371f, 0.08174742f, 0.1544118f, 1);
    public SaveManager.SerializableColor newDataFolderBackgroundColor = new SaveManager.SerializableColor(0.09337371f, 0.08174742f, 0.1544118f, 1);

    public SaveManager.SerializableColor defaultNewDataBlockBackgroundColor = new SaveManager.SerializableColor(0.09337371f, 0.08174742f, 0.1544118f, 1);
    public SaveManager.SerializableColor newDataBlockBackgroundColor = new SaveManager.SerializableColor(0.09337371f, 0.08174742f, 0.1544118f, 1);
    #endregion
}
