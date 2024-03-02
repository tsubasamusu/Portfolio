using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExceptionDataSO", menuName = "Create ExceptionDataSO")]
public class ExceptionDataSO : ScriptableObject
{
    [SerializeField]
    private List<ExceptionData> gptExceptions = new();

    [SerializeField]
    private List<ExceptionData> ttsExceptions = new();

    public List<ExceptionData> GptExceptions { get => gptExceptions; }

    public List<ExceptionData> TtsExceptions { get => ttsExceptions; }

    [Serializable]
    public class ExceptionData
    {
        public string exceptionKeyWord;

        public string displayErrorMessage;
    }

    public string GetErrorMessageByEcceptionText(string gptExceptionText)
    {
        foreach (ExceptionData gptExceptionData in gptExceptions)
        {
            if (gptExceptionText.Contains(gptExceptionData.exceptionKeyWord)) return gptExceptionData.displayErrorMessage;
        }

        return ConstData.UNKNOWN_EXCEPTION_MESSAGE;
    }

    public string TtsErrorMessageByEcceptionText(string ttsExceptionText)
    {
        foreach (ExceptionData ttsExceptionData in ttsExceptions)
        {
            if (ttsExceptionText.Contains(ttsExceptionData.exceptionKeyWord)) return ttsExceptionData.displayErrorMessage;
        }

        return ConstData.UNKNOWN_EXCEPTION_MESSAGE;
    }
}