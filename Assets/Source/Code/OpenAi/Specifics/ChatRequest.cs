using System;

[Serializable]
public class ChatRequest
{
    public string model;
    public Message[] messages;
    public Tool[] tools;
    public string tool_choice;
    
    public ChatRequest(string model, string tool_choice)
    {
        this.model = model;
        this.tool_choice = tool_choice;
    }
}