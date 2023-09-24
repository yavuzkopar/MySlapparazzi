using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTimer : NetworkBehaviour
{
    [SerializeField] SsTaker ssTaker;
    public override void Spawned()
    {
        Runner.Spawn(ssTaker, Vector3.zero, Quaternion.identity, Object.InputAuthority, (runner, o) =>
        {
            o.GetComponent<SsTaker>().Init();
        });
    }
}
