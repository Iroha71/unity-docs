using Invector.vCharacterController;
using Invector.vItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(10)]
public class ItemUseView : MonoBehaviour
{
    [SerializeField]
    private vEquipArea consumableEqupArea;
    [SerializeField]
    private List<vItemSlot> displays;
    [SerializeField]
    private Canvas itemUseView;
    [SerializeField]
    private vInventory inventory;
    private vShooterMeleeInput tpInput;
    [SerializeField]
    private GenericInput openCloseInput = new GenericInput("R", "LB", "LB");
    private const int TOP = 0, RIGHT = 1, DOWN = 2, LEFT = 3;
    public bool isOpened;
    [SerializeField]
    private vChangeInputTypeTrigger changeTrigger;
    private bool isInputMouse = true;
    private int selectedSlotNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.root.TryGetComponent(out tpInput);
        changeTrigger.OnChangeToJoystick.AddListener(() => isInputMouse = false);
        changeTrigger.OnChangeToKeyboard.AddListener(() => isInputMouse = true);
    }

    // Update is called once per frame
    void Update()
    {
        if (openCloseInput.GetButtonDown() && !inventory.lockInventoryInput)
        {
            if (!itemUseView.enabled)
            {
                isOpened = true;
                itemUseView.enabled = true;
                RefreshItemDisplays();
                tpInput.LockCursor(true);
                tpInput.SetLockMeleeInput(true);
                Time.timeScale = 0f;
            }
            else
            {
                CloseMenu();
            }
        }

        if (itemUseView.enabled)
        {
            SelectSlotByAngle(GetInputDirection());

            if (tpInput.weakAttackInput.GetButtonDown() && displays[selectedSlotNumber].item != null)
            {
                UseItemFromNumber(selectedSlotNumber);
                StartCoroutine(DelayCloseMenu());
            }
        }
    }

    /// <summary>
    /// 1フレーム待機後にメニューを閉じる
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayCloseMenu()
    {
        yield return new WaitForEndOfFrame();
        CloseMenu();
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    private void CloseMenu()
    {
        isOpened = false;
        itemUseView.enabled = false;
        tpInput.LockCursor(false);
        tpInput.SetLockCameraInput(false);
        Time.timeScale = 1f;
        tpInput.animator.ResetTrigger("WeakAttack");
        tpInput.SetLockMeleeInput(false);
    }

    /// <summary>
    /// 画面上のカーソル位置をもとに角度を算出する
    /// </summary>
    /// <param name="direction"></param>
    private void SelectSlotByAngle(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360;
        if (angle >= 315 || angle < 45)
        {
            EventSystem.current.SetSelectedGameObject(displays[TOP].gameObject);
            selectedSlotNumber = TOP;
        }
        else if (angle >= 45 && angle < 135)
        {
            EventSystem.current.SetSelectedGameObject(displays[RIGHT].gameObject);
            selectedSlotNumber = RIGHT;
        }
        else if (angle >= 135 && angle < 225)
        {
            EventSystem.current.SetSelectedGameObject(displays[DOWN].gameObject);
            selectedSlotNumber = DOWN;
        }
        else if (angle >= 225 && angle < 315)
        {
            EventSystem.current.SetSelectedGameObject(displays[LEFT].gameObject);
            selectedSlotNumber = LEFT;
        }
    }

    /// <summary>
    /// マウス・パッドの入力値をベクトルに変換する
    /// </summary>
    /// <returns></returns>
    private Vector2 GetInputDirection()
    {
        if (isInputMouse)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            return (mousePosition - screenCenter).normalized;
        }
        else
        {
            return new Vector2(Input.GetAxis("RightAnalogHorizontal"), Input.GetAxis("RightAnalogVertical"));
        }
    }

    /// <summary>
    /// インベントリを閉じた際にアイテム一覧を更新する
    /// </summary>
    /// <param name="isInventoryOpened"></param>
    private void RefreshItemDisplays()
    {
        for (int i = 0; i < displays.Count; i++) 
        {
            vEquipSlot slot = consumableEqupArea.equipSlots[i];
            if (displays[i].item != null)
            {
                displays[i].RemoveItem();
            }
            displays[i].AddItem(slot.item);
        }
    }

    /// <summary>
    /// 指定スロット番号のアイテムを使用する
    /// </summary>
    /// <param name="slotNumber"></param>
    public void UseItemFromNumber(int slotNumber)
    {
        if (displays[slotNumber].item == null) return;

        inventory.onUseItem.Invoke(displays[slotNumber].item);
    }
}
