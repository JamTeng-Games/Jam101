using System.Collections.Generic;
using Jam.Core;
using Jam.Runtime.Event;
using UnityEngine.InputSystem;

namespace Jam.Runtime.Input_
{

    public partial class InputMgr : IMgr
    {
        private InputEvent _event;
        private InputData _data;

        // Custom input
        private InputControl _input;
        private List<InputStateSnapshot> _snapshots;

        public InputEvent Event => _event;
        public InputData Data => _data;

        public InputMgr()
        {
            _input = new InputControl();
            _event = new InputEvent();
            _data = new InputData();
            _snapshots = new List<InputStateSnapshot>();
            RegisterAll();
        }

        public void Init()
        {
            EnterMode<DefaultMode>();
        }

        public void Shutdown(bool isAppQuit)
        {
            _event.Clear();
        }

        public void LateTick()
        {
            _data.Reset();
        }

        private void RegisterAll()
        {
            RegisterShortcut();
            RegisterGhost();
        }

        private void UnregisterAll()
        {
            UnregisterShortcut();
            UnregisterGhost();
        }

        public void EnterMode<T>() where T : InputMode, new()
        {
            int findIndex = _snapshots.FindIndex(snap => snap.AttachedMode.Name == typeof(T).Name);
            // 如果已经存在该模式，把该模式移到最后（等同于先退出该模式，再进入该模式）
            if (findIndex != -1)
            {
                JLog.Warning($"InputManager: EnterMode \"{typeof(T).Name}\" already exists");
                ExitMode<T>();
                EnterMode<T>();
                return;
            }

            // 创建新的模式
            InputMode mode = InputMode.Create<T>(_input);
            InputStateSnapshot snapshot = InputStateSnapshot.Create(_input, mode);
            ApplySnapshot(snapshot);
            _snapshots.Add(snapshot);

            // SendEvent
            G.Event.Send(GlobalEventId.InputEnterMode, typeof(T));
        }

        public void ExitMode<T>() where T : InputMode
        {
            int findIndex = _snapshots.FindIndex(snap => snap.AttachedMode.Name == typeof(T).Name);
            // 没找打该模式, 直接返回
            if (findIndex == -1)
            {
                JLog.Warning($"InputManager: ExitMode failed, {typeof(T).Name} not exists");
                return;
            }

            // 找到了该模式
            // 1. 找到该模式之前的模式，从这个模式开始重建所有的输入
            int precedingIndex = findIndex - 1;
            InputStateSnapshot precedingSnapshot = default;
            if (precedingIndex < 0)
            {
                // 如果该模式是第一个模式，直接清空所有输入
                precedingSnapshot = InputStateSnapshot.CreateAllDisableSnapshot(_input);
            }
            else
            {
                precedingSnapshot = _snapshots[precedingIndex];
            }

            _snapshots.RemoveAt(findIndex);

            // 2. 重建所有在该模式之后的模式
            for (int i = findIndex; i < _snapshots.Count; i++)
            {
                InputStateSnapshot snap = _snapshots[i];
                snap.CloneSnapshot(precedingSnapshot);
                precedingSnapshot = snap;
            }

            // 3. 最后应用最后一个模式
            if (_snapshots.Count > 0)
                ApplySnapshot(_snapshots[^1]);

            // SendEvent
            G.Event.Send(GlobalEventId.InputExitMode, typeof(T));
        }

        public void ResetToDefaultMode()
        {
            _snapshots.Clear();
            EnterMode<DefaultMode>();
        }

        public void RefreshSnapshot()
        {
            if (_snapshots.Count == 0)
                return;
            ApplySnapshot(_snapshots[^1]);
        }

        private void ApplySnapshot(InputStateSnapshot snapshot)
        {
            foreach (var (id, actionState) in snapshot.InputActionStates)
            {
                InputAction action = _input.FindAction(id);
                if (actionState.isEnabled)
                {
                    action.Enable();
                }
                else
                {
                    action.Disable();
                }
            }
        }
    }

}