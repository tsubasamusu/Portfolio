using System;
using System.Collections.Generic;

[Serializable]
public class GptRequest
{
    public string model;

    public List<Message> messages;

    public int max_tokens;

    [Serializable]
    public class Message
    {
        public string role;

        public string content;
    }
}