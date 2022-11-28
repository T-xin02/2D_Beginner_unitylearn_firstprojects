using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    //创建一个共有静态属性，获取当前血量本身，也就是一个静态实例（get；private set；----取值器与赋值器）
    public static UIHealthBar Instance{get;private set;}
    // Start is called before the first frame update
    //创建ui图新对象
    public Image mask;
    //设置一个变量，记录遮罩层初始长度
    float orginalSize;
    private void Awake()
    {
         //设置静态实例为当前类对象
        Instance=this;
    }
    void Start()
    {
        //获取遮罩层图像的初始宽度
        orginalSize = mask.rectTransform.rect.width;
    }

    //创建一个方法，用来设置当前mask遮罩层的宽度
    public void SetValue(float value)
    {
        //设置更改的是mask遮罩层的宽度，依据是传递过来的参数进行更改
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,orginalSize*value);
    }
}
