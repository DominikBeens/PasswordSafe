using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataFieldHolder : MonoBehaviour
{

    public DataField myDataField;

    private InputField inputField;

    public void OnValueChanged()
    {
        myDataField.text = inputField.text;
    }

    private void OnEnable()
    {
        inputField = GetComponent<InputField>();

        if (myDataField != null)
        {
            inputField.text = myDataField.text;
        }
    }

    public void DeleteInputFieldButton()
    {
        StructureManager.toDelete = gameObject;
        StructureManager.instance.OpenDeletePanel(2);
    }

    public void DeleteDataField()
    {
        DataBlockHolder topParent = transform.parent.GetComponentInParent<DataBlockHolder>();

        for (int i = 0; i < topParent.myDataBlock.myDataFields.Count; i++)
        {
            if (myDataField.iD == topParent.myDataBlock.myDataFields[i].iD)
            {
                topParent.myDataBlock.myDataFields.Remove(topParent.myDataBlock.myDataFields[i]);
                topParent.DecreaseSize();
                Destroy(gameObject);
            }
        }
    }
}
