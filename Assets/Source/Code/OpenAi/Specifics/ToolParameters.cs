using System;

[Serializable]
public class ToolParameters
{
    public string type = "object";
    public ToolProperties properties;
    public string[] required = { "row", "column" };
}