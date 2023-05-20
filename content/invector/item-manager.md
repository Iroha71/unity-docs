# Item Manager

## 目次

- [装備スロットの追加](#装備スロットの追加)
- [装備切り替えショートカットの追加](#装備切り替えショートカットの追加)
- [アイテムウィンドウを増やす](#アイテムウィンドウを増やす)

## 装備スロットの追加

- `Inventry Master` > `Equipment Area Window` > `EquipmentArea_XX`を複製
- `vEquipArea.cs` > EquipSlotsでスロット数が増やせる
- `EquipmentArea_XX` > EquipmentSlots > EquipSlot > vEquipmentSlot.cs > ItemType変更

## 装備切り替えショートカットの追加

- Inventory Master > `vInventry.cs` > Change Equipment Controllersで要素を追加する

``` vInventory.cs
// UseItemInput()のif文
&& (changeEquip.display.item.type == vItemType.Consumable || changeEquip.display.item.type == vItemType.Majinai)
```

## アイテムウィンドウを増やす

- Inventory_UI > TopMenuのボタンを増やす
- vInventory_UI > V ToolbarSelectorにウィンドウを追加
- 新ボタンに表示したいウィンドウを設定
- `vItemWindow.CreateEquipWindow(List<vItems>)`を呼び出し、アイテム欄を表示する事ができる
