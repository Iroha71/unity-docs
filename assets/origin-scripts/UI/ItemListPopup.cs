using Invector.vItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListPopup : MonoBehaviour, ISelectableUI
{
    [SerializeField]
    private vItemSlot itemSlotTemplate;
    [SerializeField]
    private ScrollRect scrollBar;
    public UnityAction<vItem, int> OnHoveredItem;
    public UnityAction<vItem, int> OnSelectedItem;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// アイテム一覧を表示する
    /// </summary>
    /// <param name="displayItems">表示するアイテムリスト</param>
    public void ViewList(List<vItem> displayItems)
    {
        ClearViewedList();
        int index = 0;
        foreach (vItem item in displayItems)
        {
            vItemSlot _slot = Instantiate(itemSlotTemplate, scrollBar.content);
            _slot.gameObject.name = $"SLOT_{item.name}";
            _slot.AddItem(item);
            _slot.onSelectSlotCallBack += ((item) => OnHoveredItem?.Invoke(item.item, index));
            _slot.onSubmitSlotCallBack += ((item) => OnSelectedItem?.Invoke(item.item, index));
            index++;
            //if (index == 1)
            //    EventSystem.current.SetSelectedGameObject(_slot.gameObject);
        }
    }

    /// <summary>
    /// リスト内のすべてのアイテムを削除する
    /// </summary>
    private void ClearViewedList()
    {
        if (scrollBar.content.childCount <= 0) return;
        
        for (int i = 0; i < scrollBar.content.childCount; i++)
        {
            Destroy(scrollBar.content.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// リストの先頭スロットを取得する
    /// </summary>
    /// <returns>リストが空であればnull</returns>
    public Transform GetFirstSlot()
    {
        if (scrollBar.content.childCount <= 0) return null;

        return scrollBar.content.GetChild(currentIndex);
    }

    public void SelectFirstContent()
    {
        Transform firstSlot = GetFirstSlot();
        if (firstSlot != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSlot.gameObject);
        }
    }

    /// <summary>
    /// リスト内の先頭スロットを選択状態にする
    /// </summary>
    public void Refresh()
    {
        Transform firstslot = GetFirstSlot();
        if (firstslot != null)
        {
            EventSystem.current.SetSelectedGameObject(firstslot.gameObject);
            Debug.Log(EventSystem.current.currentSelectedGameObject);
        }
    }
}
