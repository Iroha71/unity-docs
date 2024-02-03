using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using Invector.vMelee;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Invector.vShooter;
using System.Threading;

/// <summary>
/// 攻撃時の補正を行う
/// </summary>
public class BattleSenceStrengthen : MonoBehaviour
{
    
    private const float MAX_MATCH_DISTANCE = 5f;
    private const float ATTACK_DISTANCE = 0.5f;
    private vLockOn lockon;
    private Animator anim;
    private vMeleeManager mm;
    
    [SerializeField]
    private vDrawHideShooterWeapons drawHideWeapon;
    [SerializeField, Header("ヒットストップ")]
    private float hitstopTime = 0.1f;
    [SerializeField]
    private bool activateHitStop = true;
    [SerializeField, Header("振動")]
    private float shakeDuration = 0.1f;
    [SerializeField]
    private float shakeStrength = 0.1f;
    [SerializeField]
    private Camera tpCamera;
    private CancellationToken token;

    // Start is called before the first frame update
    void Start()
    {
        token = this.destroyCancellationToken;
        lockon = GetComponentInParent<vLockOn>();
        anim = GetComponentInParent<Animator>();
        mm = GetComponentInParent<vMeleeManager>();
        mm.onDamageHit.AddListener(IgnitHitStop);
        mm.onDamageHit.AddListener((hitinfo) => ShakeCamera(shakeDuration, shakeStrength));
        
        //sc = FindObjectOfType<SceneStateController>();
        //sc.OnChangedState += ReadyWeapon;
    }

    /// <summary>
    /// カメラを一定時間振動させる
    /// </summary>
    /// <param name="duration">振動時間</param>
    /// <param name="strength">振動の強さ</param>
    public void ShakeCamera(float duration, float strength)
    {
        tpCamera.DOKill(true);
        tpCamera.DOShakePosition(shakeDuration, strength, vibrato: 3, randomness: 90f, fadeOut: false);
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
        float distanceFromTarget = Vector3.Distance(player.position, target.position);
        if (distanceFromTarget > MAX_MATCH_DISTANCE)
            return;
        player.LookAt(new Vector3(target.position.x, player.position.y, target.position.z));
        distanceFromTarget -= ATTACK_DISTANCE;
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
        await UniTask.Delay((int)(hitstopTime * 1000f), cancellationToken: token);
        anim.speed = defaultSpeed;
    }
}
