using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    void Start()
    {
        CinemachineFreeLook m_cinemashin = GetComponent<CinemachineFreeLook>();

        Player player = Managers.Object.MyPlayer;
        if (player == null)
            return;

        Transform follow = player.GetComponent<MyPlayer>().followTransform.transform;

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            m_cinemashin.Follow = follow;
            m_cinemashin.LookAt = follow;
        }
    }
}
