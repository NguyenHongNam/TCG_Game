using UnityEngine;
using Mirror;
using CameraShake;
public class Combat : NetworkBehaviour
{
    public BounceShake.Params shakeParams;
    [Header("Entity")]
    public Entity entity;

    [Command(ignoreAuthority = true)]
    public void CmdChangeMana(int amount)
    {
        // Tang giam mana
        if (entity is Player) entity.GetComponent<Player>().mana += amount;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeStrength(int amount)
    {
        // Tang giam suc manh
        entity.strength += amount;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeHealth(int amount)
    {
        // Tang giam mau(ca cua nguoi choi luon)
        entity.health += amount;
        if (entity is Player && amount < 0)
        {
            Vector3 sourcePosition = transform.position;
            CameraShaker.Shake(new BounceShake(shakeParams, sourcePosition));
        }

        if (entity.health <= 0)
        {
            if (entity is Player)
            {
                GameOverPopup.Instance.Show();
            }
            else
            {
                Destroy(entity.gameObject);
            }
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdIncreaseWaitTurn()
    {
        entity.waitTurn++;
    }
}
