using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguagesSO", menuName = "Scriptable Objects/LanguagesSO")]
public class LanguagesSO : ScriptableObject
{
    public List<string> RussianLines = new List<string> { };
    public List<string> EnglishLines = new List<string> { };
    public List<string> ChinaLines = new List<string> { };
    public List<string> GermanyLines = new List<string> { };


}
