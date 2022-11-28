using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCCharacter : MonoBehaviour
{
    //持续时间
    public float displayTime = 4.0f;
    //获取对话框
    public GameObject dialogBox;
    //计时器
    float displaytimer;
    //创建一个游戏对象，用来获取tmp组件
    public GameObject digTexProGameObject;
    //创建游戏组件类对象
    TextMeshProUGUI _tmTxtBox;
    //初始page为1
    int _currentPage =1 ;
    //声明变量，存储总页数
    int _totalPage;
    void Start()
    {
        //对话框不显示
        dialogBox.SetActive(false);
        displaytimer = -1.0f;
        //获取对话框组件
        _tmTxtBox = digTexProGameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //获取对话框组件中对话文字的最大页数
        _totalPage = _tmTxtBox.textInfo.pageCount;
        if(displaytimer >= 0.0f)
        {
            //检测是否需要翻页
            if(Input.GetKeyUp(KeyCode.Space))
            {
                if(_currentPage<_totalPage)
                {
                    _currentPage++;
                }
                else
                {
                    _currentPage=1;
                }
            }
            displaytimer -= Time.deltaTime;
            if(displaytimer < 0)
            {
                dialogBox.SetActive(false);
            }
            //显示当前页面
            _tmTxtBox.pageToDisplay = _currentPage;
        }
    }
    //npc说话
    public void DisplayDialog()
    {
        displaytimer = displayTime;
        dialogBox.SetActive(true);
    }
}

