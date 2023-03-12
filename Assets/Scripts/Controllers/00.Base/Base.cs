using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Base : MonoBehaviour
{
    public Rigidbody Rigid { get; set; }
    protected Collider m_Collider { get; set; }
    protected Animator Animator { get; set; }

    public int ID;
    public ObjectType eObjectType { get; set; }
    public Transform Pos { get; set; } = null;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Animator = Util.GetOrAddComponent<Animator>(gameObject);
        Rigid = Util.GetOrAddComponent<Rigidbody>(gameObject);
        m_Collider = Util.GetOrAddComponent<Collider>(gameObject);

        Rigid.constraints = RigidbodyConstraints.FreezeRotation;

        if(Pos != null)
            transform.position = Pos.position;
    }

    public virtual Base GetOwner()
    {
        return this;
    }
}
