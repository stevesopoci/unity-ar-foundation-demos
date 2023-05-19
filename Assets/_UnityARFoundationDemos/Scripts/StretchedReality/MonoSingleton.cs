using UnityEngine;

namespace StretchedReality
{
    /// <summary>
    /// Extend the generic MonoSingleton class to create new singletons.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T m_Instance = null;

        public static T Instance
        {
            get
            {
                if (m_Instance)
                {
                    return m_Instance;
                }
                else
                {
                    m_Instance = FindObjectOfType(typeof(T)) as T;

                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                        if (m_Instance == null)
                        {
                            Debug.LogError("MonoSingleton: Problem during the creation of " + typeof(T).ToString());
                        }
                    }
                }

                return m_Instance;
            }
        }

        /// <summary>
        /// Override when necessary and call base.Awake() first.
        /// </summary>
        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
            }
        }

        /// <summary>
        /// Clear the reference when the application quits. Override when necessary and call base.OnApplicationQuit() last.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            m_Instance = null;
        }
    }
}