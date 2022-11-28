using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ParticleSystem CollectEffect;
    //聲明刚体
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Awake()
    {
        //获取刚体实例
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //如果在没有碰到任何碰撞体，在飞行”100“后destroy
        if(transform.position.magnitude > 50.0f)
        {
            Destroy(gameObject);
        }
    }
    //飞弹发射
    public void Launch(Vector2 direction,float force )
    {
        //通过刚体调用物理系统中的addforce
        //对游戏对象施加一个力，使其移动；
        rigidbody2d.AddForce(direction*force);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //获取子弹碰撞到的机器人对象脚本组件
        EnemyController02 enemyController02 = collision.collider.GetComponent<EnemyController02>();
        if(enemyController02 != null)
        {
            enemyController02.FIX();
        }
        Destroy(gameObject);
        //实例化
        Instantiate(CollectEffect,transform.position,Quaternion.identity);
    }
}
