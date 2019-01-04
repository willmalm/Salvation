using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateDash : LogicalStateMachineBehaviour
{
    private const float INVINCIBILITY_FRAMES = 17;
    private float spirit;
    private PlayerController player;

    protected override void Initialize()
    {
        player = Animator.GetComponent<PlayerController>();
    }

    protected override void Enter()
    {
        Vector3 movingDirection = new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY);
        movingDirection = Quaternion.AngleAxis(player.cameraRotation.y, Vector3.up) * movingDirection;

        if (movingDirection == Vector3.zero)
            movingDirection = player.transform.forward;
        else
            movingDirection.Normalize();

        Quaternion newRotation = Quaternion.LookRotation(movingDirection);

        //player.Material.SetFloat("_Spirit", 1);
        spirit = player.Material.GetFloat("_Spirit");
        player.RemoveStamina(10);
        player.regenStamina = false;
        player.invincible = true;
        player.canRotate = false;
    }

    protected override void Update()
    {
        player.Rigidbody.velocity = Animator.deltaPosition / Time.deltaTime;



        float currentFrame = Animator.CurrentFrameNumber(0);
        
        float time = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float frames = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate * Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        float fFrames = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime * frames;
        float startTime = INVINCIBILITY_FRAMES - 5f;
        float targetTime = INVINCIBILITY_FRAMES;
        float value = Mathf.Clamp(currentFrame, startTime, targetTime) / 5f;

        if(Animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") && Animator.CurrentFrameNumber(0) >= INVINCIBILITY_FRAMES)
            spirit = Mathf.Lerp(1f, 0, value);      

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") && player.invincible && Animator.CurrentFrameNumber(0) >= INVINCIBILITY_FRAMES)
        {
            player.invincible = false;
        }
        else if (Animator.CurrentFrameNumber(0) < INVINCIBILITY_FRAMES)
        {
            player.invincible = true;
            startTime = 0;
            targetTime = 5f;
            value = Mathf.Clamp(frames, startTime, targetTime) / 5f;
            spirit = Mathf.Lerp(0, 1f, value);
            //spirit = Mathf.Lerp(spirit, 1f, 10 * Time.deltaTime);
        }

        //if (!player.invincible)
        //    spirit = Mathf.Lerp(spirit, 0, 10 * Time.deltaTime);
        player.Material.SetFloat("_Spirit", spirit);
    }

    protected override void Exit()
    {
        player.canRotate = true;
        player.regenStamina = true;
    }
}
