using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Image imageFill;

    float point;
    float maxPoint;

    //Initialize max point and current point
    // Fill to bar
    public void OnInit(float maxPoint)
    {
        this.maxPoint = maxPoint;
        point = maxPoint;
        imageFill.fillAmount = 1;
    }
    // Update is called once per frame
    //The image fill rate will slowly decrease to the ratio between point and maxPoin
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount,point/maxPoint,Time.deltaTime * 5f);
    }

    //Set new value of Point
    public void SetNewPoint(float point)
    {
        this.point = point;
    }
}
