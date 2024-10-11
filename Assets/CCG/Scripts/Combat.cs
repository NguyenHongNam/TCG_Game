using UnityEngine;
using Mirror;

public class Combat : NetworkBehaviour
{
    [Header("Entity")]
    public Entity entity;

    public DamagePopup damagePopup;

    [Command(ignoreAuthority = true)]
    public void CmdChangeMana(int amount)
    {
        if (entity is Player) entity.GetComponent<Player>().mana += amount;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeStrength(int amount)
    {
        entity.strength += amount;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeHealth(int amount)
    {
        entity.health += amount;
        if (entity is Player)
        {
            RpcShowDamagePopup(-amount);  // Gửi đến các máy khách để hiển thị damage popup
        }
        if (entity.health <= 0)
        {
            Destroy(entity.gameObject);
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdIncreaseWaitTurn()
    {
        entity.waitTurn++;
    }

    public void Update()
    {
        if (entity is Player)
        {
            if (entity.health <= 0)
            {
                // Nếu người chơi bị chết, hiển thị popup thua cho người chơi và popup thắng cho đối thủ
                GameEndPopup.Instance.LoosingPopup(); // Hiện popup thua

                // Tìm đối thủ (người chơi còn lại)
                foreach (Player player in FindObjectsOfType<Player>())
                {
                    if (player != entity)
                    {
                        // Hiển thị popup thắng cho đối thủ
                        GameEndPopup.Instance.WinningPopup();
                    }
                }
            }
        }
        else
        {
            // Nếu là lá bài, phá hủy lá bài khi máu <= 0
            if (entity.health <= 0)
            {
                Destroy(entity.gameObject);
            }
        }
    }

    [ClientRpc]
    void RpcShowDamagePopup(int damageAmount)
    {
        if (damagePopup != null)
        {
            damagePopup.ShowDamage(damageAmount);
        }
    }
}
