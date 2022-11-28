using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
     //声明音频源对象，用来后期进行音频播放设置
     AudioSource audioSource;
     public AudioClip ThowCogClip;
     public float ThowCogSoundVol = 1.0f;
     public AudioClip playerHitClip;
     public float playerHitSoundVol = 1.0f;
     //声明一个公开的游戏对象ProjectilePrefab，用来获取子弹的预制件对象
     public GameObject ProjectilePrefab;
     //设置玩家无敌时间间隔
     public float timeInvincible = 2.0f;
     //设置是否无敌的变量
     bool isInvincible;
     //定义变量，进行无敌时间的计时，无敌时间计时器
     float InvincibleTimer;
     //最大生命
     public int MaxHealth = 5 ;
     //开始生命
     //设置生命属性health

     /*c#中文支持面向对象程序设计中的封装概念，对数据成员的保护
     设计成员变量，默认都应该私有，只 能通过类的方法或属性进行访问
     属性是共有的，可以通过取值器get，赋值器set设定对应字段的访问规则，满足规则才能访问
     */
     public int health{
          get{return currentHealth;}
          //set{currentHealth=value;}
          }
     int currentHealth;
    //声明刚体对象
    Rigidbody2D rigidbody2d;
    //获取用户输入
    float horizontal;
    float vertical; 
    public float speed =0.1f;
   // 在第一次帧更新之前调用 Start
   //声明一个动画管理者组件对象；
   Animator animator;
   //创建一个二维矢量，用来存储ruby静止不动时面对的direction；
   //与机器人相比，ruby可以静止不动，Move x/y都为0，此时状态机无法获取ruby的朝向，所以手动设置
   Vector2 LookDirection = new Vector2(1,0); 
   Vector2 move;

   private void Start()
   { 
         //开始生命值
        currentHealth = MaxHealth;
        //获取当前组件的刚体组件
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //获取声音源组件对象
        audioSource=GetComponent<AudioSource>();
  
   } 
   // 每帧调用一次 Update
   void Update()
   {
       horizontal = Input.GetAxis("Horizontal");
       vertical = Input.GetAxis("Vertical");

       //判断是否处于无敌状态,来进行计时器的倒计时
       if(isInvincible){
          //如果无敌，进入倒计时
          //每次update减去一帧所消耗的时间
          InvincibleTimer -= Time.deltaTime;
          //直到计时器的时间用完
          if(InvincibleTimer < 0){
               //取消无敌状态
               isInvincible = false;
          }
       }
       /*创建一个二维矢量对象来表示ruby移动的数据
       如果move中的xy为0，则表示移动
       将ruby面对方向设置为移动方向
       停止运动时，保持面对方向，所以这个if是给对象转向时重新赋值面对方向的；
       */
       move = new Vector2(horizontal,vertical);
       if(!Mathf.Approximately(move.x,0.0f)||!Mathf.Approximately(move.y,0.0f))
       {
          //将现在runby面朝方向设置为面移动方向
          LookDirection.Set(horizontal,vertical);
          /*
          归一化使其长度为1，通常用于方向而非向量上；
          blend tree取值范围为-1.0到1.0
          所以一般用向量作为animator.setfloat的参数时，先对向量进行归一化；
          */
          LookDirection.Normalize();
       }
       //传递ruby的面朝方向给blend tree；
       animator.SetFloat("Look X",LookDirection.x);
       animator.SetFloat("Look Y",LookDirection.y);
       /*传递ruby的speed给blend tree
        矢量的magnitude用来返回矢量之长度,是一个绝对值*/
       animator.SetFloat("Speed",move.magnitude); 


       //添加发射子弹的逻辑获取（玩家输入一般放在update）
       //if(Input.GetKeyDown(KeyCode.C) || Input.GetAxis("Fire1")!=0) -------教程范例
       if(Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Fire1")) 
       {
          Launch();
       }    

       if(Input.GetKeyDown(KeyCode.X))
       {
          //创建一个射线投射碰撞对象，来接收射线投射碰撞信息
          //射线投射使用的是phys2d.raycast
          //参数1： 射线投射的位置
          //参数2： 投射反向
          //参数3： 投射距离
          //参数4： 参数生效的层
          RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position+Vector2.up*0.2f,LookDirection,1.5f,LayerMask.GetMask("NPC"));
          if(hit.collider != null)
          {
               Debug.Log($"射线投射碰撞到的对象是：{hit.collider.gameObject}");
               //创建npc代码组件
               NPCCharacter npc = hit.collider.GetComponent<NPCCharacter>();
               if(npc != null)
               {
                    //调用npc组件
                    npc.DisplayDialog();
               }
          }
       }
   }
   private void FixedUpdate()
   {
     //movement
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime; 
        rigidbody2d.position = position;


   }
 
   public void ChangeHealth(int amount)
   {
     //假设玩家受伤害的间隔为1秒
     if(amount<1){
          //判断当前是否无敌
          if(isInvincible){
               //无敌不扣血，跳出函数
               return;
          }
          /*当不是无敌状态，会执行下面代码
          重置无敌状态为真
          重置无敌时间
          */
          isInvincible =true;
          InvincibleTimer=timeInvincible;
          //播放受伤动画hit
          animator.SetTrigger("Hit"); 
          PlaySound(playerHitClip,playerHitSoundVol);

     }
     //系统函数clamp为设置取值范围的函数;限制范围为0——MaxHealth
     currentHealth = Mathf.Clamp(currentHealth + amount ,0,MaxHealth);
     //输出生命信息
     //Debug.Log("当前生命值：" + currentHealth +"/"+ MaxHealth);

     UIHealthBar.Instance.SetValue(currentHealth/(float)MaxHealth);
   }

   void Launch()
   {
     //创建子弹游戏对象，三个参数依次是（你要生成什么，生成位置在哪，要不要旋转；）
     GameObject projectileObject = Instantiate(ProjectilePrefab,rigidbody2d.position+Vector2.up*0.5f,Quaternion.identity);
     //Projectile（子弹挂的脚本类） projectile（用户自定义名称） = proObject（子弹这个对象）
     //获取子弹脚本组件
     Projectile projectile = projectileObject.GetComponent<Projectile>();
     //通过脚本对象调用子弹移动的方法
     //参数一取得玩家面朝方向；参数二是力的大小；
     projectile.Launch(LookDirection,300);
     animator.SetTrigger("Launch");
     PlaySound(ThowCogClip,ThowCogSoundVol);
   }
   //新建一个共有的方法，用来播放音频剪辑
   public void PlaySound(AudioClip audioClip,float SoundVol)
   {
     //调用音频源的playoneshot方法，播放指定音频；
     audioSource.PlayOneShot(audioClip,SoundVol); 
   }

}
 