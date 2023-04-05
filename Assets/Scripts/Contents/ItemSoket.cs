using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSoket : MonoBehaviour
{
    public Transform parentOverride;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;

    public GameObject currentWeaponModel;

    private void Start()
    {
        parentOverride = transform;
    }

    public void UnloadWeapon()
    {
        if(currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeaponModel(Weapon WeaponItem)
    {
        UnloadWeaponAndDestroy();

        if (WeaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject model = Managers.Resource.Instantiate(WeaponItem.m_sPrefabPath) as GameObject;
        if(model != null)
        {
            if(parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localScale = Vector3.one;
            model.transform.localRotation = Quaternion.identity;
        }

        Util.GetOrAddComponent<TrigerDetector>(model);
        currentWeaponModel = model;
    }
}
