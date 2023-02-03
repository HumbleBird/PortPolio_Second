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
    public CinemachineVirtualCamera m_CinemachineVirtualCamera;
    public GameObject player;
    public GameObject m_FollwTarget;

	public float rotationPower = 3f;
	public float rotationLerp = 0.5f;

	Vector3 angles;
	float angle;

	private void Start()
    {
        m_Camera = Managers.Camera.m_Camera;

        player = Managers.Object.MyPlayer.gameObject;
		m_FollwTarget = player.GetComponent<MyPlayer>().m_FollwTarget;

        m_CinemachineVirtualCamera.Follow = m_FollwTarget.transform;
    }

    private void Update()
    {
		CameraAngleSet();
	}

    public void CameraAngleSet()
	{
		Vector2 _look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		//Rotate the Follow Target transform based on the input
		transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);


		#region Vertical Rotation
		transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

		angles = transform.localEulerAngles;
		angles.z = 0;

		angle = transform.localEulerAngles.x;

		//Clamp the Up/Down rotation
		if (angle > 180 && angle < 340)
		{
			angles.x = 340;
		}
		else if (angle < 180 && angle > 40)
		{
			angles.x = 40;
		}

		transform.localEulerAngles = angles;
		#endregion
	}

	public void AnimSet()
	{
		//if (move.x == 0 && move.y == 0)
		//{
		//	nextPosition = transform.position;

		//	if (aimValue == 1)
		//	{
		//		//Set the player rotation based on the look transform
		//		transform.rotation = Quaternion.Euler(0, m_FollwTarget.transform.rotation.eulerAngles.y, 0);
		//		//reset the y rotation of the look transform
		//		m_FollwTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
		//	}

		//	return;
		//}
	}
}
