using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;
public class Notification : MonoBehaviour
{
    void Start()
    {
        RegisterNotificationChannel();
        StartCoroutine(RequestNotificationPermission());
    }

    void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "habitime_channel",
            Name = "Habitime",
            Importance = Importance.Low,
            EnableVibration = false,

            Description = "Shows clock time"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    IEnumerator RequestNotificationPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;

        if (request.Status == PermissionStatus.Allowed)
            Debug.Log("Notifications permitted");
        else
            Debug.Log("Notifications denied");
    }

    public void SendStopwatchNotification(string timeText)
    {
        AndroidNotificationCenter.CancelAllNotifications();

        var notification = new AndroidNotification();
        notification.Title = "Habitime";
        notification.Text = timeText;
        notification.FireTime = System.DateTime.Now;
        notification.SmallIcon = "default";

        AndroidNotificationCenter.SendNotification(notification, "habitime_channel");
    }
}