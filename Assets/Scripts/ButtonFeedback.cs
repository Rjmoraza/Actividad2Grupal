using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonFeedback : MonoBehaviour
{
    public void ChangeTextToFeedbackUser(GameObject button)
    {
        // get text mesh pro from child of button
        button.GetComponentInChildren<TMP_Text>().color = Color.yellow;
        button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void ChangeTextToNormal(GameObject button)
    {
        button.GetComponentInChildren<TMP_Text>().color = Color.white;
        button.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
