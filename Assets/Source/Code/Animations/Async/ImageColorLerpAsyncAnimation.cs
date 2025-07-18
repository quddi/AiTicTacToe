using UnityEngine;
using UnityEngine.UI;

public class ImageColorLerpAsyncAnimation : TimedAsyncAnimation
{
    [field: SerializeField] public Color StartColor { get; set; }
    
    [field: SerializeField] public Color EndColor { get; set; }

    [field: SerializeReference] public EasingsVector3 Easings { get; set; }

    [field: SerializeField] public Image Image { get; set; }
    
    protected override void Evaluate(float t)
    {
        Image.color = new Color
        (
            Mathf.Lerp(StartColor.r, EndColor.r, Easings.Evaluate(t).x),
            Mathf.Lerp(StartColor.g, EndColor.g, Easings.Evaluate(t).y),
            Mathf.Lerp(StartColor.b, EndColor.b, Easings.Evaluate(t).z)
        );
    }
}