/*
 * PlayerController
 * 
 * A collection of data and methods used by player states to control the player character. 
 *
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using GlobalEnums;

public class PlayerController : MonoBehaviour
{
    #region Properties
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();

            return _rigidbody;
        }
    }

    private Animator _animator;

    public Animator Animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();

            return _animator;
        }
    }

    private Material _material;

    public Material Material
    {
        get
        {
            if (_material == null)
                _material = renderer.material;

            return _material;
        }
    }
    #endregion

    #region Variables
    [Header("Player")]
    public float sneakingSpeed = 5;
    public float walkingSpeed = 10;
    public float runningSpeed = 20;
    public float dashingSpeed = 50;
    public float defaultRotation = 360;
    public float jumpingRotation = 90;
    public bool isInvincible = false;

    [Header("Camera")]
    public float cameraSpeed = 90;
    public float aimSpeedMultiplier = 1;
    public float defaultCameraDistance = 30;
    public float shootingCameraDistance = 20;
    public Vector3 defaultCameraOffset = new Vector3(0, 3, 0);
    public Vector3 shootingCameraOffset = new Vector3(0, 4, 0);
    public Vector3 cameraOffset;
    public LayerMask blockCamera;
    public float maxRotation = 70;
    public float minRotation = -65;

    [Header("Lock on")]
    public float lockOnDistance = 20;

    [Header("Dash")]
    public float dashIncreaseRate = 10;
    public float minDashDistance = 5;
    public float maxDashDistance = 20;
    public GameObject dashMarkerPrefab;

    [Header("Weapon")]
    public WeaponID currentWeapon;
    public WeaponID secondWeapon;

    [Header("Shooting")]
    public LayerMask blockShooting;
 
    public GameObject cameraObject;
    public Vector3 cameraRotation = new Vector3(45, 0, 0);

    [Header("Assigned in inspector")]
    public Transform lockOnTarget;
    public GameObject arrowPrefab;
    public GameObject bow;
    public ParticleSystem[] chargeEffects;
    public GameObject crosshairUI;
    public DamageTrigger damageTrigger;
    public Animator healthAnimator;
    public RectTransform healthBar;
    public RectTransform staminaBar;
    public RectTransform penaltyBar;
    public RectTransform lostBar;
    public new Renderer renderer;
    public Transform head;
 
    [Header("Update conditions")]
    public bool grounded = false;
    public bool running = false;
    public bool regenStamina = true;
    public bool invincible = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool canAttack = true;
    public bool canHeal = true;
    public bool canRun = true;    

    public float fallingTime = 0;
    public float rotationSpeed;
    public float shotTimer;
    public bool lerpCamera = false;

    public float staminaDepletion;

    private float healingTimer;
    private bool healing;

    private bool dashing;
    private float dashTimer;
    private float dashDistance;
    private float finalDashDistance;
    private Vector3 dashOrigin;
    private Vector3 dashDestination;

    private float cameraDistance = 30;

    private GameObject dashMarker;

    private Vector3 lastViableDashPosition;

    private Camera mainCamera;
    private GameObject lockOnUI;

    private int targetIndex;
    private List<Transform> availableTargets = new List<Transform>();  

    private Vector3 movingDirection;

    private bool rotateToVelocity = true;

    private float rotationTime = 0;

    private Quaternion oldRotation = Quaternion.identity;
    private Quaternion storedRotation = Quaternion.identity;

    private string currentState;

    private SavedData savedData;

    private Dictionary<string, int> attackDamage;

    private float staminaPenalty = -1;

    private float lostTimer;
    private float healthTimer;

    private Vector3 storedVelocity;

    private List<GameObject> interactables;
    #endregion

    private void OnEnable()
    {
        EventManager.Subscribe("damage", TakeDamage, gameObject);
        EventManager.Subscribe("PauseGame", DisablePlayer);
        EventManager.Subscribe("ContinueGame", EnablePlayer);
        EventManager.Subscribe("AddInteractable", AddInteractable);
        EventManager.Subscribe("RemoveInteractable", RemoveInteractable);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("damage", TakeDamage);
        EventManager.Unsubscribe("PauseGame", DisablePlayer);
        EventManager.Unsubscribe("ContinueGame", EnablePlayer);
        EventManager.Unsubscribe("AddInteractable", AddInteractable);
        EventManager.Unsubscribe("RemoveInteractable", RemoveInteractable);
    }

    private void Start ()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraObject = mainCamera.transform.parent.gameObject;
        lockOnUI = GameObject.FindGameObjectWithTag("LockOnUI");
        lockOnUI.SetActive(false);

        savedData = SavedData.Instance;

        chargeEffects = bow.GetComponentsInChildren<ParticleSystem>();

        attackDamage = new Dictionary<string, int>();

        attackDamage["LightAttack0"] = 40;

        rotationSpeed = defaultRotation;
        cameraDistance = defaultCameraDistance;
        cameraOffset = defaultCameraOffset;

        interactables = new List<GameObject>();

        Animator.SetInteger("WeaponID", (int)savedData.currentWeapon);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate ()
    {
        if (GameManager.IsPaused)
            return;

        LerpCameraPosition();

        RotateCamera();

        grounded = IsGrounded();
    }

    private void Update()
    {
        if (PlayerActions.PauseGame)
            GameManager.IsPaused = !GameManager.IsPaused;

        if (GameManager.IsPaused)
            return;

        if (PlayerActions.StartHeal)
        {
            Interact();
        }

        if (savedData.stamina.Value < 0 && staminaPenalty < 0)
            staminaPenalty = -savedData.stamina.Value;

        if (savedData.stamina.Value > 0 && staminaPenalty > 0)
            staminaPenalty = -1;

        if (savedData.stamina.Value <= 0)
            canRun = false;

        if (savedData.stamina.Value > savedData.stamina.Max * 0.7f)
            canRun = true;

        UIManager.Health = savedData.health.Percentage;
        UIManager.Stamina = savedData.stamina.Percentage;

        if (lostTimer > 0)
            lostTimer -= Time.deltaTime;
        else
            UIManager.StaminaDepletion = Mathf.Lerp(UIManager.StaminaDepletion, UIManager.Stamina, 13 * Time.deltaTime);

        if (regenStamina && savedData.stamina.Value < savedData.stamina.Max)
            AddStamina(savedData.staminaRegen.Value * Time.deltaTime);

        if (healthTimer > 0)
            healthTimer -= Time.deltaTime;
        else
            UIManager.HealthDepletion = Mathf.Lerp(UIManager.HealthDepletion, UIManager.Health, 13 * Time.deltaTime);

        UpdateCameraZoom();
        UpdateAvailableTargets();
        
        RotatePlayer();
       
        if (running)
            dashTimer += Time.deltaTime;

        if (PlayerActions.LockOn)
            LockOn();
    }

    private void LateUpdate()
    {
        if (GameManager.IsPaused)
            return;

        UpdateCameraPosition();
        UpdateLockOn();

        if (lockOnTarget == null)
            return;

        float dot = Vector3.Dot(transform.forward, (lockOnTarget.position - head.position).normalized);
        Quaternion headRotation = Quaternion.LookRotation(lockOnTarget.position);

        if (dot > 0)
            head.rotation = headRotation;
    }

    private void DisablePlayer(object argument)
    {
        Animator.speed = 0;
        Rigidbody.isKinematic = true;
        storedVelocity = Rigidbody.velocity;
    }

    private void EnablePlayer(object argument)
    {
        Animator.speed = 1;
        Rigidbody.velocity = storedVelocity;
        Rigidbody.isKinematic = false;
    }

    private void AddInteractable(object argument)
    {
        interactables.Add((GameObject)argument);
    }

    private void RemoveInteractable(object argument)
    {
        interactables.Remove((GameObject)argument);
    }

    public void AddStamina(float amount)
    {
        float newStamina = savedData.stamina.Value + amount;

        if (newStamina > savedData.stamina.Max)
            newStamina = savedData.stamina.Max;

        savedData.stamina.Value = newStamina;
    }

    public void RemoveStamina(float amount)
    {
        float newStamina = savedData.stamina.Value - amount;

        UIManager.StaminaDepletion = savedData.stamina.Value / savedData.stamina.Max;

        lostTimer = 0.5f;

        savedData.stamina.Value = newStamina;

        canRun = true;
    }

    public void DepleteStamina(float amount)
    {
        float newStamina = SavedData.Instance.stamina.Value - amount * Time.deltaTime;

        UIManager.StaminaDepletion = UIManager.Stamina + amount / savedData.stamina.Max;

        if (UIManager.StaminaDepletion > 1)
        {
            UIManager.StaminaDepletion = 1;
        }

        SavedData.Instance.stamina.Value = newStamina;
    }

    public void Interact()
    {
        if (interactables.Count > 0)
        {
            EventManager.TriggerEvent("Interact", null, interactables[0]);
            interactables.Remove(interactables[0]);
        }
    }

    private float HealthPercentage()
    {
        return SavedData.Instance.health.Value / SavedData.Instance.health.Max;
    }

    private void UpdateDirection()
    {
        Vector3 newDirection = new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY);

        if (newDirection != Vector3.zero)
            movingDirection = newDirection.normalized;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;

        return Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 1.1f, blockCamera);
    }

    public void SwitchWeapon()
    {
        WeaponID temp = savedData.currentWeapon;

        savedData.currentWeapon = savedData.secondWeapon;
        savedData.secondWeapon = temp;
        Animator.SetInteger("WeaponID", (int)savedData.currentWeapon);
    }

    public void PrepareShot()
    {
        regenStamina = false;

        if (lockOnTarget)
        {
            cameraDistance = defaultCameraDistance;
            cameraOffset = defaultCameraOffset;
        }
        else
        {
            cameraDistance = shootingCameraDistance;
            cameraOffset = shootingCameraOffset;
            rotateToVelocity = false;

            RaycastHit hit;

            Vector3 targetDirection;

            if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, shotTimer * 105, blockShooting))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    aimSpeedMultiplier = (float)GameManager.Settings.aimAssist;
                    if (crosshairUI.transform.localScale.x > 0.3f)
                        crosshairUI.transform.localScale -= new Vector3(1, 1, 0) * 2 * Time.deltaTime;
                }
                else
                    aimSpeedMultiplier = 1f;

                targetDirection = cameraObject.transform.position + cameraObject.transform.forward * hit.distance - bow.transform.position;
            }
            else
            {
                if (crosshairUI.transform.localScale.x < 1f)
                    crosshairUI.transform.localScale += new Vector3(1, 1, 0) * 2 * Time.deltaTime;

                aimSpeedMultiplier = 1f;

                targetDirection = cameraObject.transform.position + cameraObject.transform.forward * 100 - bow.transform.position;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void ChargeShot()
    {
        if (!lockOnTarget)
            crosshairUI.SetActive(true);

        if (shotTimer < 1)
            shotTimer += Time.deltaTime;



        foreach (ParticleSystem effect in chargeEffects)
        {
            if (!effect.isPlaying)
                effect.Play();
        }
    }

    public void Shoot()
    {
        foreach (ParticleSystem effect in chargeEffects)
        {
            if (effect.isPlaying)
                effect.Stop();
        }

        if (savedData.stamina.Value > 0)
        {
            Quaternion arrowRotation;

            savedData.stamina.Value -= 40;

            if (lockOnTarget)
                arrowRotation = Quaternion.LookRotation(lockOnTarget.transform.position + Vector3.up - bow.transform.position);
            else
                arrowRotation = Quaternion.LookRotation(transform.forward);

            GameObject arrow = Instantiate(arrowPrefab, bow.transform.position, arrowRotation);
            arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * shotTimer * 200;
        }
    }

    public void ReleaseShot()
    {
        cameraDistance = defaultCameraDistance;
        cameraOffset = defaultCameraOffset;
        crosshairUI.SetActive(false);
        crosshairUI.transform.localScale = new Vector3(1, 1, 1);
        rotateToVelocity = true;
        aimSpeedMultiplier = 1f;

        regenStamina = true;
        shotTimer = 0;
    }

    private void TakeDamage(object argument)
    {
        if (invincible)
            return;

        healthTimer = 1f;
        UIManager.HealthDepletion = savedData.health.Percentage;
        DamageInstance damageInstance = (DamageInstance)argument;

        savedData.health.Value -= damageInstance.damage;

        if (savedData.health.Value <= 0)
        {
            Heal(savedData.health.Max);
            transform.position = savedData.lastCheckpoint.position;
        }

        if (savedData.health.Percentage <= 0.2f)
            healthAnimator.SetBool("Active", true);
        
    }

    public void Run()
    {
        running = true;
    }

    public void Dash()
    {
        if (dashTimer < 0.3f)
        {
            running = false;
            dashTimer = 0;
        }
        else
        {
            running = false;
            dashTimer = 0;
        }
    }

    public void Attack(string attackName)
    {
        if (!canAttack)
            return;

        if (savedData.stamina.Value < 1)
            return;

        damageTrigger.damageDealt = attackDamage[attackName];


        Animator.SetTrigger(attackName);
    }

    public void Heal(float amount)
    {
        float newHealth = savedData.health.Value + amount;
        if (newHealth > savedData.health.Max)
            newHealth = savedData.health.Max;

        savedData.health.Value = newHealth;

        if (savedData.health.Percentage > 0.2f)
            healthAnimator.SetBool("Active", false);
    }

    private void UpdateAvailableTargets()
    {
        availableTargets.Clear();

        foreach (CharacterAbstract character in EntityManager.GetCharacters())
        {
            RaycastHit hit;

            Vector3 directionToCharacter = character.transform.position - transform.position;

            float distanceToCharacter = directionToCharacter.magnitude;

            Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(character.transform.position);

            if (!character.IsHostile())
                continue;

            if (viewPortPosition.x > 1 || viewPortPosition.x < 0 || viewPortPosition.y > 1 || viewPortPosition.y < 0)
                continue;

            if (distanceToCharacter > lockOnDistance)
                continue;

            if (Physics.Raycast(transform.position, directionToCharacter, out hit, distanceToCharacter, blockCamera))
                continue;

            availableTargets.Add(character.transform);
        }
        availableTargets = availableTargets.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).ToList();
        
        
    }

    public void LockOn()
    {
        if (lockOnTarget)
        {
            lockOnTarget = null;
            lockOnUI.SetActive(false);
        }
        else
        {
            if (availableTargets.Count > 0)
            {
                //availableTargets = availableTargets.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).ToList();

                targetIndex = 0;
                lockOnTarget = availableTargets[0];
                lockOnUI.SetActive(true);
            }
        }
    }

    public void UpdateLockOn()
    {
        if (lockOnTarget && Vector3.Distance(transform.position, lockOnTarget.position) > lockOnDistance)
        {
            lockOnTarget = null;
            lockOnUI.SetActive(false);
        }

        if (lockOnTarget)
        {
            lockOnUI.transform.position = mainCamera.WorldToScreenPoint(lockOnTarget.position);
            lockOnUI.GetComponent<Image>().color = Color.white;
            lockOnUI.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        }
        else
        {
            if (availableTargets.Count > 0)
            {
                lockOnUI.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                lockOnUI.transform.position = mainCamera.WorldToScreenPoint(availableTargets[0].position);
                lockOnUI.SetActive(true);
            }
            else
                lockOnUI.SetActive(false);
        }
    }

    private void RotatePlayer()
    {
        if (!canRotate)
            return;

        if(Quaternion.AngleAxis(cameraRotation.y, Vector3.up) * new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY) != Vector3.zero)
            movingDirection = Quaternion.AngleAxis(cameraRotation.y, Vector3.up) * new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY).normalized;

        Vector3 currentDirection = movingDirection;

        if (currentDirection == Vector3.zero)
            return;

        if (!rotateToVelocity)
            return;

        Quaternion newRotation = Quaternion.LookRotation(currentDirection);

        head.localRotation = Quaternion.identity;

        //if (lockOnTarget && !running)
        //{
        //    float dot = Vector3.Dot(transform.forward, (lockOnTarget.position - head.position).normalized);
        //    Quaternion headRotation = Quaternion.LookRotation(lockOnTarget.position);

        //    if (dot > 0)
        //        head.rotation = headRotation;

        //    float x = head.localEulerAngles.x;
        //    float y = head.localEulerAngles.y;
        //    float z = head.localEulerAngles.z;
        //}     
        //else
        //{
        //    newRotation = Quaternion.LookRotation(currentDirection);
        //}

        //float angle = Quaternion.Angle(transform.rotation, newRotation);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    // CAMERA METHODS

    private void RotateCamera()
    {
        if (lockOnTarget)
        {
            Vector3 cameraDirection = (lockOnTarget.transform.position - cameraObject.transform.position).normalized;
            cameraRotation = Quaternion.LookRotation(cameraDirection).eulerAngles;
        }
        else
        {
            cameraRotation += new Vector3(
                -PlayerActions.RotateCameraX * cameraSpeed * Time.fixedDeltaTime * aimSpeedMultiplier, 
                PlayerActions.RotateCameraY * cameraSpeed * Time.fixedDeltaTime * aimSpeedMultiplier, 
                0);

            float mouseSensitivity = (float)GameManager.Settings.mouseSensitivity;

            cameraRotation += new Vector3(
                -mouseSensitivity * Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * aimSpeedMultiplier, 
                mouseSensitivity * Input.GetAxis("Mouse X") * Time.fixedDeltaTime * aimSpeedMultiplier, 
                0);
        }

        if (cameraRotation.x > maxRotation)
            cameraRotation = new Vector3(maxRotation, cameraRotation.y, cameraRotation.z);
        else if (cameraRotation.x < minRotation)
            cameraRotation = new Vector3(minRotation, cameraRotation.y, cameraRotation.z);

        Vector3 cameraOffset = new Vector3(20, 0, 0);

        cameraObject.transform.rotation = Quaternion.Slerp(cameraObject.transform.rotation, 
            Quaternion.Euler(cameraRotation + cameraOffset), 15 * Time.deltaTime);

        //cameraObject.transform.rotation = Quaternion.Euler(cameraRotation + cameraOffset);
        //cameraObject.transform.rotation = Quaternion.RotateTowards(cameraObject.transform.rotation, Quaternion.Euler(cameraRotation + cameraOffset), 720 * Time.fixedDeltaTime);
    }

    public void UpdateCameraZoom()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraObject.transform.position, -cameraObject.transform.forward, out hit, cameraDistance, blockCamera))
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, new Vector3(0, 0, -hit.distance + (hit.distance / cameraDistance) * 1f), Time.deltaTime * 15);
        else
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, new Vector3(0, 0, -cameraDistance), Time.deltaTime * 15);
    }

    private void UpdateCameraPosition()
    {
        
        if (!lerpCamera)
            cameraObject.transform.position = Rigidbody.position + new Vector3(0, cameraOffset.y, 0) + cameraObject.transform.right * cameraOffset.x;
    }

    private void LerpCameraPosition()
    {
        if (lerpCamera)
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, Rigidbody.position + new Vector3(0, cameraOffset.y, 0) + cameraObject.transform.right * cameraOffset.x, 26 * Time.fixedDeltaTime);
    }

    public void ResetCameraDistance()
    {
        cameraDistance = defaultCameraDistance;
    }

    public void SetCameraDistance(float newDistance)
    {
        cameraDistance = newDistance;
    }

    // ATTACK ANIMATION EVENTS

    public void EnableTrigger()
    {
        canRotate = true;
        damageTrigger.Enable();
        RemoveStamina(20);
        canAttack = true;

    }

    public void DisableTrigger()
    {
        canAttack = true;
        canRotate = false;
        damageTrigger.Disable();
    }
}

#region Temporary
//public void Dash()
//{
//    if (InputManager.GetBool("Dash") && !dashing)
//    {
//        RaycastHit hit;

//        canMove = false;
//        canAttack = false;

//        dashDistance += dashIncreaseRate * Time.deltaTime;
//        finalDashDistance = minDashDistance + dashDistance;

//        if (!dashMarker)
//            dashMarker = Instantiate(dashMarkerPrefab);

//        if (!dashMarker.activeSelf)
//            dashMarker.SetActive(true);

//        if (Physics.Raycast(transform.position, transform.forward, out hit, finalDashDistance, blockCamera))
//            finalDashDistance = hit.distance;

//        if (finalDashDistance > maxDashDistance)
//            finalDashDistance = maxDashDistance;

//        dashDestination = transform.position + transform.forward * finalDashDistance;
//        dashMarker.transform.position = dashDestination;
//    }

//    if (!InputManager.GetBool("Dash") && dashDistance > 0 && !dashing)
//    {
//        canRotate = false;
//        dashing = true;

//        dashDistance = 0;
//        finalDashDistance = 0;

//        dashOrigin = transform.position;
//    }

//    if (dashing)
//    {
//        isInvincible = true;

//        dashTimer += Time.deltaTime;

//        transform.position = Vector3.Lerp(dashOrigin, dashDestination, dashTimer);

//        if (dashTimer >= 1f)
//        {
//            isInvincible = false;
//            canMove = true;
//            canAttack = true;
//            canRotate = true;
//            dashing = false;
//            dashMarker.SetActive(false);
//            dashTimer = 0;
//        }
//    }
    //private void MovePlayer()
    //{
    //    if (!IsGrounded())
    //        return;

    //    Vector3 currentDirection = new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY);

    //    float finalSpeed;

    //    if (running)
    //        finalSpeed = runningSpeed;
    //    else if (currentDirection.magnitude < 0.5f)
    //        finalSpeed = sneakingSpeed;
    //    else
    //        finalSpeed = walkingSpeed;

    //    currentDirection.Normalize();

    //    currentDirection = Quaternion.AngleAxis(cameraRotation.y, Vector3.up) * currentDirection;

    //    //Rigidbody.velocity = currentDirection * finalSpeed;
    //}
//}

#endregion