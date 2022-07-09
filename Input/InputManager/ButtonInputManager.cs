using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Repository for ButtonInputs
// Needs to be object in scene so ButtonInputs can use Update
public class ButtonInputManager : MonoBehaviour
{
    static Dictionary<string, ButtonInput> buttonInputs = new Dictionary<string, ButtonInput>();


    // Needed so each input can poll their specific key and check its status
    void Update()
    {
        foreach(ButtonInput input in buttonInputs.Values)
        {
            input.Update();
        }
    }


    // Returns true if added successfully, false if key already present
    public static bool AddButtonInput(string label, ButtonInput newInput)
    {
        label = label.ToUpper();

        if(buttonInputs.ContainsKey(label)) return false;

        buttonInputs.Add(label, newInput);

        return true;
    }


    public static bool AddButtonInput(string label, KeyCode key, ButtonInput.ButtonNotification justPressedSubscriber = null,
        ButtonInput.ButtonNotification justReleasedSubscriber = null, ButtonInput.ButtonNotification heldSubscriber = null)
    {
        ButtonInput newButtonInput = new ButtonInput(key);

        if(justPressedSubscriber != null)
        {
            newButtonInput.subscribeJustPressed(justPressedSubscriber);
        }

        if(justReleasedSubscriber != null)
        {
            newButtonInput.subscribeJustReleased(justReleasedSubscriber);
        }

        if(heldSubscriber != null)
        {
            newButtonInput.subscribeHeld(heldSubscriber);
        }

        return AddButtonInput(label, newButtonInput);
    }


    public static void RemoveButtonInput(string label)
    {
        label = label.ToUpper();

        if(buttonInputs.ContainsKey(label)) 
        {
            buttonInputs.Remove(label);
        }
    }


    public static ButtonInput GetButtonInput(string label)
    {
        label = label.ToUpper();

        if(buttonInputs.ContainsKey(label))
        {
            return buttonInputs[label];
        }
        else
        {
            return null;
        }
    }


    public static void ClearButtonInputs() { buttonInputs.Clear(); }
}
