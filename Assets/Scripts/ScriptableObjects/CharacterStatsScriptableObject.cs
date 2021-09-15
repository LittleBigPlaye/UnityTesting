using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Data", menuName = "ScriptableObjects/Character Stats", order = 1)]
public class CharacterStatsScriptableObject : ScriptableObject
{
    #region General Information
    public float maxHealth;
    public float maxStamina;
    #endregion


    #region Damage Vulnerabilities
    [Range(0,1)]
    public float baseDamageVulnerability;
    [Range(0,1)]
    public float fireDamageVulnerability;
    [Range(0,1)]
    public float iceDamageVulnerability;
    [Range(0,1)]
    public float magicDamageVulnerability;
    #endregion


}