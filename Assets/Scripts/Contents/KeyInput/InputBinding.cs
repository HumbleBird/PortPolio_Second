using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using static Define;

/*
  * 날짜 : 2021-01-29 PM 5:25:39
  * 작성자 : Rito

  * 설명 : 게임 내 키보드, 마우스 바인딩 설정 클래스

  * 프로퍼티
    - Bindings : 딕셔너리를 통해 사용자 행동과 KeyCode 연결

  * 메소드
    - ResetAll() : 키보드, 마우스 바인딩을 초기 설정 상태로 되돌리기
    - Bind() : 새로운 바인딩 페어 등록 또는 기존 바인딩 변경
    - ApplyNewBindings() : 새로운 바인딩 세트 적용
    - Save() : 필드를 통해 지정한 경로에 바인딩 세트 저장
    - Load() : 지정한 경로로부터 바인딩 세트 불러와 적용
*/
namespace Rito.InputBindings
{
    /// <summary> 유저 키, 마우스 바인딩 옵션 </summary>
    [Serializable]
    public class InputBinding
    {
        // 저장, 불러오기 시 폴더명, 파일명, 확장자, 고유번호
        public string localDirectoryPath = @"Resources/Data/Input Binding/Presets"; // "Assets/Settings"
        public string fileName = "InputBindingPreset";
        public string extName = "txt";
        public string id = "001";

        public Dictionary<UserAction, KeyCode> Bindings => _bindingDict;
        private Dictionary<UserAction, KeyCode> _bindingDict;

        public bool showDebug = false;

        #region  Constructors
        public InputBinding(bool initalize = true)
        {
            _bindingDict = new Dictionary<UserAction, KeyCode>();

            if (initalize)
            {
                ResetAll();
            }
        }

        public InputBinding(SerializableInputBinding sib)
        {
            _bindingDict = new Dictionary<UserAction, KeyCode>();

            foreach (var pair in sib.bindPairs)
            {
                _bindingDict[pair.key] = pair.value;
            }
        }

        #endregion

        #region Private Methods
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void DebugLog(object msg)
        {
            if (!showDebug) return;
            UnityEngine.Debug.Log(msg);
        }

        #endregion
       
        #region Public Methods
        /// <summary> 새로운 바인딩 설정 적용 </summary>
        public void ApplyNewBindings(InputBinding newBinding)
        {
            _bindingDict = new Dictionary<UserAction, KeyCode>(newBinding._bindingDict);
        }
        /// <summary> 새로운 바인딩 설정 적용 </summary>
        public void ApplyNewBindings(SerializableInputBinding newBinding)
        {
            _bindingDict.Clear();

            foreach (var pair in newBinding.bindPairs)
            {
                _bindingDict[pair.key] = pair.value;
            }
        }

