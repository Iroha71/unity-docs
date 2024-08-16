using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IOverlapableUI
{
    UnityAction OnRendered { get; set; }
    void Close();
    void Render();
}

public interface ISelectableUI
{
    void SelectFirstContent();
}