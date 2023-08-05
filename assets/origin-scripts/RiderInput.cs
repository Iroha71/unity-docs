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
    /// HAP�̓��͂��󂯕t����
    /// </summary>
    private void InputHadle()
    {
        if (mountInput.GetButtonDown() && !rider.IsMounting)
        {
            rider.MountAnimal();
            rider.CallAnimalToggle();
        }

        if (mountInput.GetButtonTimer(1) && rider.IsMounting)
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
    /// ��n���̈ړ����@���L�[�ƃX�e�B�b�N�Ő؂�ւ���
    /// </summary>
    /// <param name="isKey">�L�[���͂ɂ��邩</param>
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
