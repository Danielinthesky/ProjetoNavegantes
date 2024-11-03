using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class FloaterObject : MonoBehaviour
{
    public Transform[] floaters;
    public float underWaterDrag = 3f;
    public float angularWaterDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public float waterHeight = 0f;
    public int floatersUnderwater;
    bool underWater;
    
    OceanManager oceanManager;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        oceanManager = GetComponent<OceanManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        floatersUnderwater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float difference = floaters[i].position.y - oceanManager.WaterHeightAtPosition(floaters[i].position);

            if (difference < 0)
            {
                rb.AddForceAtPosition(Vector3.up * floatingPower * Math.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderwater += 1;
                if (!underWater)
                {
                    underWater = true;
                    SwitchState(true);
                }
            }

            if (underWater && floatersUnderwater == 0)
            {
                underWater = false;
                SwitchState(false);
            }
        }

    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            rb.drag = underWaterDrag;
            rb.angularDrag = angularWaterDrag;
        }
        else
        {
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }


}
