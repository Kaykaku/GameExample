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
    float coodownTime;

    private void Start()
    {
        imageFill.fillAmount = 0;
    }

    //When initializing will
    //Set cooldown countdown time
    //Set full image
    //Reset timer
    public void OnInit(float coodownTime)
    {
        this.coodownTime = coodownTime;
        imageFill.fillAmount = 1;
        timeText.text = coodownTime.ToString();
        timer = 0;
    }

    private void Update()
    {
        //Timer is greater than cooldown
        // Image fill to 0
        // Hide cooldown text
        if (timer >= coodownTime)
        {
            imageFill.fillAmount = 0;
            timeText.text = "";
            return;
        }
        //Timer is less than cooldown
        // set the image fill ratio as a percentage between timer and cooldown time
        // show remaining cooldown
        imageFill.fillAmount = Mathf.Lerp(1, 0, timer/ coodownTime);
        timeText.text = (coodownTime - timer).ToString("F2");
        timer += Time.deltaTime;
    }
}
