using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DirectInteractorCustomGrab : MonoBehaviour
{
    private XRDirectInteractor _directInteractor;

    private void Start()
    {
        _directInteractor = GetComponent<XRDirectInteractor>();
    }

    public void Grab()
    {
        if (!_directInteractor.allowSelect)
        {
            return;
        }

        if (_directInteractor.hasSelection)
        {
            return;
        }

        if (_directInteractor.hasHover)
        {
            _directInteractor.StartManualInteraction((IXRSelectInteractable)_directInteractor.interactablesHovered[0]);
        }
    }

    public void Release()
    {
        if (_directInteractor.isPerformingManualInteraction)
        {
            _directInteractor.EndManualInteraction();
        }
    }
}
