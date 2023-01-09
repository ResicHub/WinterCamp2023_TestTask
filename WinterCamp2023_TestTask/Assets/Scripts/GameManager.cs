using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SaveLoadManager saveLoadManager;
    private UIManager uiManager;
    private RockLineGenerator rockLineGenerator;
    private BezierGenerator bezierGenerator;

    private void Awake()
    {
        saveLoadManager = GetComponent<SaveLoadManager>();
        uiManager = GetComponent<UIManager>();
        rockLineGenerator = GetComponent<RockLineGenerator>();
        bezierGenerator = GetComponent<BezierGenerator>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
