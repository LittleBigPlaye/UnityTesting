using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CharacterStateManager : MonoBehaviour
{
    public abstract void GetHit(float damage);
    public abstract void EndState();
}
