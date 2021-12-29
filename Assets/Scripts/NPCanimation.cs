using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCanimation : MonoBehaviour
{
    private Animator m_Animations;

    public int DanceVariation;
    // Start is called before the first frame update
    void Start()
    {
        m_Animations = GetComponent<Animator>();

        m_Animations.SetInteger("Dancing", DanceVariation);
    }


}
