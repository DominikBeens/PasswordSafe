using System.Collections.Generic;

[System.Serializable]
public class InfoBlockSave
{

    public int iD;

    public string name;

    public List<InputFieldSave> inputFieldSaves = new List<InputFieldSave>();

    public int colorR;
    public int colorG;
    public int colorB;
}
