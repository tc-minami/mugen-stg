using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    Timer timer;

    void Start()
    {
        timer = this.gameObject.AddComponent<Timer>();
        timer.RunTimer(5.0f, true);
        timer.SetOnCompleteCallback(Sample);

        Unit myUnit = Unit.CreateNewUnit(null, UnitType.Player);
    }

    private void Sample()
    {
        Unit unit = Unit.CreateNewUnit(null, UnitType.Enemy_Normal_SingleShot);
        unit.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
