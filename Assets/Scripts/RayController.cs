using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RayController : MonoBehaviour
{
    public List<string> currentGameWords = new();
    public char[,] gameField = new char[9,9];

    [SerializeField] GameObject Raycasters, Letters;

    public void GetRaysToLetters()
    {
        currentGameWords.Clear();

        {
            for (int i = 0; i < gameField.GetLength(0); i++)
            {
                for (int j = 0; j < gameField.GetLength(1); j++)
                {
                    gameField[i, j] = ' ';
                }
            }
        }

        foreach (var letterFigure in Letters.GetComponentsInChildren<BoxCollider2D>())
        {
            gameField[(int)(letterFigure.transform.position.x + 4f), (int)(4.5f - letterFigure.transform.position.y)] = 
                letterFigure.GetComponent<FigureController>().figureChar;
        }

        foreach (char ch in gameField)
        { 
            Debug.Log($"{ch}");
        }

        // Get Rays To Each Line
        foreach (Transform rays in Raycasters.GetComponentsInChildren<Transform>())
        {
            string tempStr = RayToLetters(rays.gameObject);
            if (!string.IsNullOrEmpty(tempStr))
            {
                currentGameWords.Add(tempStr);
                currentGameWords.Add(Reverse(tempStr));
                //List<string> TextListWords = GetSubsequences(wordsLine);
            }
            
        }
    }
    public string RayToLetters(GameObject ray)
    {
        string line = string.Empty;
        Vector2 direction = ray.transform.position.y < -5 ? Vector2.up : Vector2.right;
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.transform.position, direction, 10f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Figure"))
            {
                line += hit.collider.GetComponent<FigureController>().figureChar;
            }    
        }
        return line;
    }

    public static string Reverse(string s)
    {
        return new string(s.Reverse().ToArray());
    }
}
