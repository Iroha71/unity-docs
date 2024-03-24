# Item Manager

## 目次

- [Item Manager](#item-manager)
  - [目次](#目次)
  - [アイテムをスクリプトから使用する参考コード](#アイテムをスクリプトから使用する参考コード)
  - [HPが満タン時にアイテムを消費しない例](#hpが満タン時にアイテムを消費しない例)
  - [装備スロットの追加](#装備スロットの追加)
  - [装備切り替えショートカットの追加](#装備切り替えショートカットの追加)
  - [アイテムウィンドウを増やす](#アイテムウィンドウを増やす)

## アイテムをスクリプトから使用する参考コード

```cs
if (changeEquip.display != null && changeEquip.display.item != null && changeEquip.display.item.type == vItemType.Consumable)
{
    if (changeEquip.useItemInput.GetButtonDown() && changeEquip.display.item.amount > 0)
    {
        OnUseItem(changeEquip.display.item);
    }
}

internal virtual void OnUseItem(vItem item)
{
    onUseItem.Invoke(item);
}
```

## HPが満タン時にアイテムを消費しない例

- アイテム使用デリゲートにメソッドを登録

``` cs
itemManager.canUseItemDelegate += CheckUseItem;
```

- 実装例（↓のスクリプトはInventory > EquipmentDisplayWindow > EquipDisplay_Downにアタッチされている

``` cs
using System.Collections.Generic;

namespace Invector.vItemManager
{
    using UnityEngine;
    using vCharacterController;

    [vClassHeader("Check if can Add Health", "Simple Example to verify if the health item can be used based on the character's health is full or not.", openClose = false)]
    public class vCheckCanAddHealth : vMonoBehaviour
    {
        public vItemManager itemManager;
        public vThirdPersonController tpController;

        public bool getInParent = true;

        protected bool canUse;
        protected bool firstRun;

        protected virtual void Start()
        {
            itemManager = GetComponentInParent<vItemManager>();
            tpController = GetComponentInParent<vThirdPersonController>();

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
                    !firstRun…アイテムの初回仕様時のみ実行
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

## 装備スロットの追加

- `Inventry Master` > `Equipment Area Window` > `EquipmentArea_XX`を複製
- `vEquipArea.cs` > EquipSlotsでスロット数が増やせる
- `EquipmentArea_XX` > EquipmentSlots > EquipSlot > vEquipmentSlot.cs > ItemType変更

## 装備切り替えショートカットの追加

- Inventory Master > `vInventry.cs` > Change Equipment Controllersで要素を追加する

``` cs
// UseItemInput()のif文
&& (changeEquip.display.item.type == vItemType.Consumable 
|| changeEquip.display.item.type == vItemType.Majinai)
```

## アイテムウィンドウを増やす

- Inventory_UI > TopMenuのボタンを増やす
- vInventory_UI > V ToolbarSelectorにウィンドウを追加
- 新ボタンに表示したいウィンドウを設定
- `vItemWindow.CreateEquipWindow(List<vItems>)`を呼び出し、アイテム欄を表示する事ができる
