using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTargetController : MonoBehaviour, IHitable
{
    public Color defaultColor = Color.gray;
    public Color selectedColor = Color.red;
    
    private Material material;
    
    private bool isSelected;
    public bool IsSelected {
        get { return isSelected;}
        set { 
            isSelected = value;
            material.color = (isSelected) ? selectedColor : defaultColor;
        }
    }

    public void OnHit(Damage damage, Vector3 weaponPosition)
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Awake() {
        material = GetComponent<MeshRenderer>().material;
        material.color = defaultColor;    
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
