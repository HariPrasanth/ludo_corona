// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Firebase;
// using Firebase.Database;
// using Firebase.Extensions;
// using Firebase.Unity.Editor;
//
// public class BackupDice : MonoBehaviour
// {
//     Rigidbody rb;
//     bool hasLanded;
//     bool thrown;
//
//     Vector3 initPosition;
//
//     public DiceSide[] diceSides;
//     public int diceValue;
//
//     void Start()
//     {
//         initPosition = transform.position;
//         rb = GetComponent<Rigidbody>();
//         rb.useGravity = false;
//         StartListener();
//     }
//
//     protected void StartListener() {
//       FirebaseDatabase.DefaultInstance
//         .GetReference("ludo").Child("gameRooms").Child("GR1")
//         .ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
//           if (e2.DatabaseError != null) {
//             return;
//           }
//
//           diceValue = int.Parse(e2.Snapshot.Child("diceValue").Value.ToString());
//         };
//     }
//
//     public void RollDice()
//     {
//         Reset();
//         if(!thrown && !hasLanded)
//         {
//             thrown = true;
//             rb.useGravity = true;
//             rb.AddTorque(Random.Range(0, 20000), Random.Range(0, 20000), Random.Range(0, 20000));
//         }
//         else if(thrown && hasLanded)
//         {
//             Reset();
//         }
//     }
//
//     void Reset()
//     {
//         transform.position = initPosition;
//         rb.isKinematic = false;
//         thrown = false;
//         hasLanded = false;
//         rb.useGravity = false;
//     }
//
//     void Update()
//     {
//         if(rb.IsSleeping() && !hasLanded && thrown)
//         {
//             hasLanded = true;
//             rb.useGravity = false;
//             rb.isKinematic = true;
//             //Side value check
//             SideValueCheck();
//         }
//         else if(rb.IsSleeping() && hasLanded && diceValue == 0)
//         {
//             //Roll dice again
//             RollAgain();
//         }
//     }
//
//     void RollAgain()
//     {
//         Reset();
//         thrown = true;
//         rb.useGravity = true;
//         rb.AddTorque(Random.Range(0, 20000), Random.Range(0, 20000), Random.Range(0, 20000));
//     }
//
//     void SideValueCheck()
//     {
//         // diceValue = 0;
//         foreach(DiceSide side in diceSides)
//         {
//             if(side.OnGround())
//             {
//                 // diceValue = side.sideValue;
//                 FirebaseDatabase.DefaultInstance.GetReference("ludo").Child("gameRooms").Child("GR1").Child("diceValue").SetValueAsync(diceValue);
//                 //Send result to game manager
//                 GameManager.instance.RollDice(diceValue);
//             }
//         }
//     }
// }
