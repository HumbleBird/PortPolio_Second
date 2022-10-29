using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    protected Rigidbody Rigid { get; set; }
    protected Collider Coller { get; set; }
    public Animator Animator { get; set; }

    public int ID { get; set; }
    public string Type { get; set; }
    public Vector3 Pos { get; set; }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Animator = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        Coller = GetComponent<Collider>();
    }


}
