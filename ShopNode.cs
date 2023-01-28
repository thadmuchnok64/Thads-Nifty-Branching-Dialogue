using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNode : DialogueExit
{
    //[SerializeField] Shop shop;

    public override bool CheckIfActivatesShop(string name)
    {
        //if (name == null)
         //   name = "";
        //shop.SetUpShop(name);
        return true;
    }

}
