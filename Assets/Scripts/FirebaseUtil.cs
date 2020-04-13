// ﻿using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Firebase;
// using Firebase.Database;
// using Firebase.Extensions;
// using Firebase.Unity.Editor;
//
// public class FirebaseUtil : MonoBehaviour
// {
//     ArrayList leaderBoard = new ArrayList();
//     protected bool isFirebaseInitialized = false;
//     public Text gameName;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         InitializeFirebase();
//     }
//
//     protected virtual void InitializeFirebase() {
//       FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ludo-3d-d15c1.firebaseio.com/");
//       FirebaseApp app = FirebaseApp.DefaultInstance;
//       StartListener();
//       isFirebaseInitialized = true;
//     }
//
//     protected void StartListener() {
//       FirebaseDatabase.DefaultInstance
//         .GetReference("ludo").Child("gameRooms").Child("GR1")
//         .ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
//           if (e2.DatabaseError != null) {
//             Debug.LogError(e2.DatabaseError.Message);
//             return;
//           }
//           gameName.text = e2.Snapshot.Child("diceValue").Value.ToString();
//           Debug.Log("Received values for Leaders.");
//         };
//     }
// }
