using System;

[Serializable]
public class CompletionTokensDetails
{
    public int reasoning_tokens;
    public int audio_tokens;
    public int accepted_prediction_tokens;
    public int rejected_prediction_tokens;
}