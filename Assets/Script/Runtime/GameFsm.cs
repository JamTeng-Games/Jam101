using J.Core;
using J.Runtime.GameFsm;

namespace J.Runtime
{
    public partial class Game
    {
        private void ConfigFsm()
        {
            _fsm = new Fsm<Game>(this);

            _fsm.AddState(new Launch());
            _fsm.AddState(new Opening());
            _fsm.AddState(new Login());
            _fsm.AddState(new Home());
            _fsm.AddState(new Combat());
        }

        private void StartFsm()
        {
            _fsm.Start<Launch>();
        }
    }
}