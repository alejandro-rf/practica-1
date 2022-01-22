using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject WinObject;
    public static Action OnGameStateChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hola");

        var player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            OnGameStateChanged?.Invoke();
        }
    }

    private void ShowWin()
    {
        WinObject.SetActive(true);
    }
}
