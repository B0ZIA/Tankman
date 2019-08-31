using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    float MoveSpeed { get; set; }

    void Move(float turnSpeed, float inputValue);
}
