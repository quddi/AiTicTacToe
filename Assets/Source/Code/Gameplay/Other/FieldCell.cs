public class FieldCell
{
    public string TeamId { get; set; }
    
    public int X { get; set; }
    public int Y { get; set; }
    
    public FieldCell(int x, int y) => (X, Y) = (x, y);
}