using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateFall : LogicalStateMachineBehaviour
{
    private PlayerController player;

    protected override void Initialize()
    {
        player = Animator.GetComponent<PlayerController>();
    }

    protected override void Enter()
    {
        player.fallingTime = 0;
        player.Animator.SetFloat("FallToLand", 0);
        player.rotationSpeed = player.jumpingRotation / 2;
    }

    protected override void Update()
    {
        if (GameManager.IsPaused)
            return;

        if (player.grounded)
        {
            Animator.SetBool("Falling", false);
            player.Rigidbody.velocity = Vector3.zero;
            ExitState();
            return;
        }
        else
            player.fallingTime += Time.deltaTime;

        player.Rigidbody.velocity += Vector3.up * Physics.gravity.y * 1f * Time.fixedDeltaTime;

        float velocity = new Vector3(player.Rigidbody.velocity.x, 0, player.Rigidbody.velocity.z).magnitude;

        Vector3 newVelocity = player.transform.forward * velocity;

        player.Rigidbody.velocity = new Vector3(newVelocity.x, player.Rigidbody.velocity.y, newVelocity.z);
    }

    protected override void Exit()
    {
        player.rotationSpeed = player.defaultRotation;
    }
}
