using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MainCamera : MonoBehaviour
{
    public bool m_IsCameraAni = false;
    public bool m_IsFallowMy;
    public float m_ZoomDelta = 0f;

    string m_CameraAnimName;
    float m_StartCameraAniTime;
    float m_PlayCameraAniTime;

    public class cZoomInfo
    {
        public float m_StartDelay;
        public bool m_IsZoom; // m_Dest 값이 확대인지 축소 인지
        public float m_Dest;
        public float m_FadeIn_Velocity;
        public float m_Duration;
        public float m_FadeOut_Velocity;

        public float m_FadeIn_Time;
        public float m_FadeOut_Time;
        public float m_FadeIn_MoveSpeed;
        public float m_FadeOut_MoveSpeed;

        public Vector3 m_DeltaDir;
    }

    cZoomInfo m_ZoomInfo = new cZoomInfo();
    bool m_IsEndStage = false;

    protected void ResetZoom()
    {
        m_ZoomDelta = 0f;
        m_IsEndStage = false;
        m_IsFallowMy = true;
    }

    IEnumerator ZoomCoroutine()
    {
        float dt, delta;

        if(m_ZoomInfo.m_StartDelay > 0)
        {
            yield return new WaitForSeconds(m_ZoomInfo.m_StartDelay);
        }

        Transform trCon = transform.parent.parent;
        bool bMoveCamera = false;

        if(m_ZoomInfo.m_DeltaDir != Vector3.zero)
        {
            bMoveCamera = true;
            m_IsFallowMy = true;
        }

        while (true)
        {
            dt = Time.deltaTime;
            delta = dt * m_ZoomInfo.m_FadeIn_Velocity;

            if (bMoveCamera == true)
            {
                if ((m_ZoomInfo.m_FadeIn_Time -= dt) > 0f)
                {
                    trCon.position += m_ZoomInfo.m_DeltaDir * dt *
                        m_ZoomInfo.m_FadeIn_MoveSpeed;
                }
                else
                {
                    trCon.position += m_ZoomInfo.m_DeltaDir * (m_ZoomInfo.m_FadeIn_Time + dt)
                        * m_ZoomInfo.m_FadeIn_MoveSpeed;

                    bMoveCamera = false;
                }
            }

            if (m_ZoomInfo.m_IsZoom)
            {
                if ((m_ZoomDelta += delta) < m_ZoomInfo.m_Dest)
                {
                    m_ZoomDelta = m_ZoomInfo.m_Dest;
                    break;
                }
            }
            else
            {
                if ((m_ZoomDelta += delta) > m_ZoomInfo.m_Dest)
                {
                    m_ZoomDelta = m_ZoomInfo.m_Dest;
                    break;
                }
            }

            yield return null;
        }

        Debug.Break();

        if(m_ZoomInfo.m_Duration > 0)
        {
            yield return new WaitForSeconds(m_ZoomInfo.m_Duration);
        }

        if(m_ZoomInfo.m_DeltaDir != Vector3.zero)
        {
            bMoveCamera = true;
        }

        while (true)
        {
            dt = Time.deltaTime;
            delta = dt * m_ZoomInfo.m_FadeOut_Velocity;

            if(bMoveCamera == true)
            {
                if((m_ZoomInfo.m_FadeOut_Time -= dt) > 0f)
                {
                    trCon.position -= m_ZoomInfo.m_DeltaDir * dt * m_ZoomInfo.m_FadeOut_MoveSpeed;
                }
                else
                {
                    trCon.position -= m_ZoomInfo.m_DeltaDir * (m_ZoomInfo.m_FadeOut_Time + dt)
                        * m_ZoomInfo.m_FadeOut_MoveSpeed;
                    bMoveCamera = false;
                }
            }

            if (m_ZoomInfo.m_IsZoom)
            {
                if((m_ZoomDelta += delta) > 0)
                {
                    break;
                }
            }
            else
            {
                if ((m_ZoomDelta += delta) < 0)
                {
                    break;
                }
            }

            yield return null;
        }

        ResetZoom();
    }

    IEnumerator ZoomEndStageCoroutine()
    {
        CameraFilterPack_Blur_Focus filter =
            Managers.Camera.gameObject.AddComponent<CameraFilterPack_Blur_Focus>();

        CameraFilterPack_Blur_Focus.ChangeEyes = 20.0f;

        float dt, delta;

        if (m_ZoomInfo.m_StartDelay > 0)
        {
            yield return new WaitForSeconds(m_ZoomInfo.m_StartDelay);
        }

        Transform tr = transform;
        bool bMoveCamera = false;

        if (m_ZoomInfo.m_DeltaDir != Vector3.zero)
        {
            bMoveCamera = true;
        }

        while (true)
        {
            dt = Time.deltaTime;
            delta = dt * m_ZoomInfo.m_FadeIn_Velocity;

            if (CameraFilterPack_Blur_Focus.ChangeEyes > 3.0f)
            {
                CameraFilterPack_Blur_Focus.ChangeEyes -= (17f * dt) / m_ZoomInfo.m_FadeIn_Time;

                if (CameraFilterPack_Blur_Focus.ChangeEyes < 3.0f)
                    CameraFilterPack_Blur_Focus.ChangeEyes = 3.0f;
            }

            if (bMoveCamera == true)
            {
                if ((m_ZoomInfo.m_FadeIn_Time -= dt) > 0f)
                {
                    tr.position += m_ZoomInfo.m_DeltaDir * dt *
                        m_ZoomInfo.m_FadeIn_MoveSpeed;
                }
                else
                {
                    tr.position += m_ZoomInfo.m_DeltaDir * (m_ZoomInfo.m_FadeIn_Time + dt)
                        * m_ZoomInfo.m_FadeIn_MoveSpeed;

                    bMoveCamera = false;
                }
            }

            if (m_ZoomInfo.m_IsZoom)
            {
                if ((m_ZoomDelta += delta) > m_ZoomInfo.m_Dest)
                {
                    m_ZoomDelta = m_ZoomInfo.m_Dest;
                    break;
                }
                m_ZoomDelta += delta;
            }
            else
            {
                if ((m_ZoomDelta += delta) < m_ZoomInfo.m_Dest)
                {
                    m_ZoomDelta = m_ZoomInfo.m_Dest;
                    break;
                }
                m_ZoomDelta += delta;
            }

            yield return null;
        }

        yield return new WaitForSeconds(m_ZoomInfo.m_Duration);

        if (m_ZoomInfo.m_DeltaDir != Vector3.zero)
        {
            bMoveCamera = true;
        }

        while (true)
        {
            dt = Time.deltaTime;
            delta = dt * m_ZoomInfo.m_FadeOut_Velocity;

            if (CameraFilterPack_Blur_Focus.ChangeEyes < 3.0f)
            {
                CameraFilterPack_Blur_Focus.ChangeEyes += (17f * dt) / m_ZoomInfo.m_FadeOut_Time;
            }

            if (bMoveCamera == true)
            {
                if ((m_ZoomInfo.m_FadeOut_Time -= dt) > 0f)
                {
                    tr.position += m_ZoomInfo.m_DeltaDir * dt *
                        m_ZoomInfo.m_FadeOut_MoveSpeed;
                }
                else
                {
                    tr.position += m_ZoomInfo.m_DeltaDir * (m_ZoomInfo.m_FadeOut_Time + dt)
                        * m_ZoomInfo.m_FadeOut_MoveSpeed;

                    bMoveCamera = false;
                }
            }


            if (m_ZoomInfo.m_IsZoom)
            {
                if ((m_ZoomDelta += delta) > 0)
                {
                    m_ZoomDelta = 0;
                    break;
                }
                m_ZoomDelta += delta;
            }
            else
            {
                if ((m_ZoomDelta += delta) < 0)
                {
                    m_ZoomDelta = 0;
                    break;
                }
                m_ZoomDelta += delta;
            }

            yield return null;
        }

        Destroy(filter);
    }

    public void Zoom(int CameraID)
    {
        // 아이디 받아와서 넘기기
    }

    // 카메라 연출, 카메라 앞으로 뒤로만, 오브젝트를 주위를 회전하는 것은 애니메이션으로 처리할 것.
    public void Zoom(float Startdelay, float ZoomDest,
        float BlendInTime, float Duration, float BlendOutTime, Vector3 vDeltaPos) 
    {
        m_ZoomInfo.m_FadeIn_Time = (BlendInTime * Time.timeScale);
        m_ZoomInfo.m_FadeOut_Time = (BlendOutTime * Time.timeScale);

        m_ZoomInfo.m_StartDelay = Startdelay;
        m_ZoomInfo.m_Dest = ZoomDest;
        m_ZoomInfo.m_IsZoom = (m_ZoomInfo.m_Dest < 0);
        m_ZoomInfo.m_Duration = Duration * Time.timeScale;
        m_ZoomInfo.m_FadeIn_Velocity = (m_ZoomInfo.m_Dest) / m_ZoomInfo.m_FadeIn_Time;
        m_ZoomInfo.m_FadeOut_Velocity = (-m_ZoomInfo.m_Dest) / m_ZoomInfo.m_FadeOut_Time;

        if(vDeltaPos != Vector3.zero)
        {
            vDeltaPos.z = 0;
            float lenth = vDeltaPos.magnitude;
            m_ZoomInfo.m_FadeIn_MoveSpeed = lenth / m_ZoomInfo.m_FadeIn_Time;
            m_ZoomInfo.m_FadeOut_MoveSpeed = lenth / m_ZoomInfo.m_FadeOut_Time;
            vDeltaPos.Normalize();
        }
        m_ZoomInfo.m_DeltaDir = vDeltaPos;

        StopCoroutine(ZoomCoroutine());
        ResetZoom();
        StartCoroutine(ZoomCoroutine());
    }

    // 보스 잡았을 때 블러효과를 줌
    public void ZoomEndStage(float Startdelay, float ZoomDest,
        float BlendInTime, float Duration, float BlendOutTime, Vector3 _vec)
    {
        m_IsEndStage = true;

        m_ZoomInfo.m_FadeIn_Time = (BlendInTime * Time.timeScale);
        m_ZoomInfo.m_FadeOut_Time = (BlendOutTime * Time.timeScale);

        m_ZoomInfo.m_StartDelay = Startdelay;
        m_ZoomInfo.m_Dest = ZoomDest;
        m_ZoomInfo.m_IsZoom = (m_ZoomInfo.m_Dest < 0);
        m_ZoomInfo.m_Duration = Duration * Time.timeScale;
        m_ZoomInfo.m_FadeIn_Velocity = (m_ZoomInfo.m_Dest) / m_ZoomInfo.m_FadeIn_Time;
        m_ZoomInfo.m_FadeOut_Velocity = (-m_ZoomInfo.m_Dest) / m_ZoomInfo.m_FadeOut_Time;

        m_ZoomInfo.m_DeltaDir = _vec;

        ResetZoom();

        StartCoroutine(ZoomEndStageCoroutine());
    }
}
