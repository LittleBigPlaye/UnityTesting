using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Data", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    #region Descriptive Information
    public new string name;
    public string description;
    public Image icon;
    #endregion

    #region Damage Types
    public float baseDamage;
    public float fireDamage;
    public float iceDamage;
    public float magicDamage;
    #endregion
}
