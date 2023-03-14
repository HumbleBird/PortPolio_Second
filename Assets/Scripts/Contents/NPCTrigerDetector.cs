using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NPCTrigerDetector : MonoBehaviour
{
    NPC npc;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.UI.ShowPopupUI<UI_SelectWindow>();

            npc = transform.GetComponentInParent<NPC>();
            Managers.Battle.EVENTFunction += npc.InputButtonSelect;
            Managers.Battle.ExecutionEventFunction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Managers.UI.ClosePopupUI();

        Managers.Battle.EVENTFunction -= npc.InputButtonSelect;
        npc.EndButtonSelect();
    }

}
