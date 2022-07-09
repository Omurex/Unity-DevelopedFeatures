using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to help handle button input, designers shouldn't need to worry about implementation too much
public class ButtonInput
{
    // Pass buttonCode to subscribed methods in case they have multiple
    // input notifications tied to that function and need to differentiate
    public delegate void ButtonNotification(KeyCode code);

    // Events that subscriber can subscribe to
    private event ButtonNotification ButtonJustPressed;
    private event ButtonNotification ButtonJustReleased;
    private event ButtonNotification ButtonHeld;

    public KeyCode buttonCode; // Can be keyboard OR mouse button


    public ButtonInput(KeyCode _buttonCode) // Which key / mouse to check and notify subscribers about
    {
        buttonCode = _buttonCode;
    }


    public void Update() // Checks on status of dedicated button and calls events associated with status
    {
        if(Input.GetKeyDown(buttonCode))
        {
            ButtonJustPressed?.Invoke(buttonCode);
        }
        else if(Input.GetKeyUp(buttonCode))
        {
            ButtonJustReleased?.Invoke(buttonCode);
        }

        if(Input.GetKey(buttonCode))
        {
            ButtonHeld?.Invoke(buttonCode);
        }
    }


    #region SubscribeUnsubscribeFunctions

    public void subscribeJustPressed(ButtonNotification subscriberPressMethod)
    {
        ButtonJustPressed += subscriberPressMethod;
    }
    public void removeJustPressed(ButtonNotification subscriberPressMethod)
    {
        ButtonJustPressed -= subscriberPressMethod;
    }


    public void subscribeJustReleased(ButtonNotification subscriberReleaseMethod)
    {
        ButtonJustReleased += subscriberReleaseMethod;
    }
    public void removeJustReleased(ButtonNotification subscriberReleaseMethod)
    {
        ButtonJustReleased -= subscriberReleaseMethod;
    }


    public void subscribeHeld(ButtonNotification subscriberHeldMethod)
    {
        ButtonHeld += subscriberHeldMethod;
    }
    public void removeHeld(ButtonNotification subscriberHeldMethod)
    {
        ButtonHeld -= subscriberHeldMethod;
    }

    #endregion
}
