using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("🔔 NotificationManager Start");

        var channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Notificaciones del juego",
            Importance = Importance.Default,
            Description = "Canal de notificaciones para atraer al jugador"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        Debug.Log("✅ Canal de notificación registrado");
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log($"🔁 OnApplicationPause llamado. ¿Pausa? {pause}");

        if (pause)
        {
            Debug.Log("⏰ Programando notificación por pausa");
            ScheduleNotification();
        }
        else
        {
            Debug.Log("🚫 Cancelando notificaciones programadas");
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("❌ OnApplicationQuit llamado");
        ScheduleNotification();
    }

    private void ScheduleNotification()
    {
        Debug.Log("📨 Ejecutando ScheduleNotification");

        var notification = new AndroidNotification
        {
            Title = "¡Te estamos esperando!",
            Text = "Tu bonus diario está listo para ser reclamado.",
            FireTime = System.DateTime.Now.AddSeconds(10) // ← para testeo rápido
        };

        int id = AndroidNotificationCenter.SendNotification(notification, "default_channel");
        Debug.Log($"📩 Notificación enviada con ID: {id}");
    }
}
