using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBlockHolder : MonoBehaviour
{

    public DataBlock myDataBlock;

    public InputField nameInputField;

    public Transform inputFieldHolder;

    private RectTransform rectTransform;

    public GameObject optionsPanel;
    public Animator anim;

    public Image customizableImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnValueChanged()
    {
        myDataBlock.dataBlockName = nameInputField.text;
    }

    public void Initialize()
    {
        if (myDataBlock != null)
        {
            nameInputField.text = myDataBlock.dataBlockName;

            //TODO: DataCreationManager and this script: data block custom color dont get saved properly, custom folder colors do.
            customizableImage.color = new Color(myDataBlock.color.r, myDataBlock.color.g, myDataBlock.color.b, myDataBlock.color.a);
        }
    }

    public void CreateNewInputField(int type)
    {
        DataCreationManager.instance.CreateNewDataField(type, this);
    }

    public void CustomizeButton()
    {
        ColorPicker.instance.StartCustomizing(customizableImage, gameObject);
        StartCoroutine(CloseOptions());
    }

    public void IncreaseSize()
    {
        // Dont increase the size with the first data field.
        if (myDataBlock.myDataFields.Count == 1)
        {
            return;
        }

        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - 75);
    }

    public void DecreaseSize()
    {
        // Dont decrease the size when its the last data field.
        if (myDataBlock.myDataFields.Count == 0)
        {
            return;
        }

        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y + 75);
    }

    public void SetSize(int dataFieldAmount)
    {
        // No point in setting a custom size if theres no data fields.
        if (dataFieldAmount == 0)
        {
            return;
        }

        // Amount of data fields minus one because the first data field doesnt increase the data block size.
        float bottomSize = (dataFieldAmount - 1) * 75;
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - bottomSize);
    }

    public void ToggleOptionsButton()
    {
        if (!optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CloseOptions());
        }
    }

    public IEnumerator CloseOptions()
    {
        anim.SetTrigger("Close");

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        optionsPanel.SetActive(false);
    }

    public void DeleteInfoBlock()
    {
        StructureManager.toDelete = gameObject;
        StructureManager.instance.OpenDeletePanel(1);
    }
}
