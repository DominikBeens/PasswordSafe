using System.Collections.Generic;

[System.Serializable]
public class FolderSave
{

    public int iD;

    public string name;

    public List<InfoBlockSave> infoBlockSaves = new List<InfoBlockSave>();

    public int colorR;
    public int colorG;
    public int colorB;
}
