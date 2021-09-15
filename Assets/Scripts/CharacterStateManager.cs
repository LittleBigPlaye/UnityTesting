using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CharacterStateManager : MonoBehaviour
{
    [SerializeField] CharacterStatsScriptableObject characterStats;
    public CharacterStatsScriptableObject CharacterStats {get => characterStats;}

    public abstract void GetHit(float damage);
    public abstract void EndState();
}
