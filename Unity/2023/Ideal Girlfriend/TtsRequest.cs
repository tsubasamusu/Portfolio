using System;

[Serializable]
public class TtsRequest
{
    public string model;

    public string input;

    public string voice;

    public string response_format;

    public float speed = 1f;
}