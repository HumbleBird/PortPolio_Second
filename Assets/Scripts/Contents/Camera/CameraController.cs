using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera m_Camera;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_Camera = GetComponent<Camera>();
    }

    public void Update()
    {
        LookAround();
    }

    void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = m_Camera.transform.eulerAngles;

        m_Camera.transform.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y, camAngle.y + mouseDelta.x, camAngle.z);


    }
}
