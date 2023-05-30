using System.Collections;
using System.Collections.Generic;
//using Unity.Notifications.Android;
using UnityEngine;

/*
 * Manages the notifications that are sent to the phone`s notification bar.
 */

public class NotificationManager {


    /*public static void NotificationCreation() {

        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        var channel = new AndroidNotificationChannel() {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Low,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        AndroidNotification notification = new AndroidNotification();
        notification.Title = "Sneak Away: Ayuda a Anthony a escapar de las instalaciones.";
        notification.Text = "¡Los guardias están cerca, evita que descubrán a Anthony!";
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";
        notification.FireTime = System.DateTime.Now.AddSeconds(7);

        // Finalmente enviamos la notificacion al movil
        int id = AndroidNotificationCenter.SendNotification(notification, "channel_id");
        // Hemos recogido la notificacion en una variable llamada id para comprobar si el mensaje esta mostrado para no acumular mensajes
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled) {
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }*/
}
