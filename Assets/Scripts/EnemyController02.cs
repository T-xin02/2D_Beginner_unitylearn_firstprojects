    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController02 : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip FixedClip;
     public float FixedSoundVol = 1.0f;
    public AudioClip walkClip;
    public float walkSoundVol = 1.0f;
    public float speed;
    //是否垂直
    public bool vertical;
    //朝一个方向移动的总时间
    public float changeTime = 3.0f;
    //声明刚体
    Rigidbody2D rigidbody2d;
    //计时器
    float timer;
    int direction=1;
    Animator animator;
    //设定一个bool值，初始代表机器人是“坏“
    bool broked = true;
    //声明一个特效smokeeffect
    public ParticleSystem SmokeEffect;
    void Start()
    {
        //获取当前刚体组件
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        //animator获取参数；
        animator = GetComponent<Animator>(); 
        audioSource=GetComponent<AudioSource>();   
    }
    void Update()
    {
        //如若已修复机器人，则直接退出update，不再移动
        if(!broked)
        {
            return;
        }
        //如若timer小于0则反转方向，重置timer
        timer -= Time.deltaTime;
        if(timer<0)
        {
            direction = -direction;
            timer = changeTime;
        }
        
    }
    private void FixedUpdate()
    {
         //如若已修复机器人，则直接退出update，不再移动
        if(!broked)
        {
            return;
        }
        //获取当前位置
        Vector2 position = rigidbody2d.position;
        if(vertical)
        {
            position.y=position.y + Time.deltaTime * speed * direction;
            //参数值发送到 Animator。我们可以通过 Animator 上的 SetFloat 函数
            animator.SetFloat("Move X",0);
            animator.SetFloat("Move Y",direction);
        }
        else
        {
            position.x=position.x + Time.deltaTime * speed * direction;
            //参数值发送到 Animator。我们可以通过 Animator 上的 SetFloat 函数
            animator.SetFloat("Move X",direction);
            animator.SetFloat("Move Y",0);
        }
        rigidbody2d.MovePosition(position);
    }

    //刚体碰刚体OncollisionEnter(刚体碰撞事件)    enemy对玩家的伤害
    private void OnCollisionEnter2D(Collision2D other)
    {
        //获取玩家
        RubyController rubyController = other.gameObject.GetComponent<RubyController>();
        //如若碰到机器人，则ruby扣血
        if(rubyController!=null)
        {
            //调用
            rubyController.ChangeHealth(-1);
        }

    }
    //飞轮修复机器人方法
    public void FIX()
    //更改状态已修复
    {
 
        broked = false;
        //让机器人不再碰撞；取消物理引擎（刚体）
        rigidbody2d.simulated = false;
        //播放修复好的动画
        //SetInteger、SetFloat、SetBool、SetTrigger分别对应Paramters中的Int、Float、Bool、Trigger类型;
        animator.SetTrigger("Fixed");

        
        //结束播放厌恶
        //Destroy(SmokeEffect);
        //特效停止，产生新的特效，结束特效
        //原有已生产粒子会走完最后一程
        SmokeEffect.Stop();
        //获取和机器人挂接的声音源组件，让其停止播放 
        GetComponent<AudioSource>().Stop();
    
    }
    public void ifwalkSound(AudioClip pauseaudioClip,float pauseSoundVol)
    {
        audioSource.PlayOneShot(pauseaudioClip,pauseSoundVol);
    }
   
}
