using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imageFill;

    float hp;
    float maxHp;

    public void OnInit(float maxHp)
    {
        this.maxHp = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }
    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount,hp/maxHp,Time.deltaTime * 5f);
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }
}
