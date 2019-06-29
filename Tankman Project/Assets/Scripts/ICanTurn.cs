using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanTurn
{
    float TurnSpeed { get; set; }

    void TurnForValue(float turnSpeed, float inputValue);
}
