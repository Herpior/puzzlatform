using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour {

     GameObject climber;
     bool canClimb = false;
     public float speed = 1;
     public float top;
     public float bottom;
     private CharacterControl ctrl;
 
    // Use this for initialization
    void Start () {
        //Fetch the box coordinates from the GameObject
        BoxCollider box = GetComponent<BoxCollider>();
        top = box.bounds.max.y;
        bottom = box.bounds.min.y;
    }
        
     void OnCollisionEnter(Collision coll)
     {
         if (coll.gameObject.tag == "Player")
         {
             canClimb = true;
             climber = coll.gameObject;
             ctrl = climber.GetComponent<CharacterControl>();
         }
     }
 
     void OnCollisionExit(Collision coll2)
     {
         if (coll2.gameObject.tag == "Player")
         {
             canClimb = false;
             climber = null;
         }
     }
     void Update()
     {
         if (canClimb)
         {
             if (Input.GetAxis("Vertical")>0)
             {
                 climber.transform.Translate(Vector2.up * Time.deltaTime * speed);
             }
             if (Input.GetAxis("Vertical")<0)
             {
                 climber.transform.Translate(Vector2.down * Time.deltaTime * speed);
             }
         }
     }
     // function called by controller script when controller is attached to ladder
    // delta is the forward/backward movement
    /*
    void Move (float delta) 
    {
        // we calc vertical position using current vertical position  + delta movement corrected for time interval
        var vertPos : float = ctrl.transform.position.y + delta * Time.deltaTime;
        ctrl.transform.rotation = transform.rotation;
        // Debug.Log("Move: " + vertPos); // debug usefull to determine top and bottom values
        if ((vertPos > top) || (vertPos < bottom)) 
        {
            // we're at top or bottom and moving away from ladder, use walk animation
            ctrl.SendMessage("SetCurrentSpeed", delta, SendMessageOptions.DontRequireReceiver);
            // set climb speed 0 to stop climb animation
            ctrl.SendMessage("SetCurrentClimbSpeed", 0, SendMessageOptions.DontRequireReceiver);
            // move forwards/backwards, orientation determined by lader orientation
            ctrl.Move(transform.rotation * new Vector3(0.0, 0.0, delta) * Time.deltaTime);        
        }
        else 
        {
            // no forward movement as we're climbing
            ctrl.SendMessage("SetCurrentSpeed", 0, SendMessageOptions.DontRequireReceiver);
            // set climb animation
            ctrl.SendMessage("SetCurrentClimbSpeed", delta, SendMessageOptions.DontRequireReceiver);
            Vector2 pos = transform.position; // get position of ladder, use it for x and z pos of the character
            pos.y = vertPos; // the y pos (vertical) is determined by calculated vertical position
            ctrl.transform.position = pos;
        }
    }*/


 }