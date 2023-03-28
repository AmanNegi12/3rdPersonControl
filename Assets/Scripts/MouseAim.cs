using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour
{
    [SerializeField] GameObject Aimpos;
    [SerializeField] LayerMask LayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        // If the ray hits an object, store the position of the hit
        shoot();



    }
    void shoot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0f));
        if (Physics.Raycast(ray, out hit, 100f))
        {
          Aimpos.transform.position = hit.point;

        }
        else
        {
          Aimpos.transform.position = ray.GetPoint(100f);

        }
        
       
    }
}
