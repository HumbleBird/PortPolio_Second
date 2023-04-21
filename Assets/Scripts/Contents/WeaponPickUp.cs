using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    private void Start()
    {
        interactableText = "Pick Up Item";
    }

    public override void Interact(Player player)
    {
        base.Interact(player);

        PickUpItem(player);
    }

    private void PickUpItem(Player player)
    {
        // 아이템 지급
        Item item = Item.MakeItem(4);
        Managers.Battle.RewardPlayer(player, item);

        // UI
        UI_Interact UIInteract = Managers.UI.ShowPopupUI<UI_Interact>();
        UIInteract.SetInfo(item.Name, item.iconPath);
        player.UIInteractPost = UIInteract;

        // Player Animation
        player.PlayAnimation("Pick Up Item", true);

        Destroy(gameObject);
    }
}
