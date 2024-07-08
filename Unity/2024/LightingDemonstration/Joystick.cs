using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LightingDemonstration
{
    [RequireComponent(typeof(RectTransform))]
    public class Joystick : MonoBehaviour, ISetup
    {
        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private CanvasGroup cgJoystick;

        private RectTransform rectTransform;

        private Vector2 defaultJoystickPosition;

        private bool isEnable;

        public void Setup()
        {
            if (!Input.touchSupported) return;

            if (!TryGetComponent(out rectTransform))
            {
                Debug.LogWarning("Failed to get the RectTransform.");

                return;
            }

            defaultJoystickPosition = rectTransform.position;

            isEnable = true;

            this.UpdateAsObservable()
                .Subscribe(_ => OnUpdated())
                .AddTo(this);
        }

        private void OnUpdated()
        {
            if (rectTransform == null) return;

            if (!isEnable) return;

            if (Input.touches.Length == 0)
            {
                if (!IsAtDefaultPosition()) OnReleasedJoystick();

                return;
            }

            Touch touch = Input.touches[0];

            if (IsTouchingJoystickRange(touch))
            {
                if (IsTouchingAnyButton(touch))
                {
                    OnReleasedJoystick();

                    return;
                }

                OnTouchedJoystickRange(touch);

                return;
            }

            if (!IsAtDefaultPosition()) OnReleasedJoystick();
        }

        private bool IsTouchingAnyButton(Touch touch)
        {
            PointerEventData pointerEventData = new(EventSystem.current);

            pointerEventData.position = touch.position;

            List<RaycastResult> raycastResults = new();

            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.TryGetComponent(out ButtonBase _)) return true;
            }

            return false;
        }

        private bool IsTouchingJoystickRange(Touch touch)
        {
            if (rectTransform == null) return false;

            PointerEventData pointerEventData = new(EventSystem.current);

            pointerEventData.position = touch.position;

            List<RaycastResult> RaycastResults = new();

            EventSystem.current.RaycastAll(pointerEventData, RaycastResults);

            for (int i = 0; i < RaycastResults.Count; i++)
            {
                if (GetRelativeTouchPosition(touch).magnitude <= ConstDataSO.Instance.joystickEnableRadius) return true;
            }

            return false;
        }

        private void OnTouchedJoystickRange(Touch touch)
        {
            if (rectTransform == null) return;

            if (!isEnable) return;

            MakeJoystickFollowTouch(touch);

            if (playerController == null) return;

            playerController.OnInputJoystick(GetInputValue(touch));
        }

        private Vector2 GetInputValue(Touch touch)
        {
            if (rectTransform == null || defaultJoystickPosition == null) return Vector2.zero;

            Vector2 relativeJoystickPosition = new(rectTransform.position.x - defaultJoystickPosition.x, rectTransform.position.y - defaultJoystickPosition.y);

            float ratio = Math.Clamp(relativeJoystickPosition.magnitude / ConstDataSO.Instance.joystickMoveableRadius, 0f, 1f);

            return relativeJoystickPosition.normalized * ratio;
        }

        private void MakeJoystickFollowTouch(Touch touch)
        {
            if (GetRelativeTouchPosition(touch).magnitude <= ConstDataSO.Instance.joystickMoveableRadius)
            {
                rectTransform.position = touch.position;

                return;
            }

            Vector2 relativeIdealDirection = GetRelativeTouchPosition(touch).normalized * ConstDataSO.Instance.joystickMoveableRadius;

            rectTransform.position = new(defaultJoystickPosition.x + relativeIdealDirection.x, defaultJoystickPosition.y + relativeIdealDirection.y);
        }

        private void OnReleasedJoystick()
        {
            if (!isEnable) return;

            isEnable = false;

            rectTransform.DOMove(defaultJoystickPosition, ConstDataSO.Instance.joystickAnimationTime)
                .OnComplete(() => isEnable = true);
        }

        private Vector2 GetRelativeTouchPosition(Touch touch)
        {
            if (defaultJoystickPosition == null) return Vector2.zero;

            return new(touch.position.x - defaultJoystickPosition.x, touch.position.y - defaultJoystickPosition.y);
        }

        private bool IsAtDefaultPosition()
        {
            if (rectTransform == null) return true;

            return rectTransform.position.x == defaultJoystickPosition.x && rectTransform.position.y == defaultJoystickPosition.y;
        }

        public void SetEnable(bool enable, bool changeVisibility = false)
        {
            if (changeVisibility) cgJoystick.alpha = enable ? 1f : 0f;

            isEnable = enable;
        }
    }
}