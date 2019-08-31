using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurn
{
    float TurnSpeed { get; set; }

    void TurnForValue(float turnSpeed, float inputValue);
}
