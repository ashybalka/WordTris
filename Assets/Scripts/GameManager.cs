using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool spawnLetter = true;

    [SerializeField] GameObject LetterPrefab, Letters;

    private LetterFactory letterFactory;
    private RayController rayController;

    public int level = 0;

    [SerializeField] TMP_Text levelText, scoreText, nextLetter;

    [SerializeField] GameObject WordPanelPrefab, WordPanelContext;

    [SerializeField] ScrollRect scrollRect;

    private char nextLetterChar;

    private int score = 0;

    private bool isCorutineRunning = false;
    private bool isFallingExist = false;

    void Start()
    {
        rayController = GameObject.Find("RayCasters").GetComponent<RayController>();
        letterFactory = GameObject.Find("LetterFactory").GetComponent<LetterFactory>();

        scoreText.text = score.ToString();
        levelText.text = level.ToString();

        nextLetterChar = letterFactory.GetRandomLetter();
        nextLetter.text = nextLetterChar.ToString();

        StartCoroutine(LineEraser());
    }

    private void Update()
    {
        level = (int)Math.Floor(Time.timeSinceLevelLoad / 30);
        levelText.text = level.ToString();


    }

    public void FalingRutine()
    {
        StartCoroutine(LineEraser());
    }


    public int CheckFalingFigures()
    {
        int falling = 0;
        foreach (var figure in Letters.GetComponentsInChildren<FigureController>())
            {
            if (figure.isFalling)
                { 
                    falling++;
                }
            }
        return falling;
    }

    IEnumerator LineEraser()
    {
        if (!isCorutineRunning)
        {
            isCorutineRunning = true;

            if (CheckFalingFigures() != 0)
            {
                yield return new WaitForSeconds(0.5f);
                isCorutineRunning = false;
                StartCoroutine(LineEraser());
            }
            else
            {
                rayController.GetRaysToLetters();
                if (FindDictionaryWords() != 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    isCorutineRunning = false;
                    StartCoroutine(LineEraser());
                }
                else
                {
                    isCorutineRunning = false;
                    LetterSpawner();
                }


            }
        }
    }


    public int FindDictionaryWords()
    {
        int count = 0;
        foreach (string wordsLine in rayController.currentGameWords)
        {
            if (letterFactory.WordInDictionary(wordsLine))
            {
                score += wordsLine.Length;
                scoreText.text = score.ToString();
                Debug.Log("Найдено слово " + wordsLine + " Длинной " + wordsLine.Length);
                // Tut zapolniaem panel
                Wordpanel(wordsLine);
                count++;

                List<int> positions = rayController.PositionFinder(wordsLine);

                for (int i = 0; i < positions.Count; i += 2)
                {
                    FigureController highlighFigure = Letters.GetComponentsInChildren<FigureController>()
                        .Where(n => n.transform.position.x == (positions[i] - 4) 
                        && n.transform.position.y == (4.5f - positions[i + 1])).First();
                    Destroy(highlighFigure.gameObject);
                }
            }
        }
        Debug.Log("Count: " + count);
        return count;
    }

    public void Wordpanel(string text)
    {
        GameObject wordPanelItem = Instantiate(WordPanelPrefab);
        wordPanelItem.GetComponentInChildren<TMP_Text>().text = text;
        wordPanelItem.transform.SetParent(WordPanelContext.transform, false);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    public void LetterSpawner()
    {
        GameObject newLetter = Instantiate(LetterPrefab);
        newLetter.transform.position = new Vector2(0f, 5.5f);
        newLetter.GetComponent<FigureController>().figureChar = nextLetterChar;
        newLetter.GetComponent<FigureController>().SetCharToTMP(nextLetterChar);
        newLetter.transform.SetParent(Letters.transform, false);
        nextLetterChar = letterFactory.GetRandomLetter();
        nextLetter.text = nextLetterChar.ToString();   
    }
}
