using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBlock")]
public class DataBlock : ScriptableObject
{

    public int iD;

    public string dataBlockName;

    public List<InputFieldSave> inputFieldSaves = new List<InputFieldSave>();

    public int colorR;
    public int colorG;
    public int colorB;

}
