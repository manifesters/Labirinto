using UnityEngine;

namespace Helper
{
    public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance => _instance;

        public bool dontDestroyOnLoad = false;

        public virtual void Awake() 
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogError($"Multiple instances of {typeof(T)} detected. Destroying the new one.");
                Destroy(this.gameObject);
                return;
            }

            _instance = this as T;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}