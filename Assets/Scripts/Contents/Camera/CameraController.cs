using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class CameraController : MonoBehaviour
{
    #region Variable

    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTranform;
    private Transform myTranform;
    private Vector3 cameraTransformPosition;
    private LayerMask ignoreLayers;
    private Vector3 cameraFollwVelocity = Vector3.zero;

    public float lookSpeed = 0.1f;
    public float follwSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPostion;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;

    public float minimumPivot = -35;
    public float maximumPivot = 35;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffSet = 0.2f;

    #endregion

    public float maximunLockOnDistance = 30f;

    public List<Character> m_ListAvilableTarget = new List<Character>();
    public Transform m_trNearestLockOnTarget;
    public Transform m_trCurrentLockOnTarget;

    public bool m_bLockOnFlag = false;

    private void Start()
    {
        Application.targetFrameRate = 60;

        myTranform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10 | 1 << 12);
    }

    public void FollwTarget(float delta)
    {
        Vector3 targetPositoin = Vector3.SmoothDamp(myTranform.position, targetTransform.position, ref cameraFollwVelocity, delta / follwSpeed);
        myTranform.position = targetPositoin;

        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if (!m_bLockOnFlag && m_trCurrentLockOnTarget == null)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTranform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTranform.localRotation = targetRotation;
        }
        else
        {
        }
    }

    private void HandleCameraCollision(float delta)
    {
        targetPostion = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTranform.position;
        direction.Normalize();

        if(Physics.SphereCast
            (cameraPivotTranform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPostion)
            , ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTranform.position, hit.point);
            targetPostion = -(dis - cameraCollisionOffSet);
        }

        if(Mathf.Abs(targetPostion) < minimumCollisionOffSet)
        {
            targetPostion = -minimumCollisionOffSet;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPostion, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        if (!m_bLockOnFlag)
        {
            ClearLockOnTargets();
            LockOn();
            if(m_trNearestLockOnTarget != null)
            {
                m_trCurrentLockOnTarget = m_trNearestLockOnTarget;
                m_bLockOnFlag = true;
            }
        }
        else if (m_bLockOnFlag)
        {
            m_bLockOnFlag = false;
            ClearLockOnTargets();
        }
    }

    public void LockOn()
    {
        GameObject player = Managers.Object.myPlayer.gameObject;

        float shortDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 26);//, (int)Layer.Monster);

        for (int i = 0; i < colliders.Length; i++)
        {
            Character character = colliders[i].GetComponent<Character>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, Managers.Camera.m_Camera.transform.forward);

                if (character.transform.root != player.transform.root
                    && viewableAngle > -50 && viewableAngle < 50
                    && distanceFromTarget <= maximunLockOnDistance)
                {
                    m_ListAvilableTarget.Add(character);
                }
            }
        }

        for (int i = 0; i < m_ListAvilableTarget.Count; i++)
        {
            float distanceFromTarget = Vector3.Distance(player.transform.position, m_ListAvilableTarget[i].transform.position);

            if (distanceFromTarget < shortDistance)
            {
                shortDistance = distanceFromTarget;
                m_trNearestLockOnTarget = m_ListAvilableTarget[i].m_LockOnTransform;
            }
        }
    }

    public void ClearLockOnTargets()
    {
        m_ListAvilableTarget.Clear();
        m_trCurrentLockOnTarget = null;
        m_trNearestLockOnTarget = null;
    }
}
