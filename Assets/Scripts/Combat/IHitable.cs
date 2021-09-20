using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    void OnHit(Damage damage, Vector3 weaponPosition);
}
