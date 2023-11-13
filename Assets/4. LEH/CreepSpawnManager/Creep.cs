using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep
{
    public string name;
    public int id;
    public int maxHp;
    public int hp;
    public int attackDamage;
    public float attackSpeed;
    public int attackRange;
    public float moveSpeed;

    //public int x;
    //public int y;

    public Avatar avatarPath; // + fbx .dae (Collada) .3ds, .dxf .obj 
    public Animator animatorPath; // + .controller
}
