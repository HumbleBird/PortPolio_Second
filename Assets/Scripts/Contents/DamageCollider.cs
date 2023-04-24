using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DamageCollider : MonoBehaviour
{
    Collider m_DamageCollider;

    public int m_iCurrentWeaponDamage = 25;

    private void Awake()
    {
        m_DamageCollider = GetComponent<Collider>();
        m_DamageCollider.gameObject.SetActive(true);
        m_DamageCollider.isTrigger = true;
        m_DamageCollider.enabled = false;
    }

    public void EnableDamageColiider()
    {
        m_DamageCollider.enabled = true;

    }

    public void DisableDamageColiider()
    {
        m_DamageCollider.enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hittable")
        {
            Character chatacer = other.GetComponent<Character>();

            if(chatacer != null)
            {
                chatacer.HitEvent(m_iCurrentWeaponDamage);
            }
        }
    }
}
