using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class part1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("1a");
        ONEa();
        reset();
        Debug.Log("1b");
        ONEb();
        reset();
        //1c steering seek curves into the position while kinematic seems goes straight to the point 
        Debug.Log("1d");
        ONEd();
        reset();
        Debug.Log("1e");
        ONEe();
        reset();
        //1f both slow down as they approach the saturation radius however steering seek curves into the position while kinematic seems goes straight to the point 
        Debug.Log("2a");
        TWOa();
        reset();
        Debug.Log("2b");
        TWOb();
        reset();
        
    }

    double pInitX = 3;
    double pInitY = 6;
    double vInitX = 2;
    double vInitY = 3;
    double pTargetX = 5;
    double pTargetY = 4;
    double dt = 0.25;
    double maxVel = 3.6;
    double maxAccel = 12.25;

    double nextVelX, nextVelY;
    double normVelX, normVelY;
    double kinSeekVelX, kinSeekVelY;

    double steerAccelX, steerAccelY;
    double steeringSeekVelX, steeringSeekVelY;

    double rsat = 0.5;
    double t2t = 0.55;

    double avgActPX = (21 + 5 + 28) / 3;
    double avgActPY = (16 + 11 + 9) / 3;

    double avgActVX = (3 + 3 + 6) / 3;
    double avgActVY = (1 + 3 + 5) / 3;

    double avgAssSlotPX = (22 + 6 + 29) / 3;
    double avgAssSlotPY = (18 + 13 + 12) / 3;
    
    public void reset()
        {
            float pInitX = 3;
            float pInitY = 6;
            float vInitX = 2;
            float vInitY = 3;
            float pTargetX = 5;
            float pTargetY = 4;
            float dt = 0.25f;
            float maxVel = 3.6f;
            float maxAccel = 12.25f;

            float nextVelX, nextVelY;
            float normVelX, normVelY;
            float kinSeekVelX, kinSeekVelY;

            float steerAccelX, steerAccelY;
            float steeringSeekVelX, steeringSeekVelY;

            float rsat = 0.5f;
            float t2t = 0.55f;
            
            float avgActPX;
            float avgActPY;
            
            float avgActVX;
            float avgActVY;
            
            float avgAssSlotPX;
            float avgAssSlotPY;
        }

        public void ONEa() 
        {
            for (int i = 0; i < 5; ++i)
            {
                nextVelX = pTargetX - pInitX;
                nextVelY = pTargetY - pInitY;
                normVelX = nextVelX / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                normVelY = nextVelY / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                kinSeekVelX = maxVel * normVelX;
                kinSeekVelY = maxVel * normVelY;
                pInitX = pInitX + (kinSeekVelX * dt);
                pInitY = pInitY + (kinSeekVelY * dt);
                Debug.Log(pInitX + ", " + pInitY);
            }
        }
        void ONEb()
        {
            for (int i = 0; i < 5; ++i)
            {
                nextVelX = pTargetX - pInitX;
                nextVelY = pTargetY - pInitY;
                steerAccelX = maxAccel * nextVelX / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                steerAccelY = maxAccel * nextVelY / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                nextVelX = vInitX + (dt * steerAccelX);
                nextVelY = vInitY + (dt * steerAccelY);
                
                pInitX = pInitX + (nextVelX * dt);
                pInitY = pInitY + (nextVelY * dt);

                Debug.Log(pInitX + ", " + pInitY);
            }
        }
        
        void ONEd()
        {
            for (int i = 0; i < 5; ++i)
            {
                nextVelX = pTargetX - pInitX;
                nextVelY = pTargetY - pInitY;
                if (Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY))) > rsat)
                {
                    normVelX = nextVelX / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                    normVelY = nextVelY / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                    kinSeekVelX = maxVel * normVelX;
                    kinSeekVelY = maxVel * normVelY;
                    
                }
                else
                {
                    normVelX = nextVelX / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                    normVelY = nextVelY / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                    double tempMaxVel = Mathf.Min((float) maxVel, (float)(Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY))) / t2t));
                    kinSeekVelX = tempMaxVel * normVelX;
                    kinSeekVelY = tempMaxVel * normVelY;
                }
                
                pInitX = pInitX + (kinSeekVelX * dt);
                pInitY = pInitY + (kinSeekVelY * dt);
                Debug.Log(pInitX + ", " + pInitY);
            }
        }
        
        void ONEe()
        {
            for (int i = 0; i < 5; ++i)
            {
                nextVelX = pTargetX - pInitX;
                nextVelY = pTargetY - pInitY;
                steerAccelX = maxAccel * nextVelX / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                steerAccelY = maxAccel * nextVelY / Mathf.Sqrt((float) ((nextVelX * nextVelX) + (nextVelY * nextVelY)));
                nextVelX = vInitX + (dt * steerAccelX);
                nextVelY = vInitY + (dt * steerAccelY);

                pInitX = pInitX + (nextVelX * dt);
                pInitY = pInitY + (nextVelY * dt);
                Debug.Log(pInitX + ", " + pInitY);
            }
        }


        void TWOa()
        {
            avgActPX = (21 + 5 + 28) / 3;
            avgActPY = (16 + 11 + 9) / 3;
            avgActVX = (3 + 3 + 6) / 3;
            avgActVY = (1 + 3 + 5) / 3;
            avgAssSlotPX = (22 + 6 + 29) / 3;
            avgAssSlotPY = (18 + 13 + 12) / 3;
            Debug.Log("Panchor ( " + (avgActPX + avgActVX) + ", " + (avgActPY + avgActVY) + " )");
        }
        void TWOb()
        {
            avgActPX = (21 + 5 + 28) / 3;
            avgActPY = (16 + 11 + 9) / 3;
            avgActVX = (3 + 3 + 6) / 3;
            avgActVY = (1 + 3 + 5) / 3;
            avgAssSlotPX = (22 + 6 + 29) / 3;
            avgAssSlotPY = (18 + 13 + 12) / 3;
            Debug.Log("Psi ( " + (avgAssSlotPX - (avgActPX + avgActVX)) + ", " + (avgAssSlotPY - (avgActPY + avgActVY)) + " )");
        }
        void TWOc()
        {
            avgActPX = (21 + 5) / 2;
            avgActPY = (16 + 11) / 2;
            avgActVX = (3 + 3) / 2;
            avgActVY = (1 + 3) / 2;
            avgAssSlotPX = (22 + 6) / 2;
            avgAssSlotPY = (18 + 13) / 2;
            
            Debug.Log("Psi ( " + (avgAssSlotPX - (avgActPX + avgActVX)) + ", " + (avgAssSlotPY - (avgActPY + avgActVY)) + " )");
            Debug.Log("Panchor ( " + (avgActPX + avgActVX) + ", " + (avgActPY + avgActVY) + " )");
        }
}
