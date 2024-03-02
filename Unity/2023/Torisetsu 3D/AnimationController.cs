using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator bedAnimator;

    [SerializeField]
    private string animationName;

    [SerializeField]
    private float speedUpVelocity;

    [SerializeField]
    private float speedDownVelocity;

    [SerializeField]
    private float keepTimeAtEnd;

    private bool isRewinding;

    private float defaultAnimationSpeed;

    private void Start()
    {
        defaultAnimationSpeed = bedAnimator.speed;

        bedAnimator.speed = 0;
    }

    public void OnAnimationEnd(string text)
    {
        KeepAnimationAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid KeepAnimationAsync(CancellationToken token)
    {
        StopAnimation();

        await UniTask.Delay(TimeSpan.FromSeconds(keepTimeAtEnd), cancellationToken: token);

        PlayAnimationFromOrigin();
    }

    public void StartAnimation()
    {
        isRewinding = false;

        bedAnimator.SetFloat("Speed", 1f);

        bedAnimator.speed = defaultAnimationSpeed;
    }

    public void StopAnimation()
    {
        bedAnimator.speed = 0;
    }

    public void SpeedUpAnimation()
    {
        isRewinding = false;

        bedAnimator.SetFloat("Speed", 1f);

        bedAnimator.speed = speedUpVelocity * defaultAnimationSpeed;
    }

    public void SlowDownAnimation()
    {
        isRewinding = false;

        bedAnimator.SetFloat("Speed", 1f);

        bedAnimator.speed = defaultAnimationSpeed / speedDownVelocity;
    }

    public void RewindAnimation()
    {
        if (isRewinding) return;

        isRewinding = true;

        bedAnimator.SetFloat("Speed", -1f);

        bedAnimator.speed = defaultAnimationSpeed;
    }

    public void ResetAnimation()
    {
        bedAnimator.Play(animationName, 0, 0);

        bedAnimator.speed = defaultAnimationSpeed;
    }

    public void PlayAnimationFromOrigin()
    {
        isRewinding = false;

        bedAnimator.SetFloat("Speed", 1f);

        bedAnimator.speed = defaultAnimationSpeed;

        AnimatorStateInfo stateInfo = bedAnimator.GetCurrentAnimatorStateInfo(0);

        bedAnimator.Play(stateInfo.fullPathHash, -1, 0f);
    }
}