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

    // ÀåÂø ÇØÁ¦
    public void UnloadWeapon()
    {
        if(currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    // ÀåÂø ÇØÁ¦ ¹× ¸ðµ¨ »èÁ¦
    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    // ¸ðµ¨ ÀåÂø
    public void LoadWeaponModel(Weapon WeaponItem)
    {
        UnloadWeaponAndDestroy();

        if (WeaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject model = Managers.Resource.Instantiate(WeaponItem.m_sPrefabPath);
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
