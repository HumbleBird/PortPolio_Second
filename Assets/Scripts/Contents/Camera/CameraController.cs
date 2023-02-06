using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class CameraController : MonoBehaviour
{
    public Camera m_Camera;
    public CameraEffectController m_CameraEffect;
    public CinemachineFreeLook m_CinemachineFreeLook;
    public GameObject player;
    public GameObject m_FollwTarget;

	private void Start()
    {
        m_Camera = Managers.Camera.m_Camera;
        m_CameraEffect = GetComponentInParent<CameraEffectController>();
        Managers.Camera.m_CameraEffect = m_CameraEffect;

        player = Managers.Object.MyPlayer.gameObject;
		m_FollwTarget = player.GetComponent<MyPlayer>().m_FollwTarget;

		m_CinemachineFreeLook.Follow = m_FollwTarget.transform;
		m_CinemachineFreeLook.LookAt = m_FollwTarget.transform;
    }
}
