using Cysharp.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Asset;
using Jam.Runtime.Event;
using Jam.Runtime.Fsm_;
using Jam.Runtime.Input_;
using Jam.Runtime.IOC;
using Jam.Runtime.ObjectPool;
using Jam.Runtime.Quantum_;
using Jam.Runtime.UI_;
using UnityEngine;

namespace Jam.Runtime
{

    public partial class G : MonoBehaviour
    {
        // Singleton
        private static G _instance;
        public static G Instance => _instance;

        // SerializeField
        public Asset.PlayMode PlayMode;
        public Canvas UICanvas;
        public QuantumChannel QuantumChannel;

        // 字母序排列
        private IAssetMgr _assetMgr;
        private cfg.Tables _cfgMgr; // need asset
        private EventMgr _eventMgr;
        private FsmMgr _fsmMgr;
        private InputMgr _inputMgr;
        private IOCMgr _iocMgr;
        private IObjectPoolMgr _objectPoolMgr;
        private UIMgr _uiMgr; // need asset

        // 字母序排列
        public static IAssetMgr Asset => _instance._assetMgr;
        public static cfg.Tables Cfg => _instance._cfgMgr;
        public static EventMgr Event => _instance._eventMgr;
        public static FsmMgr Fsm => _instance._fsmMgr;
        public static InputMgr Input => _instance._inputMgr;
        public static IOCMgr IOC => _instance._iocMgr;
        public static IObjectPoolMgr ObjectPool => _instance._objectPoolMgr;
        public static UIMgr UI => _instance._uiMgr;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            _instance = this;

            _assetMgr = new YooAssetMgr();
            _cfgMgr = new cfg.Tables();
            _eventMgr = new EventMgr();
            _fsmMgr = new FsmMgr();
            _inputMgr = new InputMgr();
            _iocMgr = new IOCMgr();
            _objectPoolMgr = new ObjectPoolMgr();
            _uiMgr = new UIMgr();
        }

        private void Start()
        {
            // 字母序排列，除非遇到依赖的情况
            // 此时的资源初始化还未完成，不可以加载资源
            _inputMgr.Init();
            _assetMgr.Init();
            _uiMgr.Init();

            StartFsm();
        }

        private void OnApplicationQuit()
        {
            _uiMgr.Shutdown(true);
            _objectPoolMgr.Shutdown(true);
            _inputMgr.Shutdown(true);
            _cfgMgr.Shutdown(true);
            _assetMgr.Shutdown(true);
            _eventMgr.Shutdown(true);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _fsmMgr.Tick(dt);
            _uiMgr.Tick(dt);
            _objectPoolMgr.Tick(dt);
            _eventMgr.Tick(dt);
        }

        private void FixedUpdate()
        {
            _fsmMgr.FixedTick();
        }

        private void LateUpdate()
        {
            _fsmMgr.LateTick();
        }
    }

}