using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opensafe : MonoBehaviour
{
   public Animator m;

    public void SafeOpen()
    {
        m.SetBool("Isopen", true);
    }
}
