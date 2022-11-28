using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthcolllectible : MonoBehaviour
{
    //声明也该音频剪辑字段
    public AudioClip CollectedClip;
    public float CollectSoundVol = 1.0f;
    //草莓加的血量
    public int amount = 1;
    //用来记录碰撞次数
    int collideCount;
    //添加碰撞触发器事件，每次碰撞触发器，执行其中代码
    public ParticleSystem AddHealthEffect;
    private void OnTriggerEnter2D(Collider2D other)
    {
        collideCount++;
        Debug.Log($"和当前物体发生碰撞的物体是：{other},当前是第{collideCount}次碰撞");
        //获取ruby游戏对象的脚本组件对象
        RubyController rubyController = other.GetComponent<RubyController>();
        if(rubyController!=null)
        {
            if(rubyController.health < rubyController.MaxHealth)
            {
                //更改生命值
                rubyController.ChangeHealth(amount);
                //销毁当前游戏对象,草莓被吃掉
                Destroy(gameObject);

                rubyController.PlaySound(CollectedClip,CollectSoundVol);
                
                //实例化
                Instantiate(AddHealthEffect,transform.position,Quaternion.identity);
            }
            else
            {
                Debug.Log("当前玩家生命充足，不需要加血！");
            }          
        }
        else
        {
            Debug.Log("rubyController游戏组件并未获取到");
        }       
    }
}
