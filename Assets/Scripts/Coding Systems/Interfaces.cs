using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactive
{
    void TurnOn();
    void TurnOff();
}

public interface IInteractable
{
    void Interact();
}

public interface ITakeLight
{
    void LightHit(Ray ray = new Ray());
}