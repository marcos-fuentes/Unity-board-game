using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Units;
using UnityEngine;

public class BaseTower : MonoBehaviour {
    
    internal HealthSystem HealthSystem;
    internal HealSystem HealSystem;
    public HealthBar healthBar;
    public HealBar healBar;
    public Faction faction;
    public bool isAlive = true;

    
    //ANIMATIONS
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        HealthSystem = new HealthSystem(3);
        healthBar.Setup(HealthSystem);
        HealSystem = new HealSystem(3);
            healBar.Setup(HealSystem);
        anim = gameObject.GetComponent<Animator>();
        Debug.Log("BaseTowerCreated "+anim.name);
        IdleAnimation();
    }
    
    public void HealTower(int heal) => HealthSystem.Heal(heal);
    
    public bool DamageTower(int damage) {
        var isDead = false;
        HealthSystem.Damage(damage);
        if (HealthSystem.GetHealth() <= 0)
        {
            Debug.Log("Dying");
            isDead = true;
            isAlive = false;
        }
        return isDead;
    }
    
    public void Attack()
    {
        Debug.Log("Attack!!!" + anim);


        if (anim == null) {
            anim = gameObject.GetComponent<Animator>();
            Debug.Log("Tower Attack anim");
            anim.Play("Attack");
        }
        else {
            Debug.Log("Tower Attack anim");
            anim.Play("Attack");
        }
    }

    public void AttackShot()
    {
        Debug.Log("Tower Shot");
        GameManager.Instance.TowerAttack();   
    }
    
    public void IdleAnimation() => anim.Play("Idle");
}
