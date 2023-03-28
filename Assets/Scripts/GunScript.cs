using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
   public bool Isfiring = false;
   [SerializeField] ParticleSystem particle;
   [SerializeField] ParticleSystem particleImpact;
    [SerializeField] TrailRenderer tracereffect;
   [SerializeField] GameObject StartingPoint;
    [SerializeField] int bulletpersecond=25;
    float TimeBetweenbullets=0f;



    public void UpdateFiring(float deltatime)
    {
        TimeBetweenbullets += deltatime;
        float fireintervals = 1f/bulletpersecond;
        while (TimeBetweenbullets>=0.0f)
        {
            FireCode();
            TimeBetweenbullets -= fireintervals;

        }
    }

    public void StartFiring()
    {

       Isfiring = true;
      
       
    }
    public void StopFiring()
    {
      Isfiring = false;

    }
    void FireCode()
    {
        particle.Emit(1);
        Vector3 StartPoint = StartingPoint.transform.position;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        if (Physics.Raycast(ray, out hit, 100f))
        {
            var tracer = Instantiate(tracereffect, StartPoint, Quaternion.identity);
            tracer.AddPosition(StartPoint);
            tracer.transform.position = hit.point;

            particleImpact.transform.position = hit.point;
            particleImpact.transform.forward = hit.normal;
            particleImpact.Emit(1);
            Debug.DrawLine(StartPoint, hit.point, Color.red, 1f);
        }
        else
        {
            Debug.DrawLine(StartPoint, ray.GetPoint(100f), Color.red, 1f);
            var tracer = Instantiate(tracereffect, StartPoint, Quaternion.identity);
            tracer.AddPosition(StartPoint);
            tracer.transform.position = ray.GetPoint(100f);
        }
    }
}
