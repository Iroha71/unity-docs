using Cysharp.Threading.Tasks;
using Invector.vItemManager;
using Invector.vMelee;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class CombatArtsSetter : MonoBehaviour
{
    [SerializeField]
    private CombatArtsWindow view;
    private CombatArtsWindow _view;
    
    private CombatArtsSetModel model;
    private vItemManager itemManager;
    private List<PageParts> allPage = new List<PageParts>();
    private Stack<IOverlapableUI> histories = new Stack<IOverlapableUI>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Viewの生成と削除を行う
    /// </summary>
    /// <param name="isOpen">Viewを生成するか （false: Viewを削除）</param>
    public void OpenCloseWindow(bool isOpen)
    {
        if (isOpen)
        {
            model = new CombatArtsSetModel();
            _view = Instantiate(view);
            RegistEvents(_view, model);
            GoTo("Home");
        }
        else
        {
            model = null;
            histories.Clear();
            
            Destroy(_view.gameObject);
        }
    }

    /// <summary>
    /// ヒストリーバックの実行
    /// 履歴から最新の画面を閉じ、その前の画面を表示する
    /// </summary>
    private void BackHistory()
    {
        IOverlapableUI history = histories.Pop();
        history.Close();
        if (histories.Count > 0)
            histories.Peek().Render();
    }

    /// <summary>
    /// 指定の画面へ遷移する
    /// </summary>
    /// <param name="pageName">遷移先の画面名</param>
    /// <param name="isDeleteHistory">これまでの履歴を削除するか</param>
    public void GoTo(string pageName, bool isDeleteHistory=false)
    {
        PageParts targetPage = allPage.Find(page => page.PageName.Equals(pageName));
        if (histories.Count > 0)
        {
            Debug.Log("close");
            histories.Peek().Close();
        }
        if (targetPage != null)
        {
            if (isDeleteHistory)
                histories = DeleteHistoryBefore(targetPage, histories);
            else
                histories.Push(targetPage);
            targetPage.Render();
        }
    }

    /// <summary>
    /// 指定のページ以前の履歴を削除し、戻る操作で意図しない動作にならないようにする
    /// </summary>
    /// <param name="newPage">現在表示しているページ</param>
    /// <param name="histories">履歴</param>
    /// <returns>履歴削除後の履歴リスト</returns>
    private Stack<IOverlapableUI> DeleteHistoryBefore(PageParts newPage, Stack<IOverlapableUI> histories)
    {
        histories.Clear();
        histories.Push(newPage);

        return histories;
    }

    private void RegistEvents(CombatArtsWindow view, CombatArtsSetModel model)
    {
        allPage = view.GetComponentsInChildren<PageParts>().ToList();
        
        // 武器一覧
        view.RootPage.OnRendered += async () =>
        {
            view.WeaponList.ViewList(GetAllWeapons());
            await UniTask.DelayFrame(1, cancellationToken: view.destroyCancellationToken);
            view.WeaponList.Refresh();
        };
        view.OnInputBack += () => BackHistory();

        view.WeaponList.OnHoveredItem += (item, index)
            => model.CurrentWeapon = item;

        model.OnChangedWeapon += (item) 
            => view.WeaponDescription.UpdateItem(item);

        // 戦技モーダルの表示
        view.WeaponList.OnSelectedItem += (item, index) =>
        {
            GoTo("CombatArtsModal");
        };

        view.CombatArtsModal.OnRendered += () =>
        {
            view.CombatArtsList.ViewList(GetAllCombatArts());
            view.CombatArtsList.Refresh();
        };

        view.CombatArtsList.OnHoveredItem += (item, index) 
            => model.CurrentCombatArts = item;

        view.CombatArtsList.OnSelectedItem += (item, index) =>
        {
            itemManager.TryGetComponent(out vMeleeManager mm);
            model.SetCombatArtsToEquip(model.CurrentWeapon, item, mm);
            BackHistory();
            //GoTo("Home");
        };

        model.OnChangedCombatArts += (item) =>
        {
            view.SelectingArtsDesc.UpdateItem(item);
        };
    }

    /// <summary>
    /// 所持武器の一覧を取得する
    /// </summary>
    /// <returns></returns>
    private List<vItem> GetAllCombatArts()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out itemManager);
        return itemManager.items.Where(item => item.type == vItemType.CombatArts).ToList();
    }

    /// <summary>
    /// 所持戦技の一覧を取得する
    /// </summary>
    /// <returns></returns>
    private List<vItem> GetAllWeapons()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out itemManager);
        return itemManager.items.Where(item => item.type == vItemType.MeleeWeapon).ToList();
    }
}

public class CombatArtsSetModel
{
    private vItem currentWeapon = null;
    public vItem CurrentWeapon 
    {
        get => currentWeapon;
        set
        {
            currentWeapon = value;
            OnChangedWeapon?.Invoke(value);
        }
    }
    public UnityAction<vItem> OnChangedWeapon;

    private vItem currentCombatArts = null;
    public vItem CurrentCombatArts
    {
        get => currentCombatArts;
        set
        {
            currentCombatArts = value;
            OnChangedCombatArts?.Invoke(value);
        }
    }
    public UnityAction<vItem> OnChangedCombatArts;

    /// <summary>
    /// 指定の武器に戦技attributeを追加する
    /// 装備中の武器は再装備し、戦技設定を有効化する
    /// </summary>
    /// <param name="weapon">変更対象の武器</param>
    /// <param name="combatArts">追加する戦技</param>
    /// <param name="meleeManager">プレイヤーのMeleeManager</param>
    public void SetCombatArtsToEquip(vItem weapon, vItem combatArts, vMeleeManager meleeManager)
    {
        vItemAttribute existArts = weapon.GetItemAttribute(vItemAttributes.CombatArts);
        if (existArts == null)
        {
            existArts = new vItemAttribute(vItemAttributes.CombatArts, combatArts.id);
            weapon.attributes.Add(existArts);
        }
        else
        {
            existArts.value = combatArts.id;
        }

        if (meleeManager.rightWeapon != null)
        {
            meleeManager.rightWeapon.TryGetComponent(out vMeleeEquipment equipData);
            equipData.OnEquip(weapon);
        }

        CurrentWeapon = weapon;
    }
}