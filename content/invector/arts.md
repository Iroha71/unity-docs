# 魔術拡張

- [魔術拡張](#魔術拡張)
  - [MPの実装](#mpの実装)
  - [アイテム準備](#アイテム準備)
  - [杖の準備](#杖の準備)
  - [魔術の実行](#魔術の実行)
    - [TIPS](#tips)

## MPの実装

1. `vThirdPersonMotor.cs`へ以下を実装

    ``` cs[vThirdPersonMotor.cs]
    [vEditorToolBar("Arts", order=2)]
    protected int maxMp;
    public virtual int MaxMp { get { return maxMp; } set { maxMp = value; } }

    protected int mp;
    public virtual int Mp { get { return mp; } }

    public void AddMp (int value)
    {
      ~~~~
    }
    ```

## アイテム準備

1. `Arts`のItemTypeを追加
2. `EquipmentArea`を複製
   1. Inventory_UI > EquipmentWindow > EquipmentInventory > EquipmentWindow
3. 複製したEquipmentArea内の`EquipSlot` > ItemTypeを「Arts」へ変更
4. `PickerWindow`を複製
   1. Inventory_UI > EquipmentWindow
5. 複製したPickerWindowの`SupportedItems`を「Arts」に変更
   1. PickerWindow > vItemWindow > Default
6. `EquipDisplay`を複製
   1. Inventory > EquipmentDisplayWindow
7. `ChangeEquipmentControllers`を追加
   1. Inventory > vInventory > Settings

   |設定項目|値|
   |---|---|
   |〇〇Item Input|Next Item Input|
   |EquipArea|複製したEquipMentArea|
   |Display|複製したEquipDisplay|

## 杖の準備

1. `Staff`のItemTypeを作成
2. 左手装備スロット（EquipmentArea_Defence）の`ItemType`へ「Staff」を追加
   1. Inventory_UI > EquipmentWindow > EquipmentInventory > EquipmentWindow
3. `DefencePickerWindow`の`SupportedItems`に「Staff」を追加
4. 左手にartsHandler（空オブジェクト）追加
5. `vItemManager`にartsHandlerを追加
   1. vItemManager > EquipPoints > Custom Handlers > EquipPointName: LeftArm
6. 杖アイテムの`customHandler`に「artsHandler」を設定

## 魔術の実行

- 参考スクリプト

### TIPS

- 装備中のアイテムを取得する

    ``` cs[arts.cs]
    vItem displayItem = FindObjectOfType<vInventory>().changeEquipmentControllers[0].display.item;
    // 添え字は各装備スロットに対応する番号
    ```
