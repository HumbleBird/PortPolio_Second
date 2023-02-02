using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager 
{
    public Camera m_Camera;

    public void Init()
    {
        m_Camera = Camera.main;
    }
}
