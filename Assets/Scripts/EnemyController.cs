using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //设定移动速度变量
    public float speed = 0.1f;
    public float distance = 4;
    //声明一个Vector2对象来存储当前位置
    Vector2 position;
    
    Rigidbody2D rigidbody2d;
    float initY;
    float direction;
  
    // Start is called before the first frame update
    void Start()
    {
        /*获取对象的变量或值
        */
        rigidbody2d = GetComponent<Rigidbody2D>();
        position = transform.position;
        initY = position.y;
        direction = 1.0f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MovePosition();
    
    }
    private void MovePosition()
    {
        if(position.y-initY < distance && direction > 0)
        {
            position.y +=speed;
        }
        if(position.y -initY >= distance && direction > 0)
        {
            direction = -1.0f;
        }
        if(position.y -initY >0 && direction < 0)
        {
            position.y -=speed;
        }
        if(position.y -initY <= 0 && direction < 0)
        {
            direction =1.0f;
        }
        rigidbody2d.position =position;
    }

}
