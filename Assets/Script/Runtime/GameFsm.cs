using Jam.Core;
using Jam.Runtime.GameFsm;

namespace Jam.Runtime
{

    public partial class G
    {
        private Fsm _fsm;

        private void StartFsm()
        {
            ConfigFsm();
            _fsm.Start<Launch>();
        }

        private void ConfigFsm()
        {
            _fsm = _fsmMgr.CreateFsm(out int id);

            _fsm.AddState(new Launch())
                .AddState(new Splash())
                .AddState(new ResourcePipeline())
                .AddState(new Preload())
                .AddState(new Login())
                .AddState(new Home())
                .AddState(new Combat());

            _fsm.Configure<Launch>().To<Splash>();
            _fsm.Configure<Splash>().To<ResourcePipeline>();
            _fsm.Configure<ResourcePipeline>().To<Preload>();
            _fsm.Configure<Preload>().To<Login>();
            _fsm.Configure<Login>().To<Home>();
            _fsm.Configure<Home>().To<Combat>();
            _fsm.Configure<Combat>().To<Home>();
        }
    }

}