using System;

[Serializable]
public class Choice
{
    public int index;
    public Message message;
    public string logprobs;
    public string finish_reason;
}