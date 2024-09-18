using System.Collections.Generic;
using J.Core;
using J.Runtime.Event;
using UnityEngine.InputSystem;

namespace J.Runtime.Input
{

    public static partial class InputMgr
    {
        private static InputEvent _event;

        // Custom input
        private static InputControl _input;
        private static List<InputStateSnapshot> _snapshots;

        public static InputEvent Event => _event;

        public static void Init()
        {
            _input = new InputControl();
            _event = new InputEvent();
            _snapshots = new List<InputStateSnapshot>();
            RegisterAll();
            EnterMode<DefaultMode>();
        }

        public static void Shutdown()
        {
            _event.Clear();
        }

        private static void RegisterAll()
        {
            RegisterShortcut();
            RegisterGhost();
        }

        private static void UnregisterAll()
        {
            UnregisterShortcut();
            UnregisterGhost();
        }

        public static void EnterMode<T>() where T : InputMode, new()
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
            EventMgr.Send(GlobalEventId.InputEnterMode, typeof(T));
        }

        public static void ExitMode<T>() where T : InputMode
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
            EventMgr.Send(GlobalEventId.InputExitMode, typeof(T));
        }

        public static void ResetToDefaultMode()
        {
            _snapshots.Clear();
            EnterMode<DefaultMode>();
        }

        public static void RefreshSnapshot()
        {
            if (_snapshots.Count == 0)
                return;
            ApplySnapshot(_snapshots[^1]);
        }

        private static void ApplySnapshot(InputStateSnapshot snapshot)
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