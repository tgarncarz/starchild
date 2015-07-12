     using UnityEngine;
     using System.Collections;
     
     public class Camera : MonoBehaviour {
         
         public float dampTime, threshX, threshY;
         private Vector3 newPos, tempPos;
         public GameObject player;
     	
         // Update is called once per frame
         void Start (){
            threshX = 0.5f;
         	player = GameObject.Find("Player");
         }

         void Update () 
         {
            newPos = player.transform.position;
            tempPos = transform.position;
            if (newPos.x > tempPos.x + threshX){
                tempPos.x = Mathf.Lerp(transform.position.x, newPos.x, 0.75f);
            }
            tempPos.y = Mathf.Lerp(transform.position.y, newPos.y, 0.75f);
            transform.position = tempPos;
        }
    }

