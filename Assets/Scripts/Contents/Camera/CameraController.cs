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
    public Transform cameraArm;

    public GameObject followTransform;

    private void Start()
    {
        m_Camera = Managers.Camera.m_Camera;

        player = Managers.Object.MyPlayer.gameObject;
        followTransform = player.GetComponent<MyPlayer>().m_FollwTarget;

        cameraArm = followTransform.transform;

        m_CinemachineVirtualCamera.Follow = cameraArm;
    }

    public Vector2 _move;
    public Vector2 _look;
    public float aimValue;
    public float fireValue;

    public Vector3 nextPosition;
    public Quaternion nextRotation;

    public float rotationPower = 3f;
    public float rotationLerp = 0.5f;

    public float speed = 1f;

    private void Update()
    {
        _move = player.GetComponent<MyPlayer>().move;
        _look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        #region Follow Transform Rotation

        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        followTransform.transform.rotation = Quaternion.Euler(camAngle.x - _look.y, camAngle.y + _look.x, camAngle.z);

        Vector3 angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        followTransform.transform.localEulerAngles = angles;
        #endregion

        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (_move.x == 0 && _move.y == 0)
        {
            nextPosition = transform.position;

            if (aimValue == 1)
            {
                //Set the player rotation based on the look transform
                player.transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                //reset the y rotation of the look transform
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            return;
        }
        float moveSpeed = speed / 100f;

        Vector3 position = (player.transform.forward * _move.y * moveSpeed) + (player.transform.right * _move.x * moveSpeed);
        nextPosition = player.transform.position + position;


        //Set the player rotation based on the look transform
        player.transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }
}
