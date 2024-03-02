using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField]
    private TextAsset gptCharacterTextFile;

    private string apiKey;

    [HideInInspector]
    public List<GptRequest.Message> messages = new();

    public static GameData instance;

    public string ApiKey
    {
        get
        {
            if (string.IsNullOrEmpty(apiKey)) apiKey = PlayerPrefs.GetString(ConstData.SAVE_DATA_NAME_OF_API_KEY);

            return apiKey;
        }
        set
        {
            apiKey = value;

            PlayerPrefs.SetString(ConstData.SAVE_DATA_NAME_OF_API_KEY, value);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetGptCharacter();
    }

    private void SetGptCharacter()
    {
        GptRequest.Message firstMessage = new()
        {
            role = RoleType.assistant.ToString(),
            content = gptCharacterTextFile.text,
        };

        messages.Add(firstMessage);
    }

    public void ResetSaveData() => PlayerPrefs.DeleteAll();
}