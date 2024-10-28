using Invector.vCharacterController;
using Invector.vMelee;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(5)]
public class StrongAttackCharger : MonoBehaviour
{
    [SerializeField]
    private vShooterMeleeInput tpInput;
    [SerializeField]
    private vMeleeManager meleeManager;
    private int currentLevel = 0;
    private const int MAX_CHARGE_LEVEL = 2;
    private const float CHARGE_COMPLETE_TIME = 2f;
    private float charge = 0f;
    public UnityAction<float> OnChangedValue;
    public UnityAction<int> OnChangedChargeLevel;
    private const int CHARGE_PER_LEVEL = 100;
    [Header("音響"), SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip chargeSound;
    [SerializeField]
    private AudioClip filledSound;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tpInput.cc.OnRoll.AddListener(ResetCharge);
        //meleeManager.onDamageHit.AddListener((hitinfo) => AddCharge(5f));
        tpInput.OnPressingStrongAttack += AddCharge;
        tpInput.OnCanceledStrongAttack += ResetCharge;
        tpInput.OnReleasedStrongAttack += PlayStrongAttack;
    }

    /// <summary>
    /// チャージ量を加算する
    /// </summary>
    public void AddCharge()
    {
        // チャージ開始時
        if (charge <= 0f)
        {
            // 音源はループさせる
            AudioSource _src = Instantiate(audioSource, transform);
            _src.transform.localPosition = Vector3.zero;
            _src.PlayOneShot(chargeSound);
        }

        // 1レベル溜めるまでに何秒かかるか（CHARGE_COMPLETE_TIME）をもとに加算量を計算
        float addValue = 100f / CHARGE_COMPLETE_TIME * Time.deltaTime;
        charge = Mathf.Clamp(addValue + charge, 0f, CHARGE_PER_LEVEL * MAX_CHARGE_LEVEL);
        OnChangedValue?.Invoke(charge);

        if (CanLevelUp())
        {
            AddLevel();
        }
    }

    /// <summary>
    /// チャージレベルを上昇できるか
    /// </summary>
    /// <returns>true＝レベル上昇可能</returns>
    private bool CanLevelUp()
    {
        int level = (int)charge / 100;
        return level > currentLevel;
    }

    /// <summary>
    /// チャージ量に応じた強攻撃を実行する
    /// </summary>
    public void PlayStrongAttack()
    {
        if (charge >= 100f)
        {
            tpInput.animator.SetInteger("ActionState", -1);
            tpInput.TriggerStrongAttack();
        }
        else
        {
            tpInput.TriggerStrongAttack();
        }

        ResetCharge();
        ResetLevel();
    }

    /// <summary>
    /// チャージレベルを上昇させる
    /// </summary>
    public void AddLevel()
    {
        if (currentLevel >= MAX_CHARGE_LEVEL)
            return;

        AudioSource _audioSrc = Instantiate(audioSource, transform.position, Quaternion.identity);
        _audioSrc.PlayOneShot(filledSound);
        currentLevel++;
        OnChangedChargeLevel?.Invoke(currentLevel);
    }

    /// <summary>
    /// チャージレベルを0にする
    /// </summary>
    public void ResetLevel()
    {
        currentLevel = 0;
        OnChangedChargeLevel?.Invoke(0);
    }

    /// <summary>
    /// チャージ量を0にする
    /// </summary>
    public void ResetCharge()
    {
        charge = 0f;
        OnChangedValue?.Invoke(charge);
    }
}
