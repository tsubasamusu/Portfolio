using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx.Triggers;
using UniRx;

public class ScrollbarController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Animator bedAnimator;

    [SerializeField]
    private string animationName;

    [SerializeField]
    private Scrollbar scrollbar;

    private bool isDragging;

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => !isDragging)
            .Subscribe(_ => UpdateScrollbarValue())
            .AddTo(this);
    }

    private void UpdateScrollbarValue()
    {
        AnimatorStateInfo stateInfo = bedAnimator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName(animationName)) return;

        scrollbar.value = stateInfo.normalizedTime % 1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        bedAnimator.Play(animationName, -1, scrollbar.value);
    }
}