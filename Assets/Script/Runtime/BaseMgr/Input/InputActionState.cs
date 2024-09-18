namespace J.Runtime.Input
{

    // 记录InputAction的状态
    public class InputActionState
    {
        public string actionId;
        public bool isEnabled;

        public static InputActionState Create(string actionId, bool isEnabled)
        {
            InputActionState state = new InputActionState();
            state.actionId = actionId;
            state.isEnabled = isEnabled;
            return state;
        }

        public void Dispose()
        {
        }

        public void Clear()
        {
            actionId = string.Empty;
            isEnabled = false;
        }
    }

}