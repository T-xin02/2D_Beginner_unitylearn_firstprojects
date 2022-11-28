using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagzons : MonoBehaviour
{
    //每次减血量
    public int damageNUM=-1;
    //刚体在触发器内的每一帧都会调用函数，而不是进入时调用一次（即连续掉血不是站在钉子上只扣一次）
    private void OnTriggerStay2D(Collider2D other)
    {
        RubyController rubyController = other.GetComponent<RubyController>();

        if (rubyController != null){
            rubyController.ChangeHealth(damageNUM);
        }
    }
}
