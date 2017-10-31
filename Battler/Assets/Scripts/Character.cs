using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;




public class Character : MonoBehaviour
{
    private System.Collections.Generic.List<Character> enemyTeam;
    private int position;
    private int bulletCount; // { get; private set; }
    private WeaponInfo Weapon { get { return characterInfo.WeaponInfo; } }

    public CharacterInfo characterInfo;

    public float Hp; // { get; private set; }
    public bool IsHide; // { get; private set; }
    public bool Reloading; // { get; private set; }
    public bool HaveBullets { get { return BulletCount > 0; } }
    public bool Alive; // { get; private set; }
    public bool Attacking;
    public bool Selected;
    public GameObject BulletPrefab;
    public Transform BulletStartPosition;
    public Slider HealthBar;
    public Button reloadButton;
    public Text reloadBtnText;
    public Text selectBtnText;
    public Character Target;

    public Animator animator;

    public int BulletCount
    {
        get { return bulletCount; }
        set
        {
            bulletCount = value;
            reloadBtnText.text = bulletCount.ToString();
            if (BulletCount <= 0) {
                reloadButton.enabled = true;
                reloadBtnText.text = "RELOAD";
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        Hp = characterInfo.MaxHp;
        HealthBar.maxValue = Hp;
        HealthBar.value = Hp;
        BulletCount = Weapon.BulletCount;
        Reloading = false;
        Alive = true;
        Attacking = false;

        reloadBtnText.text = BulletCount.ToString();
        reloadButton.onClick.AddListener(UpdateBullet);
        reloadButton.enabled = false;
    }

    public IEnumerator Battle(Slot[] enemies, int pos)
    {
        enemyTeam = enemies.Select(x => x.character).ToList();
        position = pos;
        if (!IsHide) {
            SetPosition();
        }

        while (true) {
            if (!Alive)
                yield break;

            if (!IsHide && !Reloading && HaveBullets) {
                if (Target != null && Target.Alive && !Target.IsHide) {
                    Attack(Target, enemyTeam.FindIndex(x => x == Target) + position);
                } else {
                    bool shot = false;
                    int maxDist = Weapon.Ditance;
                    for (int i = 0; i < 3; i++) {
                        if (!enemyTeam[i].IsHide && enemyTeam[i].Alive) {
                            Attack(enemyTeam[i], position + i);
                            shot = true;
                            break;
                        }
                    }
                    if (!shot)
                        Attack(null,position + 3);
                }
            }

            yield return null;
        }
    }

    public void SetPosition()
    {
        //IsHide = !IsHide;
        animator.SetBool("IsHide", !IsHide);
    }

    public void SetSelectState(bool isSelect)
    {
        Selected = isSelect;
        if (Selected)
            selectBtnText.text = "Unselect";
        else
            selectBtnText.text = "Select";
    }

    public void UpdateBullet()
    {
        BulletCount = Weapon.BulletCount;
        reloadButton.enabled = false;
    }

    public void Attack(Character target, int dist)
    {
        float shotTime = SendBullet(dist);
        BulletCount--;
        Reloading = true;
        StartCoroutine(AttackAnimation());
        StartCoroutine(ReloadWeapon(Weapon.CooldownTime));
        if (target != null) {
            float damage = Weapon.Attack;
            int distOver = Mathf.Max(0, dist - Weapon.Ditance + 1);
            if (distOver > 0) {
                damage = Mathf.Max(0, distOver * Weapon.DamageReduce * damage);
            }
            StartCoroutine(DelayDamage(target, shotTime, damage));
        }
    }

    public IEnumerator DelayDamage(Character target, float delay, float damage)
    {
        yield return new WaitForSeconds(delay);
        target.GetDamage(damage);
    }

    public float SendBullet(int dist)
    {
        var bullet = Instantiate(BulletPrefab, BulletStartPosition);
        var bulletScript = bullet.AddComponent<Bullet>();
        bulletScript.Distance = dist;
        bulletScript.Send();
        return bulletScript.ShotTime;
    }

    private IEnumerator AttackAnimation()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Attack", false);
    }

    private IEnumerator ReloadWeapon(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        Reloading = false;
    }

    public void GetDamage(float damage)
    {
        Hp -= damage;
        HealthBar.value = Mathf.Max(0, Hp);
        if (Hp <= 0) {
            Die();
        }
    }

    public void Die()
    {
        Alive = false;
        animator.SetTrigger("Die");
    }
}
