using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveSettings : MonoBehaviour
{
    public GameObject settings;

  public void setingView()
    {
        settings.SetActive(true);
    }
    public void setingFalse()
    {
        settings.SetActive(false);
    }
}
