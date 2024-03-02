using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Linq;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class LookTranController : MonoBehaviour
{
    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private TapEffectController tapEffectPrefab;

    [SerializeField]
    private RectTransform canvasRectTran;

    [SerializeField]
    private float rayLength;

    [SerializeField]
    private float moveTime;

    private Vector3 defaultLookTranPos;

    private void Start()
    {
        defaultLookTranPos = transform.position;

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && Input.touchSupported && Input.touchCount == 1 && !cameraController.TouchingUI())
            .Subscribe(_ => UpdateLookTranPosAsync(this.GetCancellationTokenOnDestroy()).Forget())
            .AddTo(this);
    }

    private async UniTaskVoid UpdateLookTranPosAsync(CancellationToken token)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Moved) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

        if (!Physics.Raycast(ray, out RaycastHit hit, rayLength)) return;

        transform.DOMove(hit.point, moveTime)
            .OnComplete(() => Instantiate(tapEffectPrefab).SetUpTapEffectController(canvasRectTran, Vector3.zero));

        await UniTask.Delay(TimeSpan.FromSeconds(moveTime), cancellationToken: token);
    }

    public void ResetLookTranPos()
    {
        transform.DOMove(defaultLookTranPos, moveTime);
    }
}