using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using J.Core;
using J.Runtime.GameFsm;
using J.Runtime.Input;
using J.Runtime.Res;
using J.Runtime.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace J.Runtime
{
    public partial class Game : MonoBehaviour
    {
        // Singleton
        private static Game _instance;
        public static Game Instance => _instance;

        private bool _isInit;
        private Fsm<Game> _fsm;

        private IResMgr _resMgr;
        private UIMgr _uiMgr;

        public static IResMgr ResMgr => _instance._resMgr;
        public static UIMgr UIMgr => _instance._uiMgr;

        private void Awake()
        {
            _resMgr = new AssetMgr();
            _uiMgr = new UIMgr();
            ConfigFsm();
        }

        private async void Start()
        {
            await InitEssential();
            _isInit = true;
            StartFsm();
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
            _fsm.Tick(dt);
            _uiMgr.Tick(dt);
        }

        private void FixedUpdate()
        {
            _fsm.FixedTick();
        }

        private void LateUpdate()
        {
            _fsm.LateTick();
        }

        private async UniTask InitEssential()
        {
            _resMgr.Init();
            await _resMgr.WaitForAssetInit();
            _uiMgr.Init();
        }
    }
}