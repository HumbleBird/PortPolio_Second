using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class CursorController : MonoBehaviour
{
	int _mask = (1 << (int)Layer.Ground) | (1 << (int)Layer.Monster);
	int m_UIMask = ((int)Layer.UI);
	Texture2D m_LootIcon;
	Texture2D m_ArrowIcon;

	CursorType _cursorType = CursorType.None;

	void Start()
    {
		m_LootIcon = Managers.Resource.Load<Texture2D>("Art/Textures/Cursors/UseCursor/Loot");
		m_ArrowIcon = Managers.Resource.Load<Texture2D>("Art/Textures/Cursors/UseCursor/Arrow");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    void Update()
    {
		if (Input.GetMouseButton(0))
			return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// 인게임
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100.0f, _mask))
		{
			//if (hit.collider.gameObject.layer == (int)Layer.UI)
			//{
			//	if (_cursorType != CursorType.Arrow)
			//	{
			//		Cursor.SetCursor(m_ArrowIcon, new Vector2(m_ArrowIcon.width / 5, 0), CursorMode.Auto);
			//		_cursorType = CursorType.Arrow;
			//	}
			//}
            //else
            //{
            //    if (_cursorType != CursorType.Hand)
            //    {
            //        Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
            //        _cursorType = CursorType.Hand;
            //    }
            //}
        }

		// UI
		if(IsPointerOverUIElement() == true)
        {
            if (_cursorType != CursorType.Arrow)
            {
                Cursor.SetCursor(m_ArrowIcon, new Vector2(m_ArrowIcon.width / 5, 0), CursorMode.Auto);
                _cursorType = CursorType.Arrow;
            }
        }
		print(IsPointerOverUIElement() ? "Over UI" : "Not over UI");
	}

	public bool IsPointerOverUIElement()
	{
		return IsPointerOverUIElement(GetEventSystemRaycastResults());
	}

	//Returns 'true' if we touched or hovering on Unity UI element.
	private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
	{
		for (int index = 0; index < eventSystemRaysastResults.Count; index++)
		{
			RaycastResult curRaysastResult = eventSystemRaysastResults[index];
			if (curRaysastResult.gameObject.layer == m_UIMask)
				return true;
		}
		return false;
	}


	//Gets all event system raycast results of current mouse or touch position.
	static List<RaycastResult> GetEventSystemRaycastResults()
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = Input.mousePosition;
		List<RaycastResult> raysastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raysastResults);
		return raysastResults;
	}

	public static void MouseCurserLockOnOff(bool on)
    {
		// Lock On
		if (on == true)
        {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			// 마우스 위치 조정
		}
		// Lock Off
		else
        {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
    }
}
