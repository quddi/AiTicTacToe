using System;

[Serializable]
public class Message
{
    public string role;
    public string content;
    public ToolCall[] tool_calls;
    public string refusal;

    public Message(string role, string content)
    {
        this.role = role;
        this.content = content;
    }
}