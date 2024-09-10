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
        AddLetters('�', 1097);
        AddLetters('�', 845);
        AddLetters('�', 801);
        AddLetters('�', 735);
        AddLetters('�', 670);
        AddLetters('�', 626);
        AddLetters('�', 547);
        AddLetters('�', 473);
        AddLetters('�', 454);
        AddLetters('�', 440);
        AddLetters('�', 349);
        AddLetters('�', 321);
        AddLetters('�', 298);
        AddLetters('�', 281);
        AddLetters('�', 262);
        AddLetters('�', 201);
        AddLetters('�', 190);
        AddLetters('�', 174);
        AddLetters('�', 170);
        AddLetters('�', 165);
        AddLetters('�', 159);
        AddLetters('�', 144);
        AddLetters('�', 121);
        AddLetters('�', 97);
        AddLetters('�', 94);
        AddLetters('�', 73);
        AddLetters('�', 64);
        AddLetters('�', 48);
        AddLetters('�', 32);
        AddLetters('�', 36);
        AddLetters('�', 26);
        AddLetters('�', 4);
        AddLetters('�', 4);
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
