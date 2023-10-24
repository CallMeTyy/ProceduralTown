using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewSwitch : MonoBehaviour
{
    [SerializeField] private GameObject fps;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject[] menuItems;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleView();
        }
    }

    public void ToggleView()
    {
        if (fps.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            fps.SetActive(false);
            top.SetActive(true);
            foreach (GameObject g in menuItems)
            {
                g.SetActive(true);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            fps.SetActive(true);
            top.SetActive(false);
            foreach (GameObject g in menuItems)
            {
                g.SetActive(false);
            }
        }
    }
}
