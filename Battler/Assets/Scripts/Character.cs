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
    public GameObject DamageShowPrefab;
    public Transform BulletStartPosition;
    public Transform DamagePosition;
    public Slider HealthBar;
    public Text HealthBarText;
    public Button reloadButton;
    public Text HideBtnText;
    public Text reloadBtnText;
    public Text selectBtnText;
    public Image nobulletImage;
    public Character Target;
    public Material material;

    public Animator animator;

    public int BulletCount
    {
        get { return bulletCount; }
        set
        {
            bulletCount = value;
            reloadBtnText.text = bulletCount.ToString();
            if (BulletCount <= 0)
            {
                reloadButton.enabled = true;
                reloadBtnText.text = "RELOAD";
                nobulletImage.gameObject.SetActive(true);
                nobulletImage.fillAmount = 1;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        Hp = characterInfo.MaxHp;
        HealthBar.maxValue = Hp;
        HealthBar.value = Hp;
        HealthBarText.text = string.Format("{0}/{1}", Hp, characterInfo.MaxHp);
        BulletCount = Weapon.BulletCount;
        Reloading = false;
        Alive = true;
        Attacking = false;

        reloadBtnText.text = BulletCount.ToString();
        reloadButton.onClick.AddListener(UpdateBullet);
        reloadButton.enabled = false;
        nobulletImage.gameObject.SetActive(false);

        material = GetComponentInChildren<Renderer>().material;
    }

    public IEnumerator Battle(Slot[] enemies, int pos)
    {
        enemyTeam = enemies.Select(x => x.character).ToList();
        position = pos;
        if (!IsHide)
        {
            SetPosition();
        }

        while (true)
        {
            if (!Alive)
                yield break;

            if (!IsHide && !Reloading && HaveBullets)
            {
                if (Target != null && Target.Alive && !Target.IsHide)
                {
                    Attack(Target, enemyTeam.FindIndex(x => x == Target) + position);
                }
                else
                {
                    bool shot = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (!enemyTeam[i].IsHide && enemyTeam[i].Alive)
                        {
                            Attack(enemyTeam[i], position + i);
                            shot = true;
                            break;
                        }
                    }
                    if (!shot)
                        Attack(null, position + 3);
                }
            }

            yield return null;
        }
    }

    public void SetPosition()
    {
        //IsHide = !IsHide;
        if (IsHide)
            HideBtnText.text = "Hide";
        else
            HideBtnText.text = "Get Up";
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
        StartCoroutine(ReloadBullet(Weapon.CooldownTime));
        reloadButton.enabled = false;

    }

    public void Attack(Character target, int dist)
    {
        float shotTime = SendBullet(dist);
        BulletCount--;
        Reloading = true;
        StartCoroutine(AttackAnimation());
        StartCoroutine(ReloadWeapon(Weapon.CooldownTime));
        if (target != null)
        {            
            int distOver = Mathf.Max(0, dist - Weapon.Ditance + 1);
            float damage = Mathf.Max(0, Mathf.Pow(Weapon.DamageReduce, distOver) * Weapon.Attack);
            float accureny = Mathf.Max(0, Mathf.Pow(Weapon.AccuracyReduce, distOver) * Weapon.Accuracy);
            if (Random.value > accureny)
                damage = 0;
            StartCoroutine(DelayDamage(target, shotTime, damage));
        }
    }

    public IEnumerator DelayDamage(Character target, float delay, float damage)
    {
        yield return new WaitForSeconds(delay);
        target.GetDamage(damage, this);
    }

    public float SendBullet(int dist)
    {
        var bullet = Instantiate(BulletPrefab, BulletStartPosition);
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Send(dist, material);
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
        float time = cooldown;
        //cooldownImage.gameObject.SetActive(true);
        while (time > 0)
        {
            time -= Time.deltaTime;
            //cooldownImage.fillAmount = time / cooldown;
            yield return null;
        }
        //cooldownImage.gameObject.SetActive(false);
        Reloading = false;
    }

    private IEnumerator ReloadBullet(float cooldown)
    {
        float time = cooldown;
        nobulletImage.gameObject.SetActive(true);
        while (time > 0)
        {
            time -= Time.deltaTime;
            nobulletImage.fillAmount = time / cooldown;
            yield return null;
        }
        nobulletImage.gameObject.SetActive(false);
        BulletCount = Weapon.BulletCount;
    }

    public void GetDamage(float damage, Character from)
    {
        Hp -= damage;
        HealthBar.value = Mathf.Max(0, Hp);
        HealthBarText.text = string.Format("{0}/{1}", Mathf.Max(0, Hp), characterInfo.MaxHp);
        var damageScript = Instantiate(DamageShowPrefab, DamagePosition).GetComponent<DamageShow>();
        damageScript.SetUp(damage, from.material);
        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Alive = false;
        nobulletImage.gameObject.SetActive(false);
        animator.SetTrigger("Die");
    }
}
