using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Attackable
{
    public int id;
    public bool isPushed = false;

    public override void ProcessAttack() {
      Push();
    }

    public void Push() {
      isPushed = true;
      GetComponent<Renderer>().material.SetColor("_Color", Color.green);

      GetComponentInParent<ButtonModule>().RegisterPush(id);
    }

    public void UnPush() {
      isPushed = false;

      GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }
}
