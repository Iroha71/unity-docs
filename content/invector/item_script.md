# スクリプトでアイテム処理を制御

## アイテムをスクリプトから使用する参考コード

``` csharp
if (changeEquip.display != null && changeEquip.display.item != null && changeEquip.display.item.type == vItemType.Consumable)
{
  if (changeEquip.useItemInput.GetButtonDown() && changeEquip.display.item.amount > 0)
  {
      itemManager.inventory.onUseItem.Invoke(changeEquip.display.item);
  }
}
```

## HPが満タン時にアイテムを消費しない例

- アイテム使用デリゲートにメソッドを登録

``` csharp
itemManager.canUseItemDelegate += CheckUseItem;
```

- 実装例（↓のスクリプトはInventory > EquipmentDisplayWindow > EquipDisplay_Downにアタッチされている

``` csharp
using System.Collections.Generic;

namespace Invector.vItemManager
{
  using UnityEngine;
  using vCharacterController;

  public class vCheckCanAddHealth : vMonoBehaviour
  {
    public vItemManager itemManager;
    public vThirdPersonController tpController;

    protected bool canUse;
    protected bool firstRun;

    protected virtual void Start()
    {
      itemManager = GetComponentInParent<vItemManager>();

      // if a itemManager is founded, we use this event to call our CanUseItem method 
      if (itemManager)
      {
        itemManager.canUseItemDelegate -= new vItemManager.CanUseItemDelegate(CanUseItem);
        itemManager.canUseItemDelegate += new vItemManager.CanUseItemDelegate(CanUseItem);
      }
    }

    protected virtual void OnDestroy()
    {
      if (itemManager)
        // remove the event when this gameObject is destroyed
        itemManager.canUseItemDelegate -= new vItemManager.CanUseItemDelegate(CanUseItem);
    }

    protected virtual void CanUseItem(vItem item, ref List<bool> validateResult)
    {
      // Health属性があるアイテムはチェックを実行
      if (item.GetItemAttribute(vItemAttributes.Health) != null)
      {
        // 体力が全快か確認（全快→アイテムを使用しないようにする：全快ではない→使用）
        var valid = tpController.currentHealth < tpController.maxHealth;

        /** valid != canUse…既存のcanUseと状態が違うとき（体力が全快ではなくなった or 全快になった）
            !firstRun…アイテムの初回アイテム使用時のみ実行（validの値が空であるため）
          */
        if (valid != canUse || !firstRun)
        {
            canUse = valid;
            firstRun = true;
            // trigger a custom text if there is a HUDController in the scene
            vHUDController.instance.ShowText(valid ? "Increase health" : "Can't use " + item.name + " because your health is full", 4f);
        }
        // アイテム使用不可の時はvalidateResult.Addでアイテムを使用しないようにする
        if (!valid)
            validateResult.Add(valid);
      }
    }
  }
}
```