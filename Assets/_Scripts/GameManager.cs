using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject textObject, winObject;
    

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayerWins()
    {
        textObject.SetActive(true);
        Destroy(winObject);
    }

    private void OnEnable()
    {
        WinTrigger.OnGameStateChanged += PlayerWins;
    }

    private void OnDisable()
    {
        WinTrigger.OnGameStateChanged -= PlayerWins;
    }
}
