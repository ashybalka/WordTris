using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RayController : MonoBehaviour
{
    public List<string> currentGameWords = new();
    public char[,] gameField = new char[9,9];

    [SerializeField] GameObject Letters;

    public void GetRaysToLetters()
    {
        currentGameWords.Clear();

        //Initialize empty char array
        for (int i = 0; i < gameField.GetLength(0); i++)
        {
            for (int j = 0; j < gameField.GetLength(1); j++)
            {
                gameField[i, j] = ' ';
            }
        }

        // Find All Letters
        foreach (var letterFigure in Letters.GetComponentsInChildren<BoxCollider2D>().Where(n => n.transform.position.y< 3))
        {
            var pos = letterFigure.transform.position;
            gameField[(int)(pos.x + 4f), (int)(4.5f - pos.y)] = letterFigure.GetComponent<FigureController>().figureChar;
        }

        // Find All Substrings
        currentGameWords = FindSubstrings(gameField);
    }

    public static string Reverse(string s)
    {
        return new string(s.Reverse().ToArray());
    }


    public static List<string> FindSubstrings(char[,] array)
    {
        HashSet<string> substrings = new HashSet<string>();

        // Слева направо и справа налево
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (char.IsLetter(array[i, j]))
                {
                    // Слева направо
                    string substring = "";
                    int k = j;
                    while (k < array.GetLength(1) && char.IsLetter(array[i, k]))
                    {
                        substring += array[i, k];
                        k++;
                    }
                    if (substring.Length > 1)
                    {
                        substrings.Add(substring);
                        substrings.Add(new string(Reverse(substring)));
                    }

                    // Справа налево
                    substring = "";
                    k = j;
                    while (k >= 0 && char.IsLetter(array[i, k]))
                    {
                        substring += array[i, k];
                        k--;
                    }
                    if (substring.Length > 1)
                    {
                        substrings.Add(substring);
                        substrings.Add(new string(Reverse(substring)));
                    }
                }
            }
        }

        // Сверху вниз и снизу вверх
        for (int j = 0; j < array.GetLength(1); j++)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                if (char.IsLetter(array[i, j]))
                {
                    // Сверху вниз
                    string substring = "";
                    int k = i;
                    while (k < array.GetLength(0) && char.IsLetter(array[k, j]))
                    {
                        substring += array[k, j];
                        k++;
                    }
                    if (substring.Length > 1)
                    {
                        substrings.Add(substring);
                        substrings.Add(new string(Reverse(substring)));
                    }

                    // Снизу вверх
                    substring = "";
                    k = i;
                    while (k >= 0 && char.IsLetter(array[k, j]))
                    {
                        substring += array[k, j];
                        k--;
                    }
                    if (substring.Length > 1)
                    {
                        substrings.Add(substring);
                        substrings.Add(new string(Reverse(substring)));
                    }

                }
            }
        }

        return substrings.ToList();
    }

    public List<int> PositionFinder(string word)
    {
        return FindSubstringPositions(gameField, word);
    }


    public static List<int> FindSubstringPositions(char[,] array, string substring)
    {
        List<int> positions = new List<int>();

        // Поиск слева направо
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j <= array.GetLength(1) - substring.Length; j++)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i, j + k] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i);
                        positions.Add(j + k);
                    }
                }
            }

        }

        // Поиск сверху вниз
        for (int j = 0; j < array.GetLength(1); j++)
        {
            for (int i = 0; i <= array.GetLength(0) - substring.Length; i++)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i + k, j] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i + k);
                        positions.Add(j);
                    }
                }
            }
        }

        // Поиск справа налево
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = array.GetLength(1) - 1; j >= substring.Length - 1; j--)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i, j - k] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i);
                        positions.Add(j - k);
                    }
                }
            }
        }

        // Поиск снизу вверх
        for (int j = 0; j < array.GetLength(1); j++)
        {
            for (int i = array.GetLength(0) - 1; i >= substring.Length - 1; i--)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i - k, j] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i - k);
                        positions.Add(j);
                    }
                }
            }
        }

        return positions;
    }
}
