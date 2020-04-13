using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour
{
    [HideInInspector]
    public int NONE = 0, YES = 1, CANCEL = 2;
    [HideInInspector]
    public int result = 0;

    private void Start()
    {
        Button yesBtn = transform.GetChild(1).gameObject.GetComponent<Button>();
        yesBtn.onClick.AddListener(OnYesBtnPressed);

        Button cancelBtn = transform.GetChild(2).gameObject.GetComponent<Button>();
        cancelBtn.onClick.AddListener(OnCancelBtnPressed);
    }

    public void OnYesBtnPressed()
    {
        result = YES;
        //if I debug here it prints
    }

    public void OnCancelBtnPressed()
    {
        result = CANCEL;
    }
}
