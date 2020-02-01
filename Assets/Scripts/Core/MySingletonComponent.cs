using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySingletonComponent<T> : MyComponent
{
    // These variables link with components
    #region Link
    protected static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    /// <summary>
    /// Set the singleton's instance.
    /// </summary>
    /// <param name="_singleton">Singleton.</param>
    protected void setSingleton(T _singleton)
    {
        instance = _singleton;
    }

    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set();
        if (instance == null)
        {
            this.setSingleton(this.GetComponent<T>());
        }
    }
}
