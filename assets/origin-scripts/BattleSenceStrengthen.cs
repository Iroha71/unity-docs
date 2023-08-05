using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using Invector.vMelee;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using DG.Tweening;
//using Language.Lua;
using Invector;
using Invector.vShooter;

/// <summary>
/// 攻撃時の補正を行う
/// </summary>
public class BattleSenceStrengthen : MonoBehaviour
{
    private float hitstopTime = 0.2f;
    private float maxMatchDistance = 5f;
    private float attackDistance = 0.5f;
    private vLockOn lockon;
    private Rigidbody rb;
    private Animator anim;
    private vMeleeManager mm;
    [SerializeField]
    private Transform tpCamera;
    //private SceneStateController sc;
    [SerializeField]
    private vDrawHideShooterWeapons drawHideWeapon;

    // Start is called before the first frame update
    void Start()
    {
        lockon = GetComponentInParent<vLockOn>();
        rb = GetComponentInParent<Rigidbody>();
        anim = GetComponentInParent<Animator>();
        mm = GetComponentInParent<vMeleeManager>();
        mm.onDamageHit.AddListener(IgnitHitStop);
        mm.onDamageHit.AddListener((hitinfo) => ShakeCamera(duration: 0.2f, strength: 0.05f));
        
        //sc = FindObjectOfType<SceneStateController>();
        //sc.OnChangedState += ReadyWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void ReadyWeapon(string state)
    {
        if (state.Equals("battle"))
        {
            drawHideWeapon.DrawWeapons();
        }
        else if (state.Equals("normal"))
        {
            await UniTask.Delay(1000);
            drawHideWeapon.HideWeapons();
        }
    }

    /// <summary>
    /// カメラを一定時間振動させる
    /// </summary>
    /// <param name="duration">振動時間</param>
    /// <param name="strength">振動の強さ</param>
    public void ShakeCamera(float duration, float strength)
    {
        StartCoroutine(VibrationCamera(duration, strength));
    }

    /// <summary>
    /// カメラを一定時間ランダムに振動させる
    /// </summary>
    /// <param name="duration">振動時間</param>
    /// <param name="strength">振動の強さ</param>
    /// <returns></returns>
    private IEnumerator VibrationCamera(float duration, float strength)
    {
        Vector3 origin = tpCamera.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float x = origin.x + Random.Range(-1f, 1f) * strength;
            float y = origin.y + Random.Range(-1f, 1f) * strength;
            tpCamera.localPosition = new Vector3(x, y, origin.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        tpCamera.localPosition = origin;
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
        if (distanceFromTarget > maxMatchDistance)
            return;
        player.LookAt(new Vector3(target.position.x, player.position.y, target.position.z));
        distanceFromTarget -= attackDistance;
        Vector3 goal = player.forward * distanceFromTarget + player.position;

        player.DOMove(goal, 0.1f);
    }

    /// <summary>
    /// 攻撃ヒット時にヒットストップを発生させる
    /// </summary>
    /// <param name="hitinfo">ヒット対象情報</param>
    private async void IgnitHitStop(vHitInfo hitinfo)
    {
        if (anim.speed == 0f)
            return;

        float defaultSpeed = anim.speed;
        anim.speed = 0f;
        await UniTask.Delay((int)(hitstopTime * 1000f));
        anim.speed = defaultSpeed;
    }
}
