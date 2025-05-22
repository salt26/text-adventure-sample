using System;
using Cysharp.Text;

[Serializable]
public struct ChoiceData
{
    public string NameKey;
    public int NextDialogId;

    public ChoiceData(string nameKey, int nextDialogId)
    {
        this.NameKey = nameKey;
        this.NextDialogId = nextDialogId;
    }

    public override string ToString()
    {
        return ZString.Concat("ChoiceData (NameKey: ", NameKey, ",  NextDialogId: ", NextDialogId, ")");
    }
}