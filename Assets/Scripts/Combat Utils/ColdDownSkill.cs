using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColdDownSkill : MonoBehaviour
{
    [SerializeField] Image imageFill;
    [SerializeField] TextMeshProUGUI timeText;

    float timer;
    float coldownTime;

    private void Start()
    {
        imageFill.fillAmount = 0;
    }

    public void OnInit(float coldownTime)
    {
        this.coldownTime = coldownTime;
        imageFill.fillAmount = 1;
        timeText.text = coldownTime.ToString();
        timer = 0;
    }

    private void Update()
    {
        if (timer >= coldownTime)
        {
            imageFill.fillAmount = 0;
            timeText.text = "";
            return;
        }
        imageFill.fillAmount = Mathf.Lerp(1, 0, timer/coldownTime);
        //imageFill.fillAmount = coldownTime - timer;
        timeText.text = (Mathf.Round((coldownTime - timer) * 100f) / 100f).ToString();
        timer += Time.deltaTime;
    }
}
