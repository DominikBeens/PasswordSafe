using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{

    private Animator anim;

    public Image colorPreview;

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

    public Text rValueText;
    public Text gValueText;
    public Text bValueText;

    public static Image imageToChangeColor;
    private Color32 newColor;

    private int? rGB;
    private float timer;
    private bool mouseDownIncrease;
    private bool mouseDownDecrease;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            rValueText.text = RValue.ToString();
            gValueText.text = GValue.ToString();
            bValueText.text = BValue.ToString();

            newColor = new Color32(System.Convert.ToByte(RValue), System.Convert.ToByte(GValue), System.Convert.ToByte(BValue), 255);

            colorPreview.color = newColor;
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

    public void OnEnable()
    {
        Color32 color = imageToChangeColor.color;

        RValue = color.r;
        GValue = color.g;
        BValue = color.b;
    }

    public void ConfirmColor()
    {
        imageToChangeColor.color = newColor;
        imageToChangeColor = null;
        gameObject.SetActive(false);
    }

    public void IncreaseColorValue(int rGB)
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

    public void DecreaseColorValue(int rGB)
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
