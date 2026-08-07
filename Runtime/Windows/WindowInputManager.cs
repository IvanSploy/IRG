using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IRG.Windows
{
    public class WindowInputManager : MonoBehaviour
    {
        public enum InputType
        {
            Toggle,
            Open,
            Close
        }

        [Serializable]
        public struct InputConfig
        {
            public InputActionReference Action;
            public InputType Type;
            public string Window;
        }
        
        [SerializeField] private List<InputConfig> _inputConfigs;

        private void Awake()
        {
            foreach (var config in _inputConfigs)
            {
                switch (config.Type)
                {
                    case InputType.Open:
                        config.Action.action.performed += _ =>
                        {
                            WindowManager.Open(config.Window);
                        };
                        break;
                    case InputType.Close:
                        config.Action.action.performed += _ =>
                        {
                            WindowManager.Close(config.Window);
                        };
                        break;
                    case InputType.Toggle:
                    default:
                        config.Action.action.performed += _ =>
                        {
                            WindowManager.Toggle(config.Window);
                        };
                        break;
                }
            }
        }
    }
}