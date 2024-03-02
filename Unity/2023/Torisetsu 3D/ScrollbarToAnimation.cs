using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarToAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator bedAnimator;

    [SerializeField]
    private Scrollbar scrollbar;

    private bool scrollbarIsInteract;

    private void Start()
    {
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChange);

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                AnimatorStateInfo stateInfo = bedAnimator.GetCurrentAnimatorStateInfo(0);

                if (!scrollbarIsInteract) scrollbar.value = stateInfo.normalizedTime % 1;

                if (!Input.GetMouseButton(0)) scrollbarIsInteract = false;
            })
            .AddTo(this);
    }

    private void OnScrollbarValueChange(float value)
    {
        scrollbarIsInteract = true;

        AnimatorStateInfo stateInfo = bedAnimator.GetCurrentAnimatorStateInfo(0);

        bedAnimator.Play(stateInfo.fullPathHash, -1, value);
    }
}