using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Roulette
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class ButtonController : MonoBehaviour, ISetup
    {
        [SerializeField]
        private bool setInteractableToFalseWhenClickedButton;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Button button;

        [SerializeField]
        private TextMeshProUGUI textMeshPro;

        [SerializeField]
        private CanvasGroup cgButton;

        [SerializeField]
        private UnityEvent OnClickedButton;

        public CanvasGroup CgButton { get => cgButton; }

        public string ButtonText
        {
            get => textMeshPro == null ? string.Empty : textMeshPro.text;

            set
            {
                if (textMeshPro != null) textMeshPro.text = value;
            }
        }

        public bool Interactable
        {
            get => button.interactable;

            set => button.interactable = value;
        }

        public Sprite ImageSprite
        {
            set => image.sprite = value;
        }

        public void Setup()
        {
            button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    ChangeButtonColorScheme();

                    button.interactable = !setInteractableToFalseWhenClickedButton;

                    OnClickedButton.Invoke();

                    OnLoadedClass();
                })
                .AddTo(this);
        }

        protected virtual void OnLoadedClass()
        {

        }

        public async UniTaskVoid RestoreButtonColorSchemeAsync(CancellationToken cancellationToken)
        {
            if (button.interactable) button.interactable = false;

            await UniTask.Delay(TimeSpan.FromSeconds(ConstData.TIME_BUTTON_ANIMATION), cancellationToken: cancellationToken);

            ChangeButtonColorScheme();

            button.interactable = true;
        }

        public void ChangeButtonColorScheme()
        {
            if (textMeshPro != null) (image.color, textMeshPro.color) = (textMeshPro.color, image.color);
        }
    }
}