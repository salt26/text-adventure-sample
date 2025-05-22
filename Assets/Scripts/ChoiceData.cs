using System;
using Cysharp.Text;

[Serializable]
public struct ChoiceData
{
    /// 선택지의 버튼에 표시될 텍스트 
    public string NameKey;
    
    /// 선택한 후에 넘어갈 다음 대사 번호
    public int NextDialogId;

    /// <summary>
    /// 선택지를 생성합니다. (생성자)
    /// </summary>
    public ChoiceData(string nameKey, int nextDialogId)
    {
        NameKey = nameKey;
        NextDialogId = nextDialogId;
    }

    /// <summary>
    /// 선택지의 정보를 문자열로 표시합니다.
    /// </summary>
    public override string ToString()
    {
        return ZString.Concat("ChoiceData (NameKey: ", NameKey, ",  NextDialogId: ", NextDialogId, ")");
    }
}