using System;

[Serializable]
public class ChatResponse
{
    public string id;
    public string @object;
    public int created;
    public string model;
    public Choice[] choices;
    public Usage usage;
    public string service_tier;
    public string system_fingerprint;
}