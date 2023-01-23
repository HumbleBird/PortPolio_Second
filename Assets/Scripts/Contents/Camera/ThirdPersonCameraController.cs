using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    void Start()
    {
        CinemachineFreeLook m_cinemashin = GetComponent<CinemachineFreeLook>();

        GameObject go = Managers.Object.Find(1);
        if (go == null)
            return;

        Transform follow = go.GetComponent<MyPlayer>().followTransform.transform;

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            m_cinemashin.Follow = follow;
            m_cinemashin.LookAt = follow;
        }
    }

    //마우스 커서
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
