using UnityEngine;

namespace myEventSystem
{
    public class SingletonAsComponent<T> : MonoBehaviour where T : SingletonAsComponent<T>
    {
        private static T __Instance;

        public static T Instance
        {
            get
            {
                if (!__Instance)
                {
                    T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];
                    if (managers != null)
                    {
                        if (managers.Length == 1)
                        {
                            __Instance = managers[0];
                            return __Instance;
                        }
                        else if (managers.Length > 1)
                        {
                            Debug.LogError("You have more than one" + typeof(T).Name + " in the scene. You only need 1, it's a singleton");
                            for (int i = 0; i < managers.Length; i++)
                            {
                                T manager = managers[i];
                                Destroy(manager.gameObject);
                            }
                        }
                    }
                    GameObject go = new GameObject(typeof(T).Name, typeof(T));
                    __Instance = go.GetComponent<T>();
                    DontDestroyOnLoad(__Instance.gameObject);
                }
                return __Instance;
            }
            set
            {
                __Instance = value as T;
            }
        }
    }
}
