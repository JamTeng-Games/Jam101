using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using J.Core;
using J.Runtime.Input;
using J.Runtime.Res;
using J.Runtime.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class Game : MonoBehaviour
{
    private bool _isInit;
    private static Game _instance;
    public static Game Instance => _instance;

    public Canvas Canvas;

    private static IResMgr _resMgr;
    private static UIMgr _uiMgr;

    public static IResMgr ResMgr => _resMgr;
    public static UIMgr UIMgr => _uiMgr;

    private void Awake()
    {
        _instance = this;
        _resMgr = new AssetMgr();
        _uiMgr = new UIMgr();
    }

    private async void Start()
    {
        _resMgr.Init();
        await _resMgr.WaitForAssetInit();

        // Mgr init
        _uiMgr.Init();

        _isInit = true;
    }

    private void OnApplicationQuit()
    {
        _uiMgr.Shutdown();
        _resMgr.Shutdown();
    }

    private void Update()
    {
        if (!_isInit)
            return;

        float dt = Time.deltaTime;
        JTimer.Tick(dt);
        _uiMgr.Tick(dt);

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            JLog.Debug("A");
            _uiMgr.Show<LoginPanel>(null, UIShowMode.Cover, UILevel.Mid);
        }

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            JLog.Debug("B");
            _uiMgr.Show<TestPanel>(null, UIShowMode.Push, UILevel.Mid);
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            JLog.Debug("C");
            _uiMgr.Show<TestBotPanel>(null, UIShowMode.Cover, UILevel.Bottom);
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            JLog.Debug("D");
            _uiMgr.Close<TestPanel>();
        }
    }

    private void LateUpdate()
    {
    }
}