using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    public bool spawnLetter = true;
    [SerializeField] GameObject LetterPrefab, Letters;
    [SerializeField] TextAsset Dictionary_Main;

    private char[] ruAlphabet = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й',
        'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
        'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

    public List<string> dictionary = new();

    private RayController rayController;


    void Start()
    {
        CreateDictionary();
        LetterSpawner();
        rayController = GameObject.Find("RayCasters").GetComponent<RayController>();
    }


    void Update()
    {
        
    }

    public void CreateDictionary()
    {
        dictionary = new List<string>(Dictionary_Main.text.Split(new[] { ',', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries));

        for (int i = 0; i < dictionary.Count; i++)
        {
            dictionary[i] = dictionary[i].Trim(new[] { '{', '}', '\"', ' ' });
        }
    }


    public void FindDictionaryWords()
    {
        foreach (string wordsLine in rayController.currentGameWords)
        {
            List<string> TextListWords =  GetSubsequences(wordsLine);
            foreach (string word in TextListWords)
            {
                if (dictionary.Contains(wordsLine))
                {
                    Debug.Log("True");
                    Debug.Log("Найдено слово " + wordsLine);
                }
            }
        }
    }


    static List<string> GetSubsequences(string input)
    {
        var result = new List<string>();

        for (int start = 0; start < input.Length; start++)
        {
            for (int end = start + 1; end <= input.Length; end++)
            {
                result.Add(input.Substring(start, end - start));
            }
        }
        return result;
    }

    public void LetterSpawner()
    {
        if (spawnLetter)
        { 
            GameObject newLetter = Instantiate(LetterPrefab);
            newLetter.transform.position = new Vector2(0f, 6f);
            char randomLetter = ruAlphabet[UnityEngine.Random.Range(0, ruAlphabet.Length)];
            newLetter.GetComponent<FigureController>().figureChar = randomLetter;
            newLetter.GetComponent<FigureController>().SetCharToTMP(randomLetter);
            newLetter.transform.SetParent(Letters.transform, false);
        }
    }
}
