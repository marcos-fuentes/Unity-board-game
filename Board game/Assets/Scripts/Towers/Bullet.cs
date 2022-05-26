using System.Collections;
using System.Collections.Generic;
using Managers;
using Units;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    //ANIMATIONS
    private Animator anim;
    private BaseUnit _tileUnit; 
    
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BulletDropped()
    {
        if (_tileUnit!= null) {
            ExplosionSound();
            _tileUnit.DamageUnit(1);
        }
    }
    
    
    public void AnimBullet(BaseUnit unit)
    {
        _tileUnit = unit;
        Debug.Log("Bullet animation!!!");

        if (anim != null)
        {
            Debug.Log("Bullet Attack anim");
            anim.Play("Bullet animation");
        }

    }

    public void ExplosionSound() => SoundManager.Instance.playExplosionSound();

}
