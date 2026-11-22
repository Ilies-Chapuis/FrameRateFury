using UnityEngine;

public class TrackedEntity : MonoBehaviour
{
    private bool registered;

    private void OnEnable()
    {
        // Si le compteur n'existe pas encore, on ne s'enregistre pas
        if (PerformanceCounter.Instance == null) return;
        PerformanceCounter.Instance.Register();
        registered = true;
    }

    private void OnDisable()
    {
        // On ne se désenregistre que si on s'était enregistré
        if (!registered || PerformanceCounter.Instance == null) return;
        PerformanceCounter.Instance.Unregister();
        registered = false;
    }
}
