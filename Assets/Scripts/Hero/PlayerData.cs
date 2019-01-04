using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalEnums;

public class PlayerData : MonoBehaviour
{
    #region Public Variables
    [Header("Properties")]
    public float walkSpeed;
    public float runSpeed;
    public float sneakSpeed;
    public float dashSpeed;

    public float defaultRotation;
    public float jumpRotation;

    public float lockOnDistance;  

    [Header("Weapon")]
    public WeaponID currentWeapon;
    public WeaponID secondWeapon;

    [Header("Update conditions")]
    public bool isGrounded = false;
    public bool isRunning = false;
    public bool isInvincible = false;

    public bool canRegenStamina = true;
    public bool canMove = true;
    public bool canRotate = true;
    public bool canAttack = true;
    public bool canHeal = true;
    public bool canRun = true;
    #endregion

    #region Private Variables
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Material _material;
    #endregion

    #region Properties
    public Rigidbody Rigidbody
    {
        get
        {
            if (!_rigidbody)
                _rigidbody = GetComponent<Rigidbody>();

            return _rigidbody;
        }
    }    

    public Animator Animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();

            return _animator;
        }
    }   

    public Material Material
    {
        get
        {
            if (_material == null)
                _material = GetComponent<Renderer>().material;

            return _material;
        }
    }
    #endregion

    private void Start ()
    {
        walkSpeed = 0;
        runSpeed = 0;
        sneakSpeed = 0;
        dashSpeed = 0;

        defaultRotation = 0;
	}
}
