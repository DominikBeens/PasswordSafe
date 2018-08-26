using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{

    public static ColorPicker instance;

    [SerializeField] private GameObject colorPickerPanel;
    [SerializeField] private Image newColorPreviewImage;

    private int rValue;
    public int RValue
    {
        get { return rValue; }
        set { rValue = Mathf.Clamp(value, 0, 255); }
    }
    private int gValue;
    public int GValue
    {
        get { return gValue; }
        set { gValue = Mathf.Clamp(value, 0, 255); }
    }
    private int bValue;
    public int BValue
    {
        get { return bValue; }
        set { bValue = Mathf.Clamp(value, 0, 255); }
    }

    [Space(10)]
    [SerializeField] private Text rValueText;
    [SerializeField] private Text gValueText;
    [SerializeField] private Text bValueText;

    private GameObject dataObjectToSaveTo;
    private Image imageToChangeColor;
    private Color32 newColor;

    private int? rGB;
    private float timer;
    private bool mouseDownIncrease;
    private bool mouseDownDecrease;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (colorPickerPanel.activeInHierarchy)
        {
            rValueText.text = RValue.ToString();
            gValueText.text = GValue.ToString();
            bValueText.text = BValue.ToString();

            newColor = new Color32(System.Convert.ToByte(RValue), System.Convert.ToByte(GValue), System.Convert.ToByte(BValue), 255);

            newColorPreviewImage.color = newColor;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (mouseDownIncrease)
            {
                IncreaseColorValue((int)rGB);
            }
            if (mouseDownDecrease)
            {
                DecreaseColorValue((int)rGB);
            }
        }
    }

    public void StartCustomizingButton(Image image)
    {
        StartCustomizing(image);
    }

    public void StartCustomizing(Image image, GameObject dataObjectToSaveTo = null)
    {
        imageToChangeColor = image;
        this.dataObjectToSaveTo = dataObjectToSaveTo;
        Color32 color = imageToChangeColor.color;

        RValue = color.r;
        GValue = color.g;
        BValue = color.b;

        colorPickerPanel.SetActive(true);
    }

    public void ConfirmColor()
    {
        imageToChangeColor.color = newColor;

        SaveManager.instance.SaveAppColors();

        if (dataObjectToSaveTo != null)
        {
            if (dataObjectToSaveTo.GetComponent<DataFolderHolder>() != null)
            {
                dataObjectToSaveTo.GetComponent<DataFolderHolder>().myDataFolder.color = new SaveManager.SerializableColor(newColor.r, newColor.g, newColor.b, newColor.a);
            }
            else if (dataObjectToSaveTo.GetComponent<DataBlockHolder>() != null)
            {
                dataObjectToSaveTo.GetComponent<DataBlockHolder>().myDataBlock.color = new SaveManager.SerializableColor(newColor.r, newColor.g, newColor.b, newColor.a);
            }
        }

        imageToChangeColor = null;
        dataObjectToSaveTo = null;
        colorPickerPanel.SetActive(false);
    }

    private void IncreaseColorValue(int rGB)
    {
        switch (rGB)
        {
            case 0:
                RValue++;
                break;
            case 1:
                GValue++;
                break;
            case 2:
                BValue++;
                break;
        }
    }

    private void DecreaseColorValue(int rGB)
    {
        switch (rGB)
        {
            case 0:
                RValue--;
                break;
            case 1:
                GValue--;
                break;
            case 2:
                BValue--;
                break;
        }
    }

    public void MouseDownIncrease(int setRGB)
    {
        timer = 0.5f;
        mouseDownIncrease = true;
        rGB = setRGB;

        IncreaseColorValue((int)rGB);
    }

    public void MouseDownDecrease(int setRGB)
    {
        timer = 0.5f;
        mouseDownDecrease = true;
        rGB = setRGB;

        DecreaseColorValue((int)rGB);
    }

    public void MouseUp()
    {
        timer = 0f;
        mouseDownIncrease = false;
        mouseDownDecrease = false;
        rGB = null;
    }
}
