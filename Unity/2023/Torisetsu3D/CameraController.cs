using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform lookTran;

    private float distance;

    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float minDistance;

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private float xSpeed;

    [SerializeField]
    private float ySpeed;

    [SerializeField]
    private float minAngleY;

    [SerializeField]
    private float maxAngleY;

    private float currentAngleX;

    private float currentAngleY;

    private float defaultDistance;

    private Vector3 defaultCameraPos;

    private Vector3 defaultAngles;

    private void Start()
    {
        distance = defaultDistance = (transform.position - lookTran.position).magnitude;

        defaultCameraPos = transform.position;

        defaultAngles = transform.eulerAngles;

        currentAngleX = transform.eulerAngles.y;

        currentAngleY = transform.eulerAngles.x;

        this.UpdateAsObservable()
            .Subscribe(_ => UpdateDistance())
            .AddTo(this);

        this.LateUpdateAsObservable()
            .Subscribe(_ => UpdateAngleAndPos())
            .AddTo(this);
    }

    private void UpdateDistance()
    {
        if (TouchingUI()) return;

        if (!Input.touchSupported || Input.touchCount != 2) return;

        Touch touch1 = Input.GetTouch(0);

        Touch touch2 = Input.GetTouch(1);

        if (touch1.phase != TouchPhase.Moved || touch2.phase != TouchPhase.Moved) return;

        Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

        Vector2 prevTouch2 = touch2.position - touch2.deltaPosition;

        float prevTouchDeltaMag = (prevTouch1 - prevTouch2).magnitude;

        float touchDeltaMag = (touch1.position - touch2.position).magnitude;

        float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

        distance = Mathf.Clamp(distance + (deltaMagnitudeDiff * zoomSpeed * Time.deltaTime), minDistance, maxDistance);
    }

    private void UpdateAngleAndPos()
    {
        transform.position = transform.rotation * new Vector3(0.0f, 0.0f, -distance) + lookTran.position;

        if (TouchingUI()) return;

        if (!Input.touchSupported || Input.touchCount != 1) return;

        Touch touch = Input.GetTouch(0);

        float moveValueX = touch.phase == TouchPhase.Moved ? -touch.deltaPosition.x : 0f;

        float moveValueY = touch.phase == TouchPhase.Moved ? -touch.deltaPosition.y : 0f;

        currentAngleX += moveValueX * xSpeed * Time.deltaTime;

        currentAngleY -= moveValueY * ySpeed * Time.deltaTime;

        currentAngleY = ClampAngle(currentAngleY, minAngleY, maxAngleY);

        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(currentAngleY, currentAngleX, 0);

        transform.rotation = rotation;

        transform.position = (rotation * new Vector3(0.0f, 0.0f, -distance)) + lookTran.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;

        if (angle > 360f) angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }

    public bool TouchingUI()
    {
        Touch[] touches = Input.touches;

        for (int i = 0; i < touches.Length; i++)
        {
            PointerEventData pointData = new(EventSystem.current)
            {
                position = touches[i].position
            };

            List<RaycastResult> rayResults = new();

            EventSystem.current.RaycastAll(pointData, rayResults);

            for (int j = 0; j < rayResults.Count; j++)
            {
                if (rayResults[j].gameObject.CompareTag("UI")) return true;
            }
        }

        return false;
    }

    public void ResetCameraPos()
    {
        distance = defaultDistance;

        transform.position = defaultCameraPos;

        currentAngleX = defaultAngles.y;

        currentAngleY = defaultAngles.x;

        transform.eulerAngles = defaultAngles;
    }
}