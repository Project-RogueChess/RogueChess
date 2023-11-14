using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepComponent : MonoBehaviour
{
    public string creepName;
    public int id;
    public int maxHp;
    public int hp;
    public int attackDamage;
    public float attackSpeed;
    public int attackRange;
    public float moveSpeed;

    public Avatar avatar; // + .fbx .dae (Collada) .3ds .dxf .obj 
    public Animator animator; // + .controller

}
