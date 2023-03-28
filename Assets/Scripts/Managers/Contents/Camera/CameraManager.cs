using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager 
{
    public Camera m_Camera = null;
    public CameraEffectController m_CameraEffect;

    public void Init()
    {
        m_Camera = Camera.main;

        m_CameraEffect = m_Camera.GetComponentInParent<CameraEffectController>();
    }

    public void HandleLockOn()
    {
        CameraController cc = m_Camera.GetComponent<CameraController>();
        cc.HandleLockOn();
    }


}
