using Fusion;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour,IPlayerLeft
{
    public
    TextMeshProUGUI nameText;
    //public
    //TextMeshProUGUI queText;
    public static Player Local { get; private set; }

    [Networked(OnChanged = nameof(OnNicknamehanged))]
    public NetworkString<_16> playerNickname { get; set; }

    private static void OnNicknamehanged(Changed<Player> changed)
    {
        changed.Behaviour.OnNicknameChanged();
    }
    void OnNicknameChanged()
    {
        nameText.text = playerNickname.ToString();
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
    public override void FixedUpdateNetwork()
    {
            nameText.transform.forward = Camera.main.transform.forward;
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            if(PlayerPrefs.HasKey("PlayerName"))
            {
                RPC_SetNickname(PlayerPrefs.GetString("PlayerName"));
            }
            
        }
    }
    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    void RPC_SetNickname(string nickname,RpcInfo info = default)
    {
        playerNickname = nickname;
    }
  
}
