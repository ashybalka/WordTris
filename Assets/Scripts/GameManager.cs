using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public bool spawnLetter = true;

    [SerializeField] GameObject LetterPrefab, Letters;
    
    private LetterFactory letterFactory;
    private RayController rayController;

    public int level = 0;

    [SerializeField] TMP_Text levelText, scoreText , nextLetter;

    private char nextLetterChar;

    private int score = 0;
    void Start()
    {
        rayController = GameObject.Find("RayCasters").GetComponent<RayController>();
        letterFactory = GameObject.Find("LetterFactory").GetComponent<LetterFactory>();

        scoreText.text = score.ToString();
        levelText.text = level.ToString();

        nextLetterChar = letterFactory.GetRandomLetter();
        nextLetter.text = nextLetterChar.ToString();

        //
        LetterSpawner();
    }

    private void Update()
    {
        level = (int)Math.Floor(Time.time / 30);
        levelText.text = level.ToString();
    }

    public void FindDictionaryWords()
    {
        foreach (string wordsLine in rayController.currentGameWords)
        {
            if (letterFactory.WordInDictionary(wordsLine))
            {
                score += wordsLine.Length;
                scoreText.text = score.ToString();
                Debug.Log("Найдено слово " + wordsLine + " Длинной " + wordsLine.Length);
                List<int> positions = rayController.PositionFinder(wordsLine);

                for (int i = 0; i < positions.Count; i += 2)
                {
                    FigureController highlighFigure = Letters.GetComponentsInChildren<FigureController>()
                        .Where(n => n.transform.position.x == (positions[i] - 4) 
                        && n.transform.position.y == (4.5f - positions[i + 1])).First();
                    highlighFigure.HiglightFigure();
                    Destroy(highlighFigure.gameObject);
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
            newLetter.GetComponent<FigureController>().figureChar = nextLetterChar;
            newLetter.GetComponent<FigureController>().SetCharToTMP(nextLetterChar);
            newLetter.transform.SetParent(Letters.transform, false);
            nextLetterChar = letterFactory.GetRandomLetter();
            nextLetter.text = nextLetterChar.ToString();
        }
    }
}
