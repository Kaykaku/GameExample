using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI flyText;

    //Initialize fly text with input parameter and random color
    public void OnInit(string text)
    {
        OnInit(text, new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
    }

    //Initialize fly text with input parameter and color
    //Destroy itself ater 1 second
    public void OnInit(string text,Color color)
    {
        flyText.text = text;
        flyText.color = color;
        Invoke(nameof(OnDespawn), 1f);
    }

    //Destroy itself
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
