using UnityEngine;

namespace Helper
{
    public class SingletonMonobehaviour <T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance => _instance;
        public bool dontDestroyOnLoad = false;

        public virtual void Awake() 
        {
            _instance = this as T;
            if(dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }    
        }
    }
}

