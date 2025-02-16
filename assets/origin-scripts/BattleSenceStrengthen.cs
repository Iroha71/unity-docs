using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using Invector.vMelee;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Invector.vShooter;
using System.Threading;
using UnityEngine.AI;
using Invector.vCharacterController.AI;

/// <summary>
/// 攻撃時の補正を行う
/// </summary>
public class BattleSenceStrengthen : MonoBehaviour
{
    
    private const float MAX_MATCH_DISTANCE = 5f;
    private float attackRange = 0.8f;
    public float AttackRange { set { attackRange = value; } get => attackRange; }
    private vLockOn lockon;
    private Animator anim;
    private vMeleeManager mm;
    
    [SerializeField]
    private vDrawHideShooterWeapons drawHideWeapon;
    [SerializeField, Header("ヒットストップ")]
    private float hitstopTime = 0.15f;
    [SerializeField]
    private bool activateHitStop = true;
    public bool ActivateHitStop { get => activateHitStop; set { activateHitStop = value; } }
    [SerializeField, Header("振動")]
    private bool isActivateCameraShake = true;
    [SerializeField]
    private float shakeDuration = 0.1f;
    [SerializeField]
    private float shakeStrength = 0.1f;
    [SerializeField]
    private int shakeCount = 2;
    [SerializeField]
    private Camera tpCamera;
    [SerializeField, Header("敵の振動")]
    private float strength = 0.15f;
    [SerializeField]
    private int vibration = 20;
    private CancellationToken token;
    [SerializeField]
    private vShooterMeleeInput tpInput;
    [SerializeField]
    private float knockBackDistance = 10f;
    [SerializeField]
    private float knockBackTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        token = this.destroyCancellationToken;
        lockon = GetComponentInParent<vLockOn>();
        anim = GetComponentInParent<Animator>();
        mm = GetComponentInParent<vMeleeManager>();
        mm.onDamageHit.AddListener(IgnitHitStop);
        mm.onDamageHit.AddListener(AddKnockBack);
        
        if (isActivateCameraShake)
            mm.onDamageHit.AddListener((hitinfo) => ShakeCamera(shakeDuration, shakeStrength, shakeCount));
        //mm.onDamageHit.AddListener(ShakeHitEnemy);
        
        //sc = FindObjectOfType<SceneStateController>();
        //sc.OnChangedState += ReadyWeapon;
    }

    public void AddKnockBack(vHitInfo hitinfo)
    {
        if (!tpInput.cc.IsAnimatorTag("KnockBack"))
            return;
        Vector3 direction = hitinfo.targetCollider.transform.position - transform.root.position;
        hitinfo.targetCollider.TryGetComponent(out vControlAIMelee ai);
        if (ai == null)
            return;
        Vector3 knockBackDirection = ai.transform.position - transform.root.position;
        Vector3 targetPosition = ai.transform.position + (knockBackDirection.normalized * knockBackDistance);
        ai.isKnockbacking = true;
        ai.DisableAIController();
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, knockBackDistance, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
        }
        ai._rigidbody.DOMove(targetPosition, knockBackTime).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            ai.EnableAIController();
            ai.isKnockbacking = false;
        });
    }

    /// <summary>
    /// カメラを一定時間振動させる
    /// </summary>
    /// <param name="duration">振動時間</param>
    /// <param name="strength">振動の強さ</param>
    public void ShakeCamera(float duration, float strength, int count = 5)
    {
        //tpCamera.DOKill(true
        //tpCamera.DOShakePosition(shakeDuration, strength, vibrato: 3, randomness: 90f, fadeOut: false);
        Sequence seq = DOTween.Sequence(destroyCancellationToken);
        float partDuration = duration / count / 2f;
        float widthHalf = strength / 2f;
        for (int i = 0; i < count - 1; i++)
        {
            seq.Append(tpCamera.transform.DOLocalRotate(new Vector3(-widthHalf, 0f), partDuration));
            seq.Append(tpCamera.transform.DOLocalRotate(new Vector3(widthHalf, 0f), partDuration));
        }
        seq.Append(tpCamera.transform.DOLocalRotate(new Vector3(-widthHalf, 0f), partDuration));
        seq.Append(tpCamera.transform.DOLocalRotate(Vector3.zero, partDuration));
    }

    /// <summary>
    /// 攻撃時にロックオン中の敵へ移動させる
    /// </summary>
    /// <param name="animEvent"></param>
    public void MoveToTarget(string animEvent)
    {
        Transform target = lockon.currentTarget;
        if (!target)
            return;
        Transform player = transform.parent;
        player.LookAt(new Vector3(target.position.x, player.position.y, target.position.z));
        float distanceFromTarget = Vector3.Distance(player.position, target.position);
        if (distanceFromTarget <= attackRange || distanceFromTarget > MAX_MATCH_DISTANCE)
            return;
        
        distanceFromTarget -= attackRange;
        Vector3 goal = player.forward * distanceFromTarget + player.position;

        player.DOMove(goal, 0.1f);
    }

    /// <summary>
    /// 攻撃ヒット時にヒットストップを発生させる
    /// </summary>
    /// <param name="hitinfo">ヒット対象情報</param>
    private async void IgnitHitStop(vHitInfo hitinfo)
    {
        if (anim.speed == 0f || !activateHitStop)
            return;

        float defaultSpeed = anim.speed;
        anim.speed = 0f;
        hitinfo.targetCollider.TryGetComponent(out IDamageReactable enemy);
        //enemy.CallHitstop(hitstopTime);
        await UniTask.Delay((int)(hitstopTime * 1000f), cancellationToken: token);
        anim.speed = defaultSpeed;
    }

    /// <summary>
    /// 攻撃ヒット時にヒットストップを発生させる（画面全体版）
    /// </summary>
    private async void IgniteHitStopToScreen()
    {
        Time.timeScale = 0f;
        await UniTask.Delay(System.TimeSpan.FromSeconds(hitstopTime), DelayType.Realtime);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 攻撃が当たったオブジェクトを振動させる
    /// </summary>
    /// <param name="hitinfo"></param>
    private void ShakeHitEnemy(vHitInfo hitinfo)
    {
        Sequence seq = DOTween.Sequence(hitinfo.targetCollider.gameObject.GetCancellationTokenOnDestroy());
        seq.Append(
            hitinfo.targetCollider.transform.DOShakePosition(
                hitstopTime, 
                strength, 
                vibration, 
                fadeOut: false, 
                randomnessMode: ShakeRandomnessMode.Harmonic
                )
            );
    }
}

public interface IDamageReactable
{
    public void CallHitstop(float stopTime);
}