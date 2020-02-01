using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The expected base component class of the this project's in-house components
/// </summary>
public class MyComponent : MonoBehaviour
{
    // These variables describe the setting of the component
    #region Description
    [Tooltip("To decide when the component will set.")]
    [SerializeField]
    protected SetWhenAwakeOptions setWhenAwake = SetWhenAwakeOptions.Yes;
    protected enum SetWhenAwakeOptions
    {
        Yes, // You may consider this option if you just want the component to set itself
        YesWithForce, // You may consider this option for a singleton object that will need to reset when scene changes
        No // You may consider this option if you want other component to call Set() for you.
    }

    #endregion
    // These variables describe the current state of the component
    #region State
    bool _isSet = false;
    public bool IsSet
    {
        get
        {
            return _isSet;
        }
    }
    #endregion

    protected virtual void _set(Dictionary<string, object> args = null)
    {
        // implement component setting here 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="forceSet">Force the component to set even if it is already set</param>
    /// <param name="args">The customized arguments for setting the components</param>
    public void Set(bool forceSet = false, Dictionary<string, object> args = null)
    {
        // check if the script need to set. If already set, ignore requrest unless is a force set.
        if(!forceSet && _isSet)
        {
            return;
        }
        this._set(args);
        _isSet = true;
    }

    protected void Awake()
    {
        if (setWhenAwake == SetWhenAwakeOptions.Yes)
        {
            this.Set();
        }
        else if (setWhenAwake == SetWhenAwakeOptions.YesWithForce)
        {
            this.Set(true);
        }
    }
}
