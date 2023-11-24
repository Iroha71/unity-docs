using Invector.vCharacterController;
using MalbersAnimations;
using MalbersAnimations.HAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderInput : MonoBehaviour
{
    [SerializeField]
    private MRider rider;
    [SerializeField]
    private MWeaponManager weaponManager;
    private GenericInput mountInput = new("F", "A", "A");
    private GenericInput attackInput = new("Mouse0", "RT", "RT");
    private GenericInput aimInput = new("Mouse1", "LT", "LT");
    private MalbersInput malbersInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputHadle();
    }

    /// <summary>
    /// HAPの入力を受け付ける
    /// </summary>
    private void InputHadle()
    {
        if (mountInput.GetButtonDown() && !rider.IsMounting)
        {
            rider.MountAnimal();
            rider.CallAnimalToggle();
        }

        if (mountInput.GetButtonTimer(1))
        {
            rider.DismountAnimal();
        }

        if (rider.IsMounting)
        {
            if (attackInput.GetButtonDown())
                weaponManager.MainAttack(true);
            if (aimInput.GetButton())
                weaponManager.Aim_Set(true);
        }
    }

    /// <summary>
    /// 乗馬時の移動方法をキーとスティックで切り替える
    /// </summary>
    /// <param name="isKey">キー入力にするか</param>
    public void SwitchInputType(bool isKey)
    {
        malbersInput = FindObjectOfType<MalbersInput>();
        if (!malbersInput)
            return;

        if (!isKey)
        {
            malbersInput.Horizontal = new InputAxis("LeftAnalogHorizontal", true, true);
            malbersInput.Vertical = new InputAxis("LeftAnalogVertical", true, true);
        }
        else
        {
            malbersInput.Horizontal = new InputAxis("Horizontal", true, true);
            malbersInput.Vertical = new InputAxis("Vertical", true, true);
        }
    }
}
