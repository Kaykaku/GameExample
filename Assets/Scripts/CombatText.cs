using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    
    public void OnInit(float hp)
    {
        OnInit(hp.ToString(), new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
    }

    public void OnInit(string text,Color color)
    {
        hpText.text = text;
        hpText.color = color;
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
