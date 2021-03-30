using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[AddComponentMenu("UnityToolbag/Dispatcher")]
public class Dispatcher : MonoBehaviour

{
    private static Dispatcher _instance;
    private static bool _instanceExists;

    private static Thread _mainThread;
    private static object _lockObject = new object();
    private static readonly Queue<Action> _actions = new Queue<Action>();

    public static bool isMainThread
    {
        get
        {
            return Thread.CurrentThread == _mainThread;
        }
    }
    public static void InvokeAsync(Action action)
    {
        if (!_instanceExists)
        {
            Debug.LogError("No Dispatcher exists in the scene. Actions will not be invoked!");
            return;
        }

        if (isMainThread)
        {
            action();
        }
        else
        {
            lock (_lockObject)
            {
                _actions.Enqueue(action);
            }
        }
    }

    public static void Invoke(Action action)
    {
        if (!_instanceExists)
        {
            Debug.LogError("No Dispatcher exists in the scene. Actions will not be invoked!");
            return;
        }

        bool hasRun = false;

        InvokeAsync(() =>
        {
            action();
            hasRun = true;
        });

        while (!hasRun)
        {
            Thread.Sleep(5);
        }
    }

    void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(this);
        }
        else
        {
            _instance = this;
            _instanceExists = true;
            _mainThread = Thread.CurrentThread;
        }
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            _instanceExists = false;
        }
    }

    void Update()
    {
        lock (_lockObject)
        {
            while (_actions.Count > 0)
            {
                _actions.Dequeue()();
            }
        }
    }
}
