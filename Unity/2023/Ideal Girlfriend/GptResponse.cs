using System;

[Serializable]
public class GptResponse
{
    public string id;

    public Choice[] choices;

    public int created;

    public string model;

    public string system_fingerprint;

    public string @object;

    public Usage usage;

    [Serializable]
    public class Choice
    {
        public string finish_reason;

        public int index;

        public Message message;
    }

    [Serializable]
    public class Message
    {
        public string content;

        public ToolCall tool_calls;

        public string role;
    }

    [Serializable]
    public class ToolCall
    {
        public string id;

        public string type;

        public Function function;
    }

    [Serializable]
    public class Function
    {
        public string name;

        public string arguments;
    }

    [Serializable]
    public class Usage
    {
        public int completion_tokens;

        public int prompt_tokens;

        public int total_tokens;
    }
}