using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Attackable
{
    public abstract void HandleCollision(Collider c);

    public abstract void Die();

    public void OutOfBoundsCheck() {
      if (transform.position.y <= -50) {
        Die();
      }
    }

    public override void ProcessAttack() {
      Die();
    }
}
