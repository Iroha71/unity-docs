using Invector;
using Invector.vCharacterController;
using Invector.vItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UltimateClean;

public class ArtsManager : MonoBehaviour, ISavable
{
    [SerializeField] private Animator anim;
    [SerializeField] private vShooterMeleeInput tpInput;
    [SerializeField] private PlayerStatusHolder statusHolder;
    [SerializeField] private vLockOn lockon;
    [SerializeField] private vThirdPersonController cc;
    private GenericInput artsInput;
    [SerializeField] private SkillDataList skillDataList;
    private SkillDataList runtimeDataList;
    public SkillDataList RuntimeDataList { get { return runtimeDataList; } }
    [SerializeField] private int skillPoint;
    public int SkillPoint { get { return skillPoint; } set { skillPoint = value; } }
    [SerializeField] private vInventory inventory;

    private CancellationToken token;

    [SerializeField] private NotificationLauncher nl;

    // Start is called before the first frame update
    void Start()
    {
        artsInput = GetComponent<vShooterMeleeInput>().blockInput;
        PlayerStatus status = statusHolder.Status as PlayerStatus;
        runtimeDataList = skillDataList;
        skillPoint = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (artsInput.GetButtonDown())
        {
            IgniteArts();
        }
    }

    /// <summary>
    /// ���p�𔭓�����
    /// </summary>
    private async void IgniteArts()
    {
        vItem displayArts = inventory.changeEquipmentControllers[3].display.item;

        if (displayArts == null)
            return;

        // �I�𒆂̖��p��ArtsID�Ō���
        Skill arts = FindSkill(displayArts.attributes[0].value);

        if (arts.isLocked == 1)
            return;
        if (arts.cost > cc.Mp)
            return;

        anim.Play(displayArts.EnableAnim);

        foreach (SkillEffectType effectType in arts.effectTypes)
        {
            GameObject target = GetTarget(effectType);
            if (target == null)
                return;
        }

        foreach (SkillEffectType effectType in arts.effectTypes)
        {
            GameObject target = GetTarget(effectType);

            // effectType���̏���
            switch (effectType.effectType)
            {
                case SkillEffectType.EffectType.HP:
                    target.GetComponent<vIHealthController>().AddHealth(effectType.effectValue);
                    break;
            }
        }
        
        if (displayArts.originalObject != null)
        {
            token = this.destroyCancellationToken;
            await UniTask.Delay((int)(displayArts.enableDelayTime * 1000), cancellationToken: token);
            GameObject _effect = Instantiate(displayArts.originalObject, transform);
        }

        cc.AddMp(-arts.cost);
    }

    /// <summary>
    /// ID�ɂ���ăX�L������������
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>ID����v����X�L��</returns>
    private Skill FindSkill(int id)
    {
        return runtimeDataList.skills.Find(_skill => _skill.id == id);
    }

    /// <summary>
    /// ���p�Ώۂ��擾����
    /// </summary>
    /// <param name="effectType">�G�t�F�N�g���</param>
    /// <returns>�Ώۂ̃I�u�W�F�N�g</returns>
    private GameObject GetTarget(SkillEffectType effectType)
    {
        switch (effectType.targetType)
        {
            case SkillEffectType.TargetType.PLAYER:
                return gameObject;
            case SkillEffectType.TargetType.ENEMY:
                if (lockon.currentTarget != null)
                    return lockon.currentTarget.gameObject;
                else
                    return null;
            default:
                return null;
        }
    }

    public void Save()
    {
        ES3.Save(ConstraintValue.SKILL_POINT, skillPoint);
        ES3.Save(ConstraintValue.SKILL_DATA, runtimeDataList);
    }

    public void Load()
    {
        if (ES3.KeyExists(ConstraintValue.SKILL_DATA))
        {
            runtimeDataList = ES3.Load<SkillDataList>(ConstraintValue.SKILL_DATA);
            skillPoint = ES3.Load<int>(ConstraintValue.SKILL_POINT);
        }
        else
        {
            runtimeDataList = skillDataList;
            skillPoint = 1;
        }
    }
}
