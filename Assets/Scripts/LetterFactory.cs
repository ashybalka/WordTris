using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterFactory : MonoBehaviour
{

    private List<char> weightedAlphabet = new();
    public List<string> dictionary = new();

    [SerializeField] TextAsset Dictionary_Main;

    void Awake()
    {
        RandomLetter();
        CreateDictionary();
    }

    public void RandomLetter()
    {
        AddLetters('Î', 1097);
        AddLetters('Å', 845);
        AddLetters('À', 801);
        AddLetters('È', 735);
        AddLetters('Í', 670);
        AddLetters('Ò', 626);
        AddLetters('Ñ', 547);
        AddLetters('Ð', 473);
        AddLetters('Â', 454);
        AddLetters('Ë', 440);
        AddLetters('Ê', 349);
        AddLetters('Ì', 321);
        AddLetters('Ä', 298);
        AddLetters('Ï', 281);
        AddLetters('Ó', 262);
        AddLetters('ß', 201);
        AddLetters('Û', 190);
        AddLetters('Ü', 174);
        AddLetters('Ã', 170);
        AddLetters('Ç', 165);
        AddLetters('Á', 159);
        AddLetters('×', 144);
        AddLetters('É', 121);
        AddLetters('Õ', 97);
        AddLetters('Æ', 94);
        AddLetters('Ø', 73);
        AddLetters('Þ', 64);
        AddLetters('Ö', 48);
        AddLetters('Ý', 32);
        AddLetters('Ù', 36);
        AddLetters('Ô', 26);
        AddLetters('¨', 4);
        AddLetters('Ú', 4);
    }

    private void AddLetters(char letter, int frequency)
    {
        for (int i = 0; i < frequency; i++)
        {
            weightedAlphabet.Add(letter);
        }
    }

    public char GetRandomLetter()
    {
        int index = Random.Range(0, weightedAlphabet.Count);
        return weightedAlphabet[index];
    }

    public void CreateDictionary()
    {
        dictionary = new List<string>(Dictionary_Main.text.Split(new[] { ',', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries));

        for (int i = 0; i < dictionary.Count; i++)
        {
            dictionary[i] = dictionary[i].Trim(new[] { '{', '}', '\"', ' ' });
        }
    }

    public bool WordInDictionary(string word)
    {
        return dictionary.Contains(word.ToLower());
    }
}
