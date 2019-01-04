using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateJump : StateMachineBehaviour
{
    private PlayerController player;

    private bool active;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!player)
        {
            player = animator.GetComponent<PlayerController>();
        }

        Vector3 movingDirection = new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY);
        movingDirection = Quaternion.AngleAxis(player.cameraRotation.y, Vector3.up) * movingDirection;

        if (movingDirection == Vector3.zero)
            movingDirection = player.transform.forward;
        else
            movingDirection.Normalize();

        Quaternion newRotation = Quaternion.LookRotation(movingDirection);

        animator.applyRootMotion = false;
        //player.Rigidbody.velocity = player.transform.forward * player.Rigidbody.velocity.magnitude * 1f;

        player.Rigidbody.velocity += Vector3.up * 13;

        player.rotationSpeed = player.jumpingRotation;

        player.RemoveStamina(10);
        player.regenStamina = false;

        active = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {

        //player.Rigidbody.velocity = new Vector3(0, player.Rigidbody.velocity.y, 15f);

        if (!active || GameManager.IsPaused)
            return;

        if (!PlayerActions.Jump)
            player.Rigidbody.velocity += Vector3.up * Physics.gravity.y * 0.7f * Time.fixedDeltaTime;

        if (player.Rigidbody.velocity.y < 0)
        {
            animator.SetBool("Falling", true);
            active = false;
            return;
        }

        Vector3 movingDirection = new Vector3(PlayerActions.MoveX, 0, PlayerActions.MoveY);

        //if (movingDirection != Vector3.zero)
        //    player.Rigidbody.velocity = player.transform.forward * 12f;

        Vector3 newVelocity = Vector3.zero;

        if (movingDirection != Vector3.zero)
            newVelocity = player.transform.forward * movingDirection.magnitude * 12f;

        //newVelocity = new Vector3(newVelocity.x, player.Rigidbody.velocity.y, newVelocity.z);
        //if (movingDirection != Vector3.zero)
        //    player.Rigidbody.velocity += player.transform.forward * 10f * Time.deltaTime;

        //float velocity = new Vector3(player.Rigidbody.velocity.x, 0, player.Rigidbody.velocity.z).magnitude;

        //Vector3 newVelocity = player.transform.forward * velocity;

        player.Rigidbody.velocity = new Vector3(newVelocity.x, player.Rigidbody.velocity.y, newVelocity.z);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        player.rotationSpeed = player.defaultRotation;
        player.regenStamina = true;
    }
}
