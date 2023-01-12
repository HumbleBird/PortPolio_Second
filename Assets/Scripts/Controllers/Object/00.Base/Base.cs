using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Base : MonoBehaviour
{
    public Rigidbody Rigid { get; set; }
    protected Collider Coller { get; set; }
    protected Animator Animator { get; set; }

    public int ID { get; set; }
    public CreatureType eCreatureType { get; set; }
    protected Vector3 Pos { get; set; } = Vector3.zero;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Animator = Util.GetOrAddComponent<Animator>(gameObject);
        Rigid = Util.GetOrAddComponent<Rigidbody>(gameObject);
        Coller = Util.GetOrAddComponent<Collider>(gameObject);

        transform.position = Pos;
    }


}
