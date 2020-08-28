using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel_barrow : MonoBehaviour
{
    public GameObject player;//keep track of player
    public GameObject child;
    CapsuleCollider grabCollider ;//range of wheel barrow\
    Player pl;

    public float distanceFromPlayer;
    public float distanceFromGround;
    public float playerFromGround ;
    
    Vector3 grabbedPos;
    Vector3 playerPosOffset;
    float grabDistance;
    float seperation;
    bool canGrab;
    bool isGrabbed;



    //from wobble
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;
    Vector3 angularVelocity;
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;

    void Start()
    {

        grabDistance = 3;
        distanceFromPlayer = 4;
        playerPosOffset = new Vector3(0, playerFromGround, 0);
        
        canGrab = false;
        isGrabbed = false;
        grabbedPos = new Vector3(0, playerFromGround, 0);
        
        pl = player.GetComponent<Player>();
        
    }

    private void Update()
    {
       
      

        seperation = Vector3.Distance(this.transform.position, (player.transform.position + playerPosOffset));
        //Debug.Log(seperation);

        checkGrab();
        if (canGrab) {//allow an input by the player
            if (Input.GetKeyDown("e") && !isGrabbed)
            {
                isGrabbed = true;
                
            }
            else if(Input.GetKeyDown("e") && isGrabbed) {
                isGrabbed = false;
                transform.SetParent(null);
                
            }
        }

        if (isGrabbed) {
           
            transform.SetParent(player.transform);
            //this.transform.position =(player.transform.position + grabbedPos) ;
            this.transform.position = (player.transform.position + grabbedPos);
            this.transform.rotation = player.transform.rotation;
            wobble();
            
        }

    }

    private void checkGrab()
    {
   
        if (seperation < grabDistance)
        {  
            canGrab = true;
           
            
        }
        else
        {
            canGrab = false;
           
            
        }
    }

    private void wobble() {

        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        //child.transform.SetParent(this.transform);
        child.transform.localPosition = new Vector3(0, wobbleAmountX, distanceFromPlayer);
        child.transform.localRotation = Quaternion.Euler(0, 0, wobbleAmountZ*13);

       
        Debug.Log("wobbleZ " + wobbleAmountZ);

        // velocity
        velocity = (lastPos - player.transform.position) / Time.deltaTime;
        angularVelocity = player.transform.rotation.eulerAngles - lastRot;


        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        lastPos = player.transform.position;
        lastRot = player.transform.rotation.eulerAngles;

        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, grabDistance);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.transform.position, 1);
        Gizmos.DrawWireSphere(player.transform.position + playerPosOffset, 1);

    }
    //player can grab

    //when the wheelbarrow is grabbed it uses planting position 
    //slows player movement
    //uses tank controls

    //wheel barrow has it's own inventory space


}
