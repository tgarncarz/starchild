     using UnityEngine;
     using System.Collections;
     
     public class Camera : MonoBehaviour {
         
         public float dampTime, threshX, threshY;
         private Vector3 newPos, tempPos;
         public Transform playerTransform;
     	
         // Update is called once per frame
         void Start (){
            threshX = 0.5f;
         	playerTransform = GameObject.Find("Player").transform;
         }

         void Update () 
         {
            transform.position = new Vector3(playerTransform.position.x,playerTransform.position.y, -100f);

        }
    }

