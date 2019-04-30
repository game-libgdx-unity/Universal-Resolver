using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Built-in a public static shared Instance for singleton pattern
/// In case extended classes use "Awake" method, need to add override to the base method. "Start" are free to write.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;
    static object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T) FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        
                        Debug.Log("An instance of " + typeof(T) +
                                  " is needed in the scene, so '" + singleton +
                                  "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("Using instance already created: " +
                                  _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// Should be set from the inspector
    /// </summary>
    public bool isPersistent;
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            if (Application.isPlaying && isPersistent && !applicationIsQuitting)
            {
                DontDestroyOnLoad(this);
            }
            _instance = GetComponent<T>(); // the same as instance = this;
        }
        else
        {
            DestroyImmediate(gameObject); //this means the "Start" method will not be called.
        }
    }

    static bool applicationIsQuitting = false;

    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}