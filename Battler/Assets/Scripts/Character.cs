using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class Character : MonoBehaviour
{
    private WeaponInfo Weapon { get { return characterInfo.WeaponInfo; } }

    public CharacterInfo characterInfo;

    public int Hp; // { get; private set; }
    public int BulletCount; // { get; private set; }
    public bool IsHide; // { get; private set; }
    public bool Reloading; // { get; private set; }
    public bool HaveBullets { get { return BulletCount > 0; } }
    public bool Alive; // { get; private set; }
    public bool Attacking;
    public Slider HealthBar;
    public Button reloadButton;

    public Animator animator;


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
        
        reloadButton.onClick.AddListener(UpdateBullet);
        reloadButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPosition()
    {
        IsHide = !IsHide;
        animator.SetBool("IsHide", IsHide);
    }

    public void UpdateBullet()
    {
        BulletCount = Weapon.BulletCount;
        reloadButton.enabled = false;
    }

    public void Attack(Character target)
    {
        BulletCount--;
        if(BulletCount <= 0) {
            reloadButton.enabled = true;
        }
        Reloading = true;
        StartCoroutine(AttackAnimation());
        StartCoroutine(ReloadWeapon(Weapon.CooldownTime));
        if(target != null)
            target.GetDamage(Weapon.Attack);

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

    public void GetDamage(int damage)
    {
        Hp -= damage;
        HealthBar.value = Mathf.Max(0, Hp);
        if(Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Alive = false;
        animator.SetTrigger("Die");
    }
}
