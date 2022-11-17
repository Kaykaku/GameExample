using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imageFill;

    float point;
    float maxPoint;

    public void OnInit(float maxPoint)
    {
        this.maxPoint = maxPoint;
        point = maxPoint;
        imageFill.fillAmount = 1;
    }
    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount,point/maxPoint,Time.deltaTime * 5f);
    }

    public void SetNewHp(float point)
    {
        this.point = point;
    }
}
