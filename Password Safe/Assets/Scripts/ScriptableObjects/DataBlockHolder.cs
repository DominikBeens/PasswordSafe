using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBlockHolder : MonoBehaviour
{

    public DataBlock myDataBlock;

    public int iD;

    public List<InputFieldSave> myInputFieldSaves = new List<InputFieldSave>();

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

    public void CreateNewInputField(int type)
    {
        //DataCreationManager.instance.CreateNewInputField(type, this);
    }

    public void CustomizeButton()
    {
        ColorPicker.imageToChangeColor = customizableImage;
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

    public void OpenOptions()
    {
        if (!optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptionsButton()
    {
        StartCoroutine(CloseOptions());
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
