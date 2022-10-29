using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerDetector : MonoBehaviour
{
    [SerializeField]
    GameObject _goPlayer;

    public void Set()
    {
       // Managers.Camera.Shake(100001);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player pc = _goPlayer.GetComponent<Player>();

        if (other.CompareTag("Monster"))
            pc.Attack(other.gameObject);

        gameObject.SetActive(false);
    }
}