        /// <summary> 초기 상태로 설정 </summary>
        public void ResetAll()
        {
            // Move,  
            Bind(UserAction.Walk, KeyCode.LeftAlt);
            Bind(UserAction.Run_Forward, KeyCode.W);
            Bind(UserAction.Run_Backward, KeyCode.S);
            Bind(UserAction.Run_Left, KeyCode.A);
            Bind(UserAction.Run_Right, KeyCode.D);
            Bind(UserAction.Dash_BackStep_Roll, KeyCode.Space);
            Bind(UserAction.Jump, KeyCode.Space);

            // Camera
            //Bind(UserAction.TiltCameraUp, KeyCode.);
            //Bind(UserAction.TiltCameraDown, KeyCode.);
            Bind(UserAction.TiltCameraLeft_ChangeTargetLeft, KeyCode.R);
            Bind(UserAction.TiltCameraRight_ChangeTargetRight, KeyCode.T);
            Bind(UserAction.CameraReset_LockOn, KeyCode.Q);

            // Switch Equipment
            Bind(UserAction.SwitchSpells,          KeyCode.UpArrow);
            Bind(UserAction.SwitchQuickItems,      KeyCode.DownArrow);
            Bind(UserAction.SwitchRightHandWeapon, KeyCode.RightArrow);
            Bind(UserAction.switchLeftHandWeapon,  KeyCode.LeftArrow);

            // Attack
            Bind(UserAction.Attack_RightHand, KeyCode.Mouse0); //Left Lick
            Bind(UserAction.StrongAttack_RightHand, KeyCode.K); // KeyCode.Mouse0 + KeyCode.LeftShift); // Shitf + Left Click
            Bind(UserAction.Attack_LeftHand, KeyCode.Mouse1); //Right Lick
            Bind(UserAction.StrongAttack_LeftHand, KeyCode.L); // KeyCode.Mouse1 + KeyCode.LeftShift); // Shitf + Right Click
            Bind(UserAction.UseItem, KeyCode.R);
            Bind(UserAction.Interact, KeyCode.E);
            Bind(UserAction.TwoHandWeapon, KeyCode.F);

            // Key Bindings
            Bind(UserAction.OpenMenu, KeyCode.Escape);
            Bind(UserAction.OpenGestureMenu, KeyCode.G);
            //Bind(UserAction.MoveCurser_Up,    KeyCode.UpArrow);
            //Bind(UserAction.MoveCurser_Down,  KeyCode.DownArrow);
            //Bind(UserAction.MoveCurser_Right, KeyCode.RightArrow);
            //Bind(UserAction.MoveCurser_Left,  KeyCode.LeftArrow);
            //Bind(UserAction.Confirm, KeyCode.E);
            //Bind(UserAction.Cancel, KeyCode.Q);
            //Bind(UserAction.SwitchTab_Left, KeyCode.LeftShift + KeyCode.LeftArrow);
            //Bind(UserAction.SwitchTab_Right, KeyCode.LeftShift + KeyCode.RightArrow);
            //Bind(UserAction.Function1, KeyCode.R);





        }

        /// <summary> 바인딩 등록 또는 변경
        /// <para/> - allowOverlap이 false이면 기존 해당 키에 연결된 동작의 키가 존재할 경우 none으로 설정
        /// <para/> - allowOverlap이 true이면 바인딩이 중복되도록 유지
        /// </summary>
        public void Bind(in UserAction action, in KeyCode code, bool allowOverlap = false)
        {
            if (!allowOverlap && _bindingDict.ContainsValue(code))
            {
                var copy = new Dictionary<UserAction, KeyCode>(_bindingDict);

                foreach (var pair in copy)
                {
                    if (pair.Value.Equals(code))
                    {
                        _bindingDict[pair.Key] = KeyCode.None;
#if UNITY_EDITOR
                        DebugLog($"기존 바인딩 제거 : [{pair.Key} - {pair.Value}]");
#endif
                    }
                }
            }

#if UNITY_EDITOR
            if (_bindingDict.ContainsKey(action))
            {
                KeyCode prevCode = _bindingDict[action];
                DebugLog($"바인딩 변경 : [{action}] {prevCode} -> {code}");
            }
            else
            {
                DebugLog($"바인딩 등록 : [{action} - {code}]");
            }
#endif

            _bindingDict[action] = code;
        }

        public override string ToString()
        {
            StringBuilder Sb = new StringBuilder("");

            Sb.Append("Bindings \n");
            foreach (var pair in _bindingDict)
            {
                Sb.Append($"{pair.Key} : {pair.Value}\n");
            }

            return Sb.ToString();
        }

        #endregion
        
        #region File IO Methods
        public bool SaveToFile()
        {
            SerializableInputBinding sib = new SerializableInputBinding(this);
            string jsonStr = JsonUtility.ToJson(sib);

            if (jsonStr.Length < 3)
            {
                Debug.Log("JSON Serialization Error");
                return false;
            }

            DebugLog($"Save : Assets/{localDirectoryPath}/{fileName}_{id}.{extName}\n\n{this}");
            LocalFileIOHandler.Save(jsonStr, localDirectoryPath, $"{fileName}_{id}", extName);

            return true;
        }

        public bool LoadFromFile()
        {
            string jsonStr = LocalFileIOHandler.Load(localDirectoryPath, $"{fileName}_{id}", extName);

            if (jsonStr == null)
            {
                Debug.Log("File Load Error");
                return false;
            }

            var sib = JsonUtility.FromJson<SerializableInputBinding>(jsonStr);
            ApplyNewBindings(sib);

            DebugLog($"Load : Assets/{localDirectoryPath}/{fileName}_{id}.{extName}\n\n{this}");

            return true;
        }

        #endregion
    }
}