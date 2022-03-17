using UnityEngine;

namespace Farm2D
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance = null;

        public static T Instance => s_instance;

        protected virtual void Awake()
        {
            if (!s_instance)
            {
                s_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        protected virtual void OnApplicationQuit()
        {
            s_instance = null;
            Destroy(gameObject);
        }
    }
}
