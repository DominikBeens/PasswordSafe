using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataBlock
{

    public int iD;

    public string dataBlockName;

    public List<DataField> myDataFields = new List<DataField>();

    public SaveManager.SerializableColor color;
}
