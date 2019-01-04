using UnityEngine;

public class HeroStateAttack : LogicalStateMachineBehaviour
{
    private PlayerController player;

    public int damage;

    protected override void Initialize()
    {
        player = Animator.GetComponent<PlayerController>();
    }

    protected override void Enter()
    {
        player.canRotate = false;
        player.regenStamina = false;
        player.canAttack = false;
        player.Rigidbody.velocity = Vector3.zero;

        player.damageTrigger.damageDealt = damage;
    }

    protected override void Update()
    {
        if (GameManager.IsPaused)
            return;

        if (player.lockOnTarget)
            player.transform.rotation = Quaternion.LookRotation(player.lockOnTarget.position.ZeroY() - player.transform.position.ZeroY());

        if (PlayerActions.LightAttack && player.canAttack && SavedData.Instance.stamina.Value > 0)
        {
            Animator.SetTrigger("LightAttack0");
            return;
        }
    }

    protected override void Exit()
    {
        player.canRotate = true;
        player.regenStamina = true;
    }
}
