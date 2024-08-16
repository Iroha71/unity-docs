using Invector.vCharacterController;
using Invector.vItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatArtsWindow : MonoBehaviour
{
    [SerializeField]
    private PageParts rootPage;
    public PageParts RootPage => rootPage;
    [SerializeField]
    private ItemListPopup itemList;
    public ItemListPopup WeaponList => itemList;
    [SerializeField]
    private ItemDescriptionView itemDescriptionView;
    public ItemDescriptionView WeaponDescription => itemDescriptionView;
    [SerializeField, Header("戦技モーダル")]
    private PageParts combatArtsModal;
    public PageParts CombatArtsModal => combatArtsModal;
    [SerializeField]
    private ItemListPopup combatArtsList;
    public ItemListPopup CombatArtsList => combatArtsList;
    [SerializeField]
    private ItemDescriptionView equipingArtsDescription;
    public ItemDescriptionView EquipingArtsDesc => equipingArtsDescription;
    [SerializeField]
    private ItemDescriptionView selectingArtsDesc;
    public ItemDescriptionView SelectingArtsDesc => selectingArtsDesc;

    public UnityAction OnInputBack;
    public UnityAction OnReRendered;

    // Start is called before the first frame update
    protected void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out vShooterMeleeInput tpinput);
        tpinput.SetLockAllInput(true);
        tpinput.ShowCursor(true);
        tpinput.LockCursor(true);
    }

    /// <summary>
    /// 戻るボタン押下
    /// </summary>
    public void CloseInput()
    {
        OnInputBack?.Invoke();
    }

    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out vShooterMeleeInput tpinput);
        tpinput.SetLockAllInput(false);
        tpinput.ShowCursor(false);
        tpinput.LockCursor(false);
    }
}
