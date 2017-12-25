using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataFolder")]
public class DataFolderSO : ScriptableObject
{

    public int iD;

    public string folderName;

    public List<DataBlock> infoBlockSaves = new List<DataBlock>();

    public int colorR;
    public int colorG;
    public int colorB;

}
