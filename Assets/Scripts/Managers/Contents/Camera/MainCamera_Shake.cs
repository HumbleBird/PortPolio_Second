using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainCamera : MonoBehaviour
{
    private bool m_bCameraShake = false;

    Transform m_ShakeTr;

    public class cShakeInfo
    {
        public float m_StartDelay;
        public bool m_UseTotalTime;
        public float m_TotalTime;

        public Vector3 m_Dest;
        public Vector3 m_Shake;
        public Vector3 m_Dir;

        public float m_RemainDist;
        public float m_fRemainCountDis;

        public bool m_UseCount;
        public int m_Count;

        public float m_Veclocity;

        public bool m_UseDamping;
        public float m_Damping;
        public float m_DampingTime;
    }

    cShakeInfo m_ShakeInfo = new cShakeInfo();

    Vector3 m_vOrgPos; //실제 카메라 위치값

    public float m_CameraFOV_X = 0;
    public float m_CameraFOV_Y = 0;

    public void Awake()
    {
        m_vOrgPos = transform.position;

        InitShake();
    }

    protected void InitShake()
    {
        m_ShakeTr = transform.parent;
        m_bCameraShake = false;
    }

    protected void ResetShakeTr()
    {
        m_ShakeTr.localPosition = Vector3.zero;
        m_bCameraShake = false;

        CameraLimit(); // 보정은 예외
    }

    void CameraLimit(bool _bOrgPosY = false)
    {
        Vector3 camera = m_vOrgPos;

        if (camera.x - m_CameraFOV_X < Managers.Zone.LEFT)
            camera.x = Managers.Zone.LEFT + m_CameraFOV_X;
        else if (camera.x + m_CameraFOV_X > Managers.Zone.RIGHT)
            camera.x = Managers.Zone.RIGHT - m_CameraFOV_X;

        if (_bOrgPosY)
            camera.y = m_vOrgPos.y;
    }

    public void Shake(int _nCameraID)
    {
        Table_Camera.Info info = Managers.Table.m_Camera.Get(_nCameraID);

        if (null == info)
            return;

        m_ShakeInfo.m_StartDelay = info.m_fStartDelay;
        m_ShakeInfo.m_TotalTime = info.m_fTime;
        m_ShakeInfo.m_UseTotalTime = info.m_fTime > 0f;

        //m_ShakeInfo.m_Shake = new Vector3(info.m_fShake_X, info.m_fShake_Y, 0);
        m_ShakeInfo.m_Shake = new Vector3(0.1f, 0.1f, 0);

        m_ShakeInfo.m_Dest = m_ShakeInfo.m_Shake;

        m_ShakeInfo.m_Dir = m_ShakeInfo.m_Shake;
        m_ShakeInfo.m_Dir.Normalize();

        m_ShakeInfo.m_RemainDist = m_ShakeInfo.m_Shake.magnitude;
        m_ShakeInfo.m_fRemainCountDis = float.MaxValue;

        m_ShakeInfo.m_Veclocity = info.m_fSpeed;

        m_ShakeInfo.m_Damping = info.m_fDamping;
        m_ShakeInfo.m_UseDamping = info.m_fDamping > 0;

        if (m_ShakeInfo.m_UseDamping)
            m_ShakeInfo.m_DampingTime = m_ShakeInfo.m_RemainDist / m_ShakeInfo.m_Veclocity;

        m_ShakeInfo.m_Count = info.m_nShakeCount;
        m_ShakeInfo.m_UseCount = info.m_nShakeCount > 0;

        StopCoroutine("ShakeCoroutine");
        ResetShakeTr();
        StartCoroutine("ShakeCoroutine");
    }

    IEnumerator ShakeCoroutine()
    {
        m_bCameraShake = true;

        float dt, dist;

        if (m_ShakeInfo.m_StartDelay > 0)
            yield return new WaitForSeconds(m_ShakeInfo.m_StartDelay);

        while (true)
        {
            dt = Time.fixedDeltaTime;
            dist = dt * m_ShakeInfo.m_Veclocity;

            if((m_ShakeInfo.m_RemainDist -= dist) > 0)
            {
                m_ShakeTr.localPosition += m_ShakeInfo.m_Dir * dist;

                float rc = transform.position.x - m_CameraFOV_X - Managers.Zone.LEFT;

                if (rc < 0)
                    m_ShakeTr.localPosition += new Vector3(-rc, 0, 0);

                rc = Managers.Zone.RIGHT - (transform.position.x + m_CameraFOV_X);

                if (rc < 0)
                    m_ShakeTr.localPosition += new Vector3(rc, 0, 0);

                CameraLimit(true);

                if (m_ShakeInfo.m_UseCount)
                {
                    if ((m_ShakeInfo.m_fRemainCountDis -= dist) < 0) 
                    {
                        m_ShakeInfo.m_fRemainCountDis = float.MaxValue;

                        if (--m_ShakeInfo.m_Count <= 0)
                            break;
                    }
                }
            }
            else
            {
                if (m_ShakeInfo.m_UseDamping)
                {
                    float distdamping = Mathf.Max(m_ShakeInfo.m_Damping *
                        m_ShakeInfo.m_DampingTime,
                        m_ShakeInfo.m_Damping * dt);

                    if (m_ShakeInfo.m_Shake.magnitude > distdamping)
                        m_ShakeInfo.m_Shake -= m_ShakeInfo.m_Dir * distdamping;
                    else
                    {
                        m_ShakeInfo.m_UseCount = true;
                        m_ShakeInfo.m_Count = 1;
                    }
                }

                m_ShakeTr.localPosition = m_ShakeInfo.m_Dest -
                    m_ShakeInfo.m_Dir * (-m_ShakeInfo.m_RemainDist);

                float rc = transform.position.x - m_CameraFOV_X - Managers.Zone.LEFT;

                if (rc < 0)
                    m_ShakeTr.localPosition += new Vector3(-rc, 0, 0);

                rc = Managers.Zone.RIGHT - (transform.position.x + m_CameraFOV_X);

                if (rc < 0)
                    m_ShakeTr.localPosition += new Vector3(rc, 0, 0);

                CameraLimit(true);

                m_ShakeInfo.m_Shake = -m_ShakeInfo.m_Shake;
                m_ShakeInfo.m_Dest = m_ShakeInfo.m_Shake;
                m_ShakeInfo.m_Dir = -m_ShakeInfo.m_Dir;

                float len = m_ShakeInfo.m_Shake.magnitude;

                m_ShakeInfo.m_fRemainCountDis = len + m_ShakeInfo.m_RemainDist;
                m_ShakeInfo.m_RemainDist += len * 2f;

                m_ShakeInfo.m_DampingTime = m_ShakeInfo.m_RemainDist / m_ShakeInfo.m_Veclocity;

                if (m_ShakeInfo.m_RemainDist < dist)
                    break;
            }

            if (m_ShakeInfo.m_UseTotalTime && (m_ShakeInfo.m_TotalTime -= dt) < 0)
                break;

            yield return new WaitForFixedUpdate();
        }

        ResetShakeTr();

        yield break;
    }

}
