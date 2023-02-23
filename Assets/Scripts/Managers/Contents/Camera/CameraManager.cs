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


    }
}
