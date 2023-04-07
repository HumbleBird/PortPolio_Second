using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;
using UnityEngine;

public class Projectile : Base
{
	public Base Owner { get; private set; }

	public Projectile()
	{
		eObjectType = ObjectType.Projectile;
	}
}
