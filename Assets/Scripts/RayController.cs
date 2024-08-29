using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayController : MonoBehaviour
{
    public List<string> currentGameWords = new();

    [SerializeField] GameObject Raycasters;

    public void GetRaysToLetters()
    {
        currentGameWords.Clear();
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
