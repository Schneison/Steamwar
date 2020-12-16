using UnityEngine;


namespace Steamwar.Utils {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static readonly object _lock = new object();
        private static bool _shuttingDown = false;
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_shuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +"' already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var singletonObj = GameObject.FindGameObjectsWithTag("Singleton");
                        if(singletonObj != null && singletonObj.Length > 0) {
                            foreach(GameObject obj in singletonObj)
                            {
                                _instance = obj.GetComponent<T>();
                                if(_instance != null)
                                {
                                    return _instance;
                                }
                            }
                        }
                        if (_instance == null) { 
                            // Search for existing instance.
                            _instance = (T)FindObjectOfType(typeof(T), true);
                        }

                        // Create new instance if one doesn't already exist.
                        if (_instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return _instance;
                }
            }
        }


        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }


        private void OnDestroy()
        {
            _shuttingDown = true;
        }
    }
}