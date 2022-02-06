using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class FreezeTagBehaviour : MonoBehaviour
{
    public Transform MoveTo;
    private float rotateSpeed = 20;
    public float speed = 1;
    private float dx;
    private float dz;
    public bool isTagger = false;
    private bool isFrozen = false;
    private float normVelX = 0, normVelZ = 0;
    private float kinSeekVelX = 0, kinSeekVelZ = 0;
    
    public LayerMask lm;
    public float rsat = 1.5f;
    public float t2t = 0.55f;
    double maxAccel = 12.25;
    
    private bool dodge = false;
    public bool seekFlee = true;
    
    public GameObject manager;
    private GameObject[] enemies;
    private GameObject tagger;
    
    void Update()
    {
        int victim = 0;
        if (isTagger)
        {
            
            float distance = 60f;
            for (int i = 0; i < enemies.Length; i++)
            {
                
                if(!enemies[i].GetComponent<FreezeTagBehaviour>().isFrozen)
                {
                    Debug.Log(enemies[i].GetComponent<FreezeTagBehaviour>().isFrozen);
                    //Debug.Log(enemies[i].GetComponent<GameObject>().name);
                    dx = enemies[i].transform.position.x - transform.position.x;
                    dz = enemies[i].transform.position.z - transform.position.z;

                    //if (Mathf.Abs(dx) > 15)
                    //{
                    //    dx = Mathf.Sign(transform.position.x) *
                    //         (30 - Mathf.Abs(MoveTo.position.x - transform.position.x));
                    //}
                    
                    //if (Mathf.Abs(dz) > 15)
                    //{
                    //    dz = Mathf.Sign(transform.position.z) *
                    //         (30 - Mathf.Abs(MoveTo.position.z - transform.position.z));
                    //}

                    if (Mathf.Sqrt(((dx * dx) + (dz * dz))) < distance)
                    {
                        victim = i;
                        MoveTo = enemies[victim].transform;
                        distance = Mathf.Sqrt(((dx * dx) + (dz * dz)));
                    }
                }
            }
            Debug.DrawRay(transform.position, new Vector3(dx, 0, dz), Color.green);
        }
        else
        {
            MoveTo = tagger.transform;
            
            dx = (MoveTo.position.x - transform.position.x);
            dz = (MoveTo.position.z - transform.position.z);

            if (Mathf.Abs(dx) > 25)
            {
                dx = Mathf.Sign(transform.position.x) * (50 - Mathf.Abs(MoveTo.position.x - transform.position.x));
            }
            if(Mathf.Abs(dz) > 25)
            {
                dz = Mathf.Sign(transform.position.z) * (50 - Mathf.Abs(MoveTo.position.z - transform.position.z));
            }
            
            
        }

        if (!isFrozen)
            HandleMovement();

    }
    
    void HandleMovement()
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

    public void SetTagger()
    {
        Debug.Log("set");
        isTagger = true;
        GetComponent<Renderer>().material.color = Color.red;
        speed = 10;
    }

    public void SetFleer()
    {
        seekFlee = false;
        Debug.Log("unset");
        isTagger = false;
        GetComponent<Renderer>().material.color = Color.yellow;
        speed = 1;
    }

    public void assignPlayers(GameObject t, GameObject [] f)
    {
        tagger = t;
        enemies = f;
    }

    public void freeze()
    {
        isFrozen = true;
        GetComponent<Renderer>().material.color = Color.cyan;
    }
    
    public void thaw()
    {
        isFrozen = false;
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (isTagger)
        {
            other.collider.GetComponent<FreezeTagBehaviour>().freeze();
        }
        else
        {
            if (!other.collider.GetComponent<FreezeTagBehaviour>().isTagger)
            {
                other.collider.GetComponent<FreezeTagBehaviour>().thaw();
            } 
        }
    }
}


