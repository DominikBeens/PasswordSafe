using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{

    public static LoginManager instance;
    private Firebase.Auth.FirebaseAuth auth;
    public static Firebase.Auth.FirebaseUser user;

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Text headerText;

    [Header("Login Account")]
    public GameObject loginAccountPanel;
    [SerializeField] private GameObject onLoggingInOverlay;
    [Space(10)]
    [SerializeField] private InputField enterLoginEmailText;
    [SerializeField] private InputField enterLoginPasswordText;

    [Header("Create Account")]
    public GameObject createAccountPanel;
    [SerializeField] private GameObject onCreatingAccountOverlay;
    [Space(10)]
    [SerializeField] private InputField createLoginEmailText;
    [SerializeField] private InputField createPasswordText1;
    [SerializeField] private InputField createPasswordText2;
    [SerializeField] private InputField enterPasswordText;

    [Header("Change Password")]
    public GameObject changePassPanel;
    [Space(10)]
    [SerializeField] private InputField newPasswordText;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance && instance != this)
        {
            Destroy(this);
        }

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        loginPanel.SetActive(true);
    }

    public void OpenLoginPanel(AppSettings appSettings)
    {
        if (string.IsNullOrEmpty(appSettings.lastLoggedInEmail))
        {
            headerText.text = "Create An Account";

            createAccountPanel.SetActive(true);
        }
        else
        {
            headerText.text = "Login Your Account";

            enterLoginEmailText.text = appSettings.lastLoggedInEmail;
            loginAccountPanel.SetActive(true);
        }
    }

    public void OpenLoginPanelButton()
    {
        headerText.text = "Login Your Account";
        createAccountPanel.SetActive(false);
        loginAccountPanel.SetActive(true);
    }

    public void OpenCreateAccountPanelButton()
    {
        headerText.text = "Create An Account";
        createAccountPanel.SetActive(true);
        loginAccountPanel.SetActive(false);
    }

    public void ConfirmPasswordButton()
    {
        SignInExistingUser();

        //if (enterPasswordText.text == SaveManager.saveData.password)
        //{
        //    SignInExistingUser();
        //}
        //else
        //{
        //    StructureManager.instance.NewNotification("Wrong Password");
        //}
    }

    public void ConfirmPasswordCreationButton()
    {
        if (!string.IsNullOrEmpty(createPasswordText2.text) && string.Equals(createPasswordText2.text, createPasswordText1.text))
        {
            SignInNewUser();
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

    public void ConfirmNewPasswordButton()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;

        if (!string.IsNullOrEmpty(newPasswordText.text))
        {
            if (user != null)
            {
                string newPassword = newPasswordText.text;

                user.UpdatePasswordAsync(newPassword).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("UpdatePasswordAsync was canceled.");
                        StructureManager.instance.NewNotification("Password Updating Was Canceled");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("UpdatePasswordAsync encountered an error: " + task.Exception);
                        StructureManager.instance.NewNotification("Error: " + task.Exception.InnerExceptions[0].Message);
                        return;
                    }

                    newPasswordText.text = null;

                    StructureManager.instance.NewNotification("Password Changed");
                });
            }
        }
        else
        {
            StructureManager.instance.NewNotification("Enter A New Password");
        }
    }

    private void SignInNewUser()
    {
        string email = createLoginEmailText.text;
        string password = createPasswordText2.text;

        onCreatingAccountOverlay.SetActive(true);

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                StructureManager.instance.NewNotification("Account Creation Canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                StructureManager.instance.NewNotification("Error: " + task.Exception.InnerExceptions[0].Message);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

            user = auth.CurrentUser;

            onCreatingAccountOverlay.SetActive(false);
            createAccountPanel.SetActive(false);
            loginAccountPanel.SetActive(true);

            StructureManager.instance.NewNotification("Account Created Successfully");
        });
    }

    private void SignInExistingUser()
    {
        string email = enterLoginEmailText.text;
        string password = enterLoginPasswordText.text;

        onLoggingInOverlay.SetActive(true);

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                StructureManager.instance.NewNotification("User Login Canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                StructureManager.instance.NewNotification("Error: " + task.Exception.InnerExceptions[0].Message);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

            user = auth.CurrentUser;

            SaveManager.instance.LoadFromCloud();
            SaveManager.appSettings.lastLoggedInEmail = email;
            SaveManager.instance.SaveAppSettingsToFile(SaveManager.appSettings);

            onLoggingInOverlay.SetActive(false);
            loginPanel.SetActive(false);

            StructureManager.instance.NewNotification("Welcome");
        });
    }
}
