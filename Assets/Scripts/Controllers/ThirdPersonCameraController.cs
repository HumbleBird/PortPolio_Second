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
        Transform follow = go.GetComponent<MyPlayer>().followTransform.transform;

        m_cinemashin.Follow = follow;
        m_cinemashin.LookAt = follow;
    }
}
