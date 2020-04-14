using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;
    bool hasLanded;
    bool thrown;

    Vector3 initPosition;

    public DiceSide[] diceSides;
    public int diceValue;

    public int pubDiceValue;
    void Start()
    {
        initPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void RollDice()
    {
        Reset();
        if(!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.maxAngularVelocity = Mathf.Infinity;
            rb.AddTorque(Random.Range(200,300), Random.Range(200,300), Random.Range(200,300));
        }
        else if(thrown && hasLanded)
        {
            Reset();
        }
    }

    void Reset()
    {
        transform.position = initPosition;
        rb.isKinematic = false;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    void Update()
    {
        if(rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            //Side value check
            SideValueCheck();
        }
        else if(rb.IsSleeping() && hasLanded && diceValue == 0)
        {
            //Roll dice again
            RollAgain();
        }
    }

    void RollAgain()
    {
        Reset();
        thrown = true;
        rb.useGravity = true;
        rb.maxAngularVelocity = Mathf.Infinity;
        rb.AddTorque(Random.Range(200,300), Random.Range(200,300), Random.Range(200,300));
    }

    void SideValueCheck()
    {
        diceValue = 0;
        foreach(DiceSide side in diceSides)
        {
            if(side.OnGround())
            {
                diceValue = side.sideValue;
                //Send result to game manager
                // GameManager.instance.RollDice(diceValue);
                GameManager.instance.RollDice(pubDiceValue);
            }
        }
    }
}
