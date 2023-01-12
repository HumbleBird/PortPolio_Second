using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
	// 추후에 서버 붙으면 자주 이용할 오브젝트 매니저

	Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

	public void Add(int id, GameObject go)
	{
		_objects.Add(id, go);
	}

	public void Remove(int id)
	{
		_objects.Remove(id);
	}

	public GameObject Find(int id)
    {
		GameObject obj = null;
		_objects.TryGetValue(id, out obj);
		if (obj == null)
			return null;

		return obj;
    }

	//public GameObject Find(Func<GameObject, bool> condition)
	//{
	//	foreach (GameObject obj in _objects)
	//	{
	//		if (condition.Invoke(obj))
	//			return obj;
	//	}

	//	return null;
	//}

	public void Clear()
	{
		_objects.Clear();
	}


}
