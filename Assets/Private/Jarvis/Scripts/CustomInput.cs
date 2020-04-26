using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput
{
    int playerID;
    CharacterController currentController;
    float horizontal;
    float vertical;
    bool attackButton;
    bool jumpButton;
    bool blockbutton;

    public CustomInput(int playerID)
    {
        this.playerID = playerID;
    }

    public float GetAxis(string Axis)
    {
        if (Axis == "Horizontal")
        {
            return Input.GetAxis("Horizontal" + playerID.ToString());
        }
        else if (Axis == "Vertical")
        {
            return Input.GetAxis("Vertical" + playerID.ToString());
        }
        else
        {
            //Debug.LogError("Axis Name not Recognized");
            return 0;
        }

    }
    public float GetAxisRaw(string axisName)
    {
        if (axisName.Contains("Horizontal"))
        {
            return Input.GetAxisRaw(axisName + playerID.ToString());
        }
        else if (axisName.Contains("Vertical"))
        {
            return Input.GetAxisRaw(axisName + playerID.ToString());
        }
        else
        {
            //Debug.LogError("Axis Name not Recognized");
            return 0;
        }

    }
    public bool GetButton(string buttonName)
    {
        if (buttonName.Contains("Attack"))
        {
            return Input.GetButton(buttonName + playerID);
        }
        else if (buttonName.Contains("Jump"))
        {
            return Input.GetButton(buttonName + playerID);
        }
        else if (buttonName.Contains("Block"))
        {
            return Input.GetButton(buttonName + playerID);
        }
        else if (buttonName.Contains("SpecialAttack"))
        {
            return Input.GetButton(buttonName + playerID);
        }
        else
        {
            //Debug.LogError("Button Name not Recognized");
            return false;
        }

    }
    public bool GetButtonDown(string buttonName)
    {
        if (buttonName.Contains("Attack"))
        {
            return Input.GetButtonDown(buttonName + playerID);
        }
        else if (buttonName.Contains("Jump"))
        {
            //Debug.Log(buttonName + playerID);
            return Input.GetButtonDown(buttonName + playerID);
        }
        else if (buttonName.Contains("Block"))
        {
            return Input.GetButtonDown(buttonName + playerID);
        }
        else if (buttonName.Contains("SpecialAttack"))
        {
            return Input.GetButtonDown(buttonName + playerID);
        }
        else
        {
            //Debug.LogError("Button Name not Recognized");
            return false;
        }
    }
    public bool GetKeyDown(int controllerKey)
    {
        ////Debug.Log((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + playerID + "Button" + controllerKey));        
        return Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode),"Joystick"+ playerID+"Button"+controllerKey));
        
    }
    public bool GetKey(int controllerKey)
    {
        ////Debug.Log((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + playerID + "Button" + controllerKey));        
        return Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + playerID + "Button" + controllerKey));

    }

    public void ChangeController(int newPlayerID, CharacterController controller)
    {
        if (playerID != newPlayerID)
        {
            switch (newPlayerID)
            {
                case 1:
                    GameManager.Instance.playerOne = null;                    
                    break;
                case 2:
                    GameManager.Instance.playerTwo = null;
                    break;
            }
            this.playerID = newPlayerID;
            controller.isopponentHitboxesSet = false;
        }
        if (controller.isopponentHitboxesSet == false)
        {
            if (GameManager.Instance.playerOne == null || GameManager.Instance.playerTwo == null)
            {
                switch (newPlayerID)
                {
                    case 1:

                        GameManager.Instance.playerOne = controller;
                        ////Debug.Log(1.ToString() + GameManager.Instance.playerOne.name);
                        break;
                    case 2:

                        GameManager.Instance.playerTwo = controller;


                        break;
                }
                //Debug.Log("isopponentHitboxesSet " + controller.isopponentHitboxesSet.ToString());                
                return;
            }
            


            if (controller.opponentController == null)
            {
                switch (newPlayerID)
                {
                    case 1:
                        //Debug.Log(playerID.ToString() + " GameManager.Instance.playerTwo " + (GameManager.Instance.playerTwo == null).ToString());
                        if (GameManager.Instance.playerTwo != null) controller.opponentController = GameManager.Instance.playerTwo;
                        break;
                    case 2:
                        //Debug.Log(playerID.ToString() + " GameManager.Instance.playerOne " + (GameManager.Instance.playerOne == null).ToString());
                        if (GameManager.Instance.playerOne != null) controller.opponentController = GameManager.Instance.playerOne;
                        break;
                }
                //Debug.Log(playerID.ToString() + " opponentController " + (controller.opponentController == null).ToString());
                return;
            }
            //Debug.Log("outside check opponentHitboxes " + (controller.opponentHitboxes == null).ToString());
            if (controller.opponentHitboxes == null)
            {
                //Debug.Log("inside check opponentHitboxes " + (controller.opponentHitboxes == null).ToString());
                controller.opponentHitboxes = controller.opponentController.playerHitboxes;
                //Debug.Log("After assignment opponentHitboxes " + (controller.opponentHitboxes == null).ToString());
                return;
            }
            if (controller.opponentHitboxes != null)
            {
                //Debug.Log("opponentHitboxes not equal to null " + (controller.opponentHitboxes != null).ToString());
                foreach (Collider opponentHitbox in controller.opponentHitboxes)
                {
                    if (opponentHitbox.gameObject.CompareTag("Hitbox"))
                    {
                        Physics.IgnoreCollision(opponentHitbox, controller.playerCollisionBox);  // Prevent Opponent Hitboxes from Colliding with playerCollisionBox
                    }
                }

                foreach (Collider playerHitbox in controller.playerHitboxes)
                {
                    if (playerHitbox.gameObject.CompareTag("Hitbox"))
                    {
                        foreach (Collider opponentHitbox in controller.opponentHitboxes)
                        {
                            if (opponentHitbox.gameObject.CompareTag("Hitbox"))
                            {

                                Physics.IgnoreCollision(playerHitbox, opponentHitbox);// Prevent Player Hitboxes from Colliding with Opponent Hitboxes
                            }
                        }
                    }

                }

            }

            if (controller.opponentHitboxes != null) controller.isopponentHitboxesSet = true;
            return;
        }
        else
        {
            //Debug.Log(playerID.ToString() + "Completed");
        }
    }
    
}
