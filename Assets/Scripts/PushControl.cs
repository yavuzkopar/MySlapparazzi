using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushControl : NetworkBehaviour
{
    [Networked(OnChanged = nameof(Pushed))]
    public int isPushing { get; set; }

    Rigidbody _rigidbody;
    public override void Spawned()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    static void Pushed(Changed<PushControl> changed)
    {
        int newValue = changed.Behaviour.isPushing;
        changed.LoadOld();
        int oldValue = changed.Behaviour.isPushing;

        if (newValue > oldValue)
            changed.Behaviour.RemotePush();

    }
    Vector3 pusingdir;
    public void Push(Vector3 dir)
    {
        isPushing += 1;
        pusingdir = dir;
    }
    void RemotePush()
    {
        _rigidbody.velocity = pusingdir;
    }
}
