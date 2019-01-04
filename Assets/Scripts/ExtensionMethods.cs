using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class ExtensionMethods
{
    public static Vector3 ZeroY(this Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    public static bool ReachedDestination(this NavMeshAgent agent)
    {
        return (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f));         
    }

    public static float CurrentFrameNumber(this Animator animator, int layerIndex, int clipNumber = 0)
    {
        AnimatorClipInfo[] animationClip = animator.GetCurrentAnimatorClipInfo(layerIndex);
        float frameNumber = (animationClip[0].weight * (animationClip[0].clip.length * animationClip[0].clip.frameRate));

        float frameNumberNew = (animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime * (animationClip[0].clip.length * animationClip[0].clip.frameRate));


        return frameNumberNew;
    }
}
