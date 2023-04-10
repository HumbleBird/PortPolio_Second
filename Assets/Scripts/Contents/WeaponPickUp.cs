using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    Weapon weapon;

    public override void Interact(Player player)
    {
        base.Interact(player);

        PickUpItem();
    }

    private void PickUpItem()
    {
        Managers.Object.myPlayer.PlayerCanMove(false);
        Managers.Object.myPlayer.PlayAnimation("Pick Up Item");
        Item item = Item.MakeItem(101);
        Managers.Battle.RewardPlayer(Managers.Object.myPlayer, item);
        Destroy(gameObject);
    }
}
