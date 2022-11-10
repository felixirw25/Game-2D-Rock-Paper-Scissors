using UnityEngine;

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; protected set; }
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

    /// <summary>
    /// This transforms the static instance into a basic singleton. This will destroy any new
    /// versions created, leaving the original instance intact
    /// </summary>
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
    protected override void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
            base.Awake();
        }
    }

    /// <summary>
    /// Finally we have a persistent version of the singleton. This will survive through scene
    /// loads. Perfect for system classes which require stateful, persistent data. Or audio sources
    /// where music plays through loading screens, etc
    /// </summary>
    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}