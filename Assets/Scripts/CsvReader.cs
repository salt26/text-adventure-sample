using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Text;
using UnityEngine;

public static class CsvReader
{
    public static List<DialogEntity> ToDialogs(this string csvText, char delimiter = '\t')
    {
        bool isFirstLine = true;
        bool isSecondLine = false;
        Dictionary<int, FieldInfo> fields = new Dictionary<int, FieldInfo>();
        Dictionary<int, Func<string, object>> typeConverter = new();
        List<DialogEntity> dialogEntities = new List<DialogEntity>();
        
        foreach (var line in csvText.Split('\n'))
        {
            string l = line.Trim(); // 줄바꿈에 있을 수 있는 '\r' 제거
            string[] tokens;
            if (isFirstLine)
            {
                // 첫 번째 줄은 헤더로 사용
                tokens = l.Split(delimiter);
                for (var i = 0; i < tokens.Length; i++)
                {
                    FieldInfo fieldInfo = typeof(DialogEntity).GetField(tokens[i].Trim(), BindingFlags.Instance | BindingFlags.Public);
                    if (fieldInfo != null)
                    {
                        fields.Add(i, fieldInfo);
                    }
                }
                isFirstLine = false;
                isSecondLine = true;
                continue;
            }

            if (isSecondLine)
            {
                // 두 번째 줄은 타입으로 사용
                tokens = l.Split(delimiter);
                for (var i = 0; i < tokens.Length; i++)
                {
                    switch (tokens[i].Trim())
                    {
                        case "int":
                            typeConverter.Add(i, str =>
                            {
                                if (string.IsNullOrEmpty(str)) return -1;
                                return int.Parse(str);
                            });
                            break;
                        case "float":
                            typeConverter.Add(i, str => float.Parse(str));
                            break;
                        case "bool":
                            typeConverter.Add(i, str => bool.Parse(str));
                            break;
                        case "string":
                            typeConverter.Add(i, str => str);
                            break;
                        case "List_Color":
                            typeConverter.Add(i, str =>
                            {
                                var colorHexes = str.TrimStart('[').TrimEnd(']').Split(' ');
                                List<Color> colors = new List<Color>();
                                foreach (var colorHex in colorHexes)
                                {
                                    string hex = colorHex;
                                    
                                    // #을 맨 앞에 붙여도 되고 안 붙여도 됨
                                    if (colorHex.StartsWith("#")) hex = colorHex[1..];
                                    
                                    switch (hex.Length)
                                    {
                                        case 6:
                                            colors.Add(new Color(
                                                Convert.ToInt32(hex[0..2], 16) / 255f,
                                                Convert.ToInt32(hex[2..4], 16) / 255f,
                                                Convert.ToInt32(hex[4..6], 16) / 255f)
                                            );
                                            break;
                                        case 8:
                                            colors.Add(new Color(
                                                Convert.ToInt32(hex[0..2], 16) / 255f,
                                                Convert.ToInt32(hex[2..4], 16) / 255f,
                                                Convert.ToInt32(hex[4..6], 16) / 255f,
                                                Convert.ToInt32(hex[6..8], 16) / 255f
                                            ));
                                            break;
                                        default:
                                            Debug.LogWarning(ZString.Concat("CSV Warning: Invalid color hex format (", colorHex, ")"));
                                            colors.Add(Color.white);
                                            break;
                                    }
                                }
                                return colors;
                            });
                            break;
                        case "Enum_Emotion":
                            typeConverter.Add(i, str =>
                            {
                                var keys = Enum.GetNames(typeof(Character.Emotion));
                                if (keys.Contains(str) && Enum.TryParse(str, out Character.Emotion emotion))
                                {
                                    return emotion;
                                }
                                else
                                {
                                    return Character.Emotion.Default;
                                }
                            });
                            break;
                        default:
                            Debug.LogError(ZString.Concat("CSV Error: 알 수 없는 ", i, "번째 타입 ", tokens[i]));
                            typeConverter.Add(i, str => str);
                            break;
                    }
                }
                isSecondLine = false;
                continue;
            }
            
            // 세 번째 줄부터 진짜 데이터
            tokens = l.Split(delimiter);
            DialogEntity dialogEntity = ScriptableObject.CreateInstance<DialogEntity>();
            for (int i = 0; i < tokens.Length; i++)
            {
                if (fields.Count <= i)
                {
                    Debug.LogError(ZString.Concat("CSV Error: 헤더 길이를 초과하였습니다.\n", line));
                    continue; // Error
                }

                try
                {
                    /*
                    switch (header[i])
                    {
                        case "Id":
                            dialogEntity.Id = int.Parse(tokens[i]);
                            break;
                        case "BackgroundId":
                            dialogEntity.BackgroundId = int.Parse(tokens[i]);
                            break;
                        case "CharacterId":
                            dialogEntity.CharacterId = int.Parse(tokens[i]);
                            break;
                    }
                    */
                    fields[i].SetValue(dialogEntity, Convert.ChangeType(typeConverter[i](tokens[i].Trim()), fields[i].FieldType));
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            dialogEntities.Add(dialogEntity);
        }
        return dialogEntities;
    }
}
