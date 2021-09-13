using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStateManager : MonoBehaviour
{
    public virtual void GetHit(float damage) {}
    public virtual void EndState() {}
}
