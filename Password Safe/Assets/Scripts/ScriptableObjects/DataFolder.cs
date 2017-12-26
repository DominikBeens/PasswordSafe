using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataFolder
{

    public int iD;

    public string folderName;

    public List<DataBlock> myDataBlocks = new List<DataBlock>();

    public Color color;
}
