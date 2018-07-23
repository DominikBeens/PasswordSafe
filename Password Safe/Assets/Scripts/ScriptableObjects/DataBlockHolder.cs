using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBlockHolder : MonoBehaviour
{

    public DataBlock myDataBlock;

    public Text nameText;
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

    private void Update()
    {
        if (nameInputField.text != null)
        {
            nameText.text = nameInputField.text;
        }
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

            customizableImage.color = new Color(myDataBlock.color.r / 255, myDataBlock.color.g / 255, myDataBlock.color.b / 255, myDataBlock.color.a / 255);
        }
    }

    public void CreateNewInputField(int type)
    {
        DataCreationManager.instance.CreateNewDataField(type, this);
    }

    public void CustomizeButton()
    {
        ColorPicker.imageToChangeColor = customizableImage;
        ColorPicker.dataObjectToSaveTo = gameObject;
        StructureManager.instance.colorPickerPanel.SetActive(true);

        StartCoroutine(CloseOptions());
    }

    public void IncreaseSize()
    {
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - 75);
    }

    public void DecreaseSize()
    {
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y + 75);
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
