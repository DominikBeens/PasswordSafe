using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{

    public GameObject loginPanel;

    public InputField createPasswordText1;
    public InputField createPasswordText2;

    public InputField enterPasswordText;

    public GameObject enterPasswordPanel;
    public GameObject createPasswordPanel;

    private void Awake()
    {
        loginPanel.SetActive(true);
    }

    public void ConfirmPasswordButton()
    {
        if (enterPasswordText.text == SaveManager.saveData.password)
        {
            SaveManager.instance.SetLoadedSaveData();

            loginPanel.SetActive(false);
            StructureManager.instance.NewNotification("Welcome");
        }
        else
        {
            StructureManager.instance.NewNotification("Wrong Password");
        }
    }

    public void ConfirmPasswordCreationButton()
    {
        if (!string.IsNullOrEmpty(createPasswordText2.text) && string.Equals(createPasswordText2.text, createPasswordText1.text))
        {
            SaveManager.saveData.password = createPasswordText2.text;

            createPasswordPanel.SetActive(false);
            enterPasswordPanel.SetActive(true);

            StructureManager.instance.NewNotification("Password Created");
        }
        else
        {
            if (string.IsNullOrEmpty(createPasswordText1.text))
            {
                StructureManager.instance.NewNotification("Enter A Password");
                return;
            }

            if (string.IsNullOrEmpty(createPasswordText2.text))
            {
                StructureManager.instance.NewNotification("Confirm Your Password");
                return;
            }

            if (!string.Equals(createPasswordText2.text, createPasswordText1.text))
            {
                StructureManager.instance.NewNotification("Passwords Do Not Match");
                return;
            }
        }
    }
}
