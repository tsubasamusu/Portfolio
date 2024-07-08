using TMPro;
using UnityEngine;

namespace LightingDemonstration
{
    public class LogDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI tmpLog;

        [SerializeField]
        private CanvasGroup cgLogDisplayer;

        public static LogDisplayer Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);

                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            SetVisible(false);

            Application.logMessageReceived += OnReceivedLogMessage;
        }

        private void OnReceivedLogMessage(string logMessage, string _, LogType logType)
        {
            ShowLog(logMessage, logType);
        }

        public void ShowLog(string logMessage, LogType logType)
        {
            if (logType == LogType.Log || logType == LogType.Warning) return;

            if (!IsVisible()) SetVisible(true);

            logMessage = logMessage.Replace("\n", " ");

            AttachColorByLogType(logType, ref logMessage);

            tmpLog.text = tmpLog.text + (string.IsNullOrEmpty(tmpLog.text) ? string.Empty : "\n\n") + logMessage;
        }

        private void SetVisible(bool visible)
        {
            cgLogDisplayer.alpha = visible ? 1f : 0f;

            cgLogDisplayer.blocksRaycasts = cgLogDisplayer.interactable = visible;

            SetJoystickEnable(!visible);
        }

        private void AttachColorByLogType(LogType logType, ref string text)
        {
            const string white = "FFFFFF";
            const string yellow = "FFFF00";
            const string red = "FF0000";

            text = logType switch
            {
                LogType.Log => "<color=#" + white + ">" + text + "</color>",
                LogType.Warning => "<color=#" + yellow + ">" + text + "</color>",
                LogType.Assert => "<color=#" + yellow + ">" + text + "</color>",
                LogType.Error => "<color=#" + red + ">" + text + "</color>",
                LogType.Exception => "<color=#" + red + ">" + text + "</color>",
                _ => text
            };
        }

        private bool IsVisible() => cgLogDisplayer.interactable;

        private void SetJoystickEnable(bool enable)
        {
            Joystick joystick = FindAnyObjectByType(typeof(Joystick)) as Joystick;

            if (joystick != null) joystick.SetEnable(enable);
        }
    }
}