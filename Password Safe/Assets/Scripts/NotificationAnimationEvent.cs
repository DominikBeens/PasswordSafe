using UnityEngine;

public class NotificationAnimationEvent : MonoBehaviour 
{

    public void Activate()
    {
        StructureManager.instance.notificationIsActive = true;
    }

    public void DeActivate()
    {
        StructureManager.instance.notificationIsActive = false;
        gameObject.SetActive(false);
    }
}
