using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInputField : MonoBehaviour
{

    public int iD;

    public void DeleteInputFieldButton()
    {
        StructureManager.toDelete = gameObject;
        StructureManager.instance.OpenDeletePanel(2);
    }

    public void DeleteInputField()
    {
        InfoBlock topParent = transform.parent.GetComponentInParent<InfoBlock>();

        for (int i = 0; i < topParent.myInputFieldSaves.Count; i++)
        {
            if (iD == topParent.myInputFieldSaves[i].iD)
            {
                topParent.myInputFieldSaves.Remove(topParent.myInputFieldSaves[i]);
                topParent.DecreaseSize();
                Destroy(gameObject);
            }
        }
    }
}
