using System;

[Serializable]
public class ToolProperties
{
    public ToolParam row = new() { type = "integer", description = "Рядок (0-3)" };
    public ToolParam column = new() { type = "integer", description = "Стовпець (0-3)" };
}