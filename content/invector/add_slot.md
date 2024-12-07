# 装備スロットの追加

- `Inventry Master` > `Equipment Area Window` > `EquipmentArea_XX`を複製
- `vEquipArea.cs` > EquipSlotsでスロット数が増やせる
- `EquipmentArea_XX` > EquipmentSlots > EquipSlot > vEquipmentSlot.cs > ItemType変更

## 装備切り替えショートカットの追加

- Inventory > `vInventry.cs` > Change Equipment Controllersを追加
  - Use Input / Next Inputを設定

- vInventory > UseItemInput()のif文にConsumable以外でもアイテムを使用できるように条件式を変更する

    ``` csharp
    // UseItemInput()のif文
    && (changeEquip.display.item.type == vItemType.Consumable 
    || changeEquip.display.item.type == vItemType.Majinai)
    ```

## アイテムウィンドウを増やす

- Inventory_UI > TopMenuのボタンを増やす
- vInventory_UI > V ToolbarSelectorにウィンドウを追加
- 新ボタンに表示したいウィンドウを設定
- `vItemWindow.CreateEquipWindow(List<vItems>)`を呼び出し、アイテム欄を表示する事ができる
