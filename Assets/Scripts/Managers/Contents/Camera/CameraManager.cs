using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager 
{
    public Camera m_camera;
    public Transform m_target;

    public Quaternion CemeraPauseOnUI()
    {
        Quaternion vec = m_camera.transform.rotation;

        if (Cursor.lockState == CursorLockMode.None)
        {
            //vec = new Quaternion(m_target.position.x, m_camera.transform.position.y, m_target.position.z);
            return vec;
        }
        else
        {
            return vec;
        }
    }
}
