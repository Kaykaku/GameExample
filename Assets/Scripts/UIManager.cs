using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] ColdDownSkill skill1;

    // Function to display Coin number on UI
    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    // Function to display CoolDown Skill 1 on UI
    public void SetColdDown1(float coldDownTime)
    {
        skill1.OnInit(coldDownTime);
    }
}
