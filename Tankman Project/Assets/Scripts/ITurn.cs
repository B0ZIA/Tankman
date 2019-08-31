using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurn
{
    float TurnSpeed { get; set; }

    void Turn(float turnSpeed, float inputValue);
}
