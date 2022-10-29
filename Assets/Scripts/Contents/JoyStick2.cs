using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick2 : MonoBehaviour
{
    public Image _ball;

    Vector3 m_vInput = Vector3.zero;
    Vector3 m_vPosition = Vector3.zero;
    Vector3 m_vMove = Vector3.zero;

    public void OnDown(PointerEventData evt)
    {
        _ball.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnUp(PointerEventData evt)
    {
         m_vInput = Vector3.zero;
        _ball.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData evt)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_ball.rectTransform,
            evt.position, evt.pressEventCamera, out Vector2 localPoint))
        {
            localPoint.x = localPoint.x / _ball.rectTransform.sizeDelta.x;
            localPoint.y = localPoint.y / _ball.rectTransform.sizeDelta.y;

            m_vInput.x = localPoint.x;
            m_vInput.y = localPoint.y;
            m_vInput.x = 0;

            m_vInput = (m_vInput.magnitude > 1.0f) ? m_vInput.normalized : m_vInput;

            m_vPosition.x = m_vInput.x * (_ball.rectTransform.sizeDelta.x / 2f);
            m_vPosition.y = m_vInput.y * (_ball.rectTransform.sizeDelta.y / 2f);

            _ball.rectTransform.anchoredPosition = m_vPosition;

            _ball.rectTransform.anchoredPosition = m_vPosition;


        }
    }


}
