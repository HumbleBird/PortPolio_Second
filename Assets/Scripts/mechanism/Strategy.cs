using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public abstract class Strategy
{
    // 키보드 입력
    protected Dictionary<KeyCode, Action> MaintainkeyDictionary; // 연속성
    protected Dictionary<KeyCode, Action> OnekeyDictionary; // 단발성

    // 오브젝트
    protected Character  m_cGo; // 공격자
    protected Character m_cTarget;  // 피격자
    protected GameObject m_GoProjectile = null; // 투사체

    public void SetInfo(Character character)
    {
        m_cGo = character;
    }

    // 연속성 (쉴드, 앉기, 장전 등)
    public void InputMaintainKey()
    {
        Dictionary<KeyCode, Action> InputKeyDic = new Dictionary<KeyCode, Action>(); // 키 값 저장

        // 키를 누르고 있을 때
        if (Input.anyKey)
        {
            foreach (var dic in MaintainkeyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    if (!InputKeyDic.ContainsKey(dic.Key))
                    {
                        string animName = dic.Value.Method.Name;
                        dic.Value();

                        // 애니메이션 실행
                        animName = m_cGo.ActionAnimation(animName);

                        // 애니메이션 실행전까지 대기
                        m_cGo.FreezeWhileAnimation(animName);

                        // 입력한 값과 함수 임시 저장
                        InputKeyDic.Add(dic.Key, dic.Value);
                    }
                }
            }
        }

        if (InputKeyDic.Count == 0)
            return;

        var removeDic = InputKeyDic.ToArray();

        // 키를 올렸을 때
        foreach (var dic in removeDic)
        {
            if (Input.GetKeyUp(dic.Key))
            {
                string animName = dic.Value.Method.Name;

                animName = m_cGo.ActionAnimation(animName, false);

                m_cGo.FreezeWhileAnimation(animName);

                // 초기화
                if (dic.Key == Managers.InputKey._binding.Bindings[UserAction.Crouch])
                {
                    m_cGo.SetMoveState(MoveState.Walk);
                }
                
                if(dic.Key == Managers.InputKey._binding.Bindings[UserAction.Shield])
                {
                    m_cGo.eMoveState = MoveState.None;
                }

                InputKeyDic.Remove(dic.Key);
            }
        }
    }

    // 단발성(점프, 구르기)
    public virtual void InputOnekey()
    {
        Dictionary<KeyCode, Action> InputKeyDic = new Dictionary<KeyCode, Action>(); // 키 값 저장

        if (Input.anyKeyDown)
        {
            foreach (var dic in OnekeyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    if (!InputKeyDic.ContainsKey(dic.Key))
                    {
                        string animName = dic.Value.Method.Name;

                        InputKeyDic.Add(dic.Key, dic.Value);
                        dic.Value();
                        animName = m_cGo.ActionAnimation(animName);
                        m_cGo.FreezeWhileAnimation(animName);
                    }
                }
            }
        }
    }
}
