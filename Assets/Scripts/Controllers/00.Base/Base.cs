using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Base : MonoBehaviour
{
    public Rigidbody m_Rigidbody { get; set; }
    protected Collider m_Collider { get; set; }
    protected Animator m_Animator { get; set; }
    protected Animation m_Animaion { get; private set; }

    public int ID;
    public ObjectType eObjectType  = ObjectType.None;
    public Transform Pos { get; set; } = null;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        m_Animator = Util.GetOrAddComponent<Animator>(gameObject);
        m_Rigidbody = Util.GetOrAddComponent<Rigidbody>(gameObject);
        m_Collider = Util.GetOrAddComponent<Collider>(gameObject);

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        if(Pos != null)
            transform.position = Pos.position;
    }

    public virtual Base GetOwner()
    {
        return this;
    }
}
