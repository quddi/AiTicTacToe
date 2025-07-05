public class FieldCell
{
    public string TeamId { get; set; }
    
    public int X { get; set; }
    public int Y { get; set; }
    
    public bool IsWinning { get; set; }
    
    public bool IsEmpty => string.IsNullOrEmpty(TeamId);
    
    public FieldCell(int x, int y) => (X, Y) = (x, y);
}