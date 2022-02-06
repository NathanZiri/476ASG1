﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public Transform MoveTo;
    public float speed = 1;
    private float dx;
    private float dz;
        
    private float normVelX = 0, normVelZ = 0;
    private float kinSeekVelX = 0, kinSeekVelZ = 0;
    public LayerMask lm;
    public float rsat = 0.5f;
    public float t2t = 0.55f;
    double maxAccel = 12.25;
    private bool dodge = false;
    public bool seekFlee = true;
    void Update()
    {
        dx = (MoveTo.position.x - transform.position.x);
        dz = (MoveTo.position.z - transform.position.z);

         if (Mathf.Abs(dx) > 15)
        {
            dx = Mathf.Sign(transform.position.x) * (30 - Mathf.Abs(MoveTo.position.x - transform.position.x));
        }
        if(Mathf.Abs(dz) > 15)
        {
            dz = Mathf.Sign(transform.position.z) * (30 - Mathf.Abs(MoveTo.position.z - transform.position.z));
        }
        
        Debug.DrawRay(transform.position, new Vector3(dx, 0, dz), Color.green);
        
        if(seekFlee)
        {
            if(Mathf.Sqrt((kinSeekVelX * kinSeekVelX) + (kinSeekVelZ * kinSeekVelZ)) < 0.25f)
            {
                if (Mathf.Sqrt((dx * dx) + (dz * dz)) < 5)
                {
                    kinArrive();
                    dodge = true;
                }
                else
                {
                    kinArriveII();
                    dodge = false;
                }
            }
            else
            {
                coneRotate();
            }
        }
        else
        {
            dx = -dx;
            dz = -dz;
            if (Mathf.Sqrt((dx * dx) + (dz * dz)) < 5)
            {
                KinFlee();
                dodge = true;
            }
            else
            {
                KinFleeII();
                dodge = false;
            }
        }
    }

    void kinArrive()
    {
        float fx = dodge? dx : transform.forward.x;
        float fz = dodge? dz : transform.forward.z;
        if (Mathf.Sqrt((float) ((dx * dx) + (dz * dz))) > rsat)
        {
            normVelX =  fx/ Mathf.Sqrt(((fx * fx) + (fz * fz)));
            normVelZ = fz / Mathf.Sqrt(((fx * fx) + (fz * fz)));
            kinSeekVelX = speed * normVelX;
            kinSeekVelZ = speed * normVelZ; 
                    
        }
        else
        {
            normVelX = fx / Mathf.Sqrt( ((fx * fx) + (fz * fz)));
            normVelZ = fz / Mathf.Sqrt( ((fx * fx) + (fz * fz)));
            float tempMaxVel = Mathf.Min(speed, (Mathf.Sqrt(((fx * fx) + (fz * fz))) / t2t));
            kinSeekVelX = tempMaxVel * normVelX;
            kinSeekVelZ = tempMaxVel * normVelZ;
        }
        
        float moveToPosx = transform.position.x + (kinSeekVelX * Time.deltaTime);
        float moveToPosz = transform.position.z + (kinSeekVelZ * Time.deltaTime);
        transform.position = new Vector3(moveToPosx, 1, moveToPosz);
    }
    
    void kinArriveII()
    {
        
        Debug.DrawRay(transform.position, transform.forward*10);
        float ang = Vector3.Angle(transform.forward, new Vector3(dx, 0, dz));
        if(ang > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(dx, 0, dz), Vector3.up), 20*Time.deltaTime);
        }
        else
        {
            kinArrive();
        }
        
        
    }

    void coneArrive()
    {
        float fx = transform.forward.x;
        float fz = transform.forward.z;
        if (Mathf.Sqrt(((dx * dx) + (dz * dz))) > rsat)
        {
            normVelX =  fx/ Mathf.Sqrt(((fx * fx) + (fz * fz)));
            normVelZ = fz / Mathf.Sqrt(((fx * fx) + (fz * fz)));
            kinSeekVelX = speed * normVelX;
            kinSeekVelZ = speed * normVelZ; 
                    
        }
        else
        {
            normVelX = fx / Mathf.Sqrt(((fx * fx) + (fz * fz)));
            normVelZ = fz / Mathf.Sqrt(((fx * fx) + (fz * fz)));
            float tempMaxVel = Mathf.Min(speed, (Mathf.Sqrt(((fx * fx) + (fz * fz))) / t2t));
            kinSeekVelX = tempMaxVel * normVelX;
            kinSeekVelZ = tempMaxVel * normVelZ;
        }
        
        float moveToPosx = transform.position.x + (kinSeekVelX * Time.deltaTime);
        float moveToPosz = transform.position.z + (kinSeekVelZ * Time.deltaTime);
        transform.position = new Vector3(moveToPosx, 1, moveToPosz);
    }
    
    void coneRotate()
    {
        float ang = Vector3.Angle(transform.forward, new Vector3(dx, 0, dz));
        if(ang > 50f * Mathf.Sqrt(((normVelX * normVelX) + (normVelZ * normVelZ))))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(dx, 0, dz), Vector3.up), 5*Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(dx, 0, dz), Vector3.up), 5*Time.deltaTime);
            coneArrive();
        }
    }
    
    void KinFlee()
    {
        
        if (Mathf.Sqrt((float) ((dx * dx) + (dz * dz))) > rsat)
        {
            normVelX = dx / Mathf.Sqrt((float) ((dx * dx) + (dz * dz)));
            normVelZ = dz / Mathf.Sqrt((float) ((dx * dx) + (dz * dz)));
            kinSeekVelX = speed * normVelX;
            kinSeekVelZ = speed * normVelZ; 
                    
        }
        else
        {
            normVelX = dx / Mathf.Sqrt((float) ((dx * dx) + (dz * dz)));
            normVelZ = dz / Mathf.Sqrt((float) ((dx * dx) + (dz * dz)));
            float tempMaxVel = Mathf.Min((float) speed, (float)(Mathf.Sqrt((float) ((dx * dx) + (dz * dz))) / t2t));
            kinSeekVelX = tempMaxVel * normVelX;
            kinSeekVelZ = tempMaxVel * normVelZ;
        }
        
        float moveToPosx = transform.position.x + (kinSeekVelX * Time.deltaTime);
        float moveToPosz = transform.position.z + (kinSeekVelZ * Time.deltaTime);
        transform.position = new Vector3(moveToPosx, 1, moveToPosz);
        //Debug.Log(pInitX + ", " + pInitY);
    }
    
    void KinFleeII()
    {
        Debug.DrawRay(transform.position, transform.forward*30);
        float ang = Vector3.Angle(transform.forward, new Vector3(dx, 0, dz));
        if(ang > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(dx, 0, dz)), 20*Time.deltaTime);
        }
        else
        {
            KinFlee();
        }
    }
}
