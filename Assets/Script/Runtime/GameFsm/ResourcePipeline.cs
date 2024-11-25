using Jam.Core;
using Jam.Runtime.Event;
using Jam.Runtime.UI_;
using UnityEngine;

namespace Jam.Runtime.GameFsm
{

    public class ResourcePipeline : Fsm.State
    {
        private Fsm _resFsm;

        public Fsm.State CurrentState => _resFsm.CurrentState;

        public ResourcePipeline()
        {
            // Create the state machine
            _resFsm = G.Fsm.CreateFsm(out _);
            _resFsm.AddState(new InitializePackage())
                   .AddState(new UpdatePackageVersion())
                   .AddState(new UpdatePackageManifest())
                   .AddState(new CreatePackageDownloader())
                   .AddState(new DownloadPackageFiles())
                   .AddState(new DownloadPackageOver())
                   .AddState(new ClearPackageCache())
                   .AddState(new UpdateResDone());

            // Configure state transitions
            _resFsm.Configure<InitializePackage>().To<UpdatePackageVersion>();
            _resFsm.Configure<UpdatePackageVersion>().To<UpdatePackageManifest>();
            _resFsm.Configure<UpdatePackageManifest>().To<CreatePackageDownloader>();
            _resFsm.Configure<CreatePackageDownloader>()
                   .To<UpdateResDone>()
                   .To<DownloadPackageFiles>();
            _resFsm.Configure<DownloadPackageFiles>().To<DownloadPackageOver>();
            _resFsm.Configure<DownloadPackageOver>().To<ClearPackageCache>();
            _resFsm.Configure<ClearPackageCache>().To<UpdateResDone>();

            // Set the initial data
            _resFsm.Data.SetString("PackageName", "DefaultPackage");
            _resFsm.Data.SetInt("PlayMode", (int)G.Instance.PlayMode);
            _resFsm.Data.SetString("BuildPipeline", Asset.DefaultBuildPipeline.BuiltinBuildPipeline.ToString());
        }

        public override void OnEnter(Fsm.State fromState)
        {
            OpenStartupPanel();

            JLog.Info("Enter ResourcePipeline");
            _resFsm.Start<InitializePackage>();
        }

        public override void OnExit()
        {
            _resFsm.Stop();
        }

        public override void OnTick(float dt)
        {
            if (_resFsm.CurrentState is UpdateResDone)
            {
                G.Event.Send(GlobalEventId.ResPipelineDone);
                // To preload
                ChangeState<Preload>();
            }
        }

        private void OpenStartupPanel()
        {
            var startUpAsset = Resources.Load<GameObject>("UI/StartupPanel");
            if (startUpAsset != null)
            {
                var startUpPanel = GameObject.Instantiate(startUpAsset, G.Instance.UICanvas.transform);
                startUpPanel.name = "StartupPanel";
                StartupPanel panel = startUpPanel.GetComponent<StartupPanel>();
                G.IOC.Register(panel);
            }
        }
    }

}