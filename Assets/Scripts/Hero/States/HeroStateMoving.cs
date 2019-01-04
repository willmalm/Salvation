using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateMoving : StateMachineBehaviour
{
    private PlayerController player;

    private bool getInputs = false;

    private bool active;

    //private bool running = false;

    private bool runningPenalty = false;

    private float dashTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!player)
        {
            player = animator.GetComponent<PlayerController>();
        }

        animator.applyRootMotion = false;
        active = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!active || GameManager.IsPaused)
            return;

        if (!player.grounded && player.Rigidbody.velocity.y < 0)
        {
            animator.SetBool("Falling", true);
            player.Rigidbody.velocity = player.transform.forward * 12f;
            active = false;
            return;
        }

        if (PlayerActions.LightAttack && player.canAttack && SavedData.Instance.stamina.Value > 0)
        {
            //player.Attack("LightAttack0");
            animator.SetTrigger("LightAttack0");
            active = false;
            return;
        }

        //if (PlayerActions.ChargeShot && SavedData.Instance.stamina > 0)
        //    player.fsm.ChangeState(player.playerShooting);

        if (PlayerActions.Jump && SavedData.Instance.stamina.Value > 0)
        {
            animator.SetTrigger("Jump");
            active = false;
            return;
        }

        //if (!player.lockOnTarget && PlayerActions.Run)
        //    player.Run();

        Vector3 movingDirection = new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY);

        bool staminaCost = runningPenalty ? SavedData.Instance.stamina.Value >= SavedData.Instance.stamina.Max : SavedData.Instance.stamina.Value > 0;

        if (PlayerActions.Run && player.canRun && dashTimer >= 0.3f && movingDirection != Vector3.zero)
        {
            runningPenalty = false;
            player.running = true;
            player.regenStamina = false;
            //animator.speed = 1.5f;
        }

        if (PlayerActions.Run)
        {
            dashTimer += Time.deltaTime;
        }

        if (SavedData.Instance.stamina.Value <= 0 || movingDirection == Vector3.zero)
        {
            runningPenalty = true;
            runningPenalty = true;
            player.running = false;
            player.regenStamina = true;
            //animator.speed = 1;
        }

        if (PlayerActions.Dash)
        {
            if (dashTimer < 0.3f && SavedData.Instance.stamina.Value > 0)
                animator.SetTrigger("Dash");

            dashTimer = 0;
            player.running = false;
            player.regenStamina = true;
            //animator.speed = 1;
        }

        

        if (movingDirection == Vector3.zero)
        {
            player.Rigidbody.velocity = Vector3.zero;
            player.Animator.SetFloat("Movement", Mathf.Lerp(player.Animator.GetFloat("Movement"), 0, Time.deltaTime * 13));
        }
        else if (player.running)
        {
            player.Rigidbody.velocity = animator.deltaPosition / Time.deltaTime;
            player.Animator.SetFloat("Movement", Mathf.Lerp(player.Animator.GetFloat("Movement"), 1, Time.deltaTime * 13));
        }
        else
        {
            player.Rigidbody.velocity = animator.deltaPosition / Time.deltaTime;
            player.Animator.SetFloat("Movement", Mathf.Lerp(player.Animator.GetFloat("Movement"), 0.7f, Time.deltaTime * 13));
        }

        if (player.running)
            player.DepleteStamina(5);

        float dot = Vector3.Dot(player.cameraObject.transform.right, player.Rigidbody.velocity.normalized);

        float velocityLeft = player.Rigidbody.velocity.magnitude * dot;

        player.cameraRotation += new Vector3(0, velocityLeft, 0) * 2f * Time.deltaTime;

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.speed = 1;
        dashTimer = 0;
        player.running = false;
    }
}
