using UnityEngine;
using UnityEngine.UI;




//일단 상점 구현 할 때 써야할 필요 기물 정보



public class Pieces : Article
{
    public Sprite pieceImg;
    public new string name;
    public int id;
    public int gold;
    public string spieces;
    public string classes;
    public int grade;

    public Transform pos;
    public Item[] items;

    public Canvas canvas;
    public HpBar hpbarScript;

    public int t_objectsNum;
    public ItemsImg itemImage;

    BoxCollider boxCollider;

    public PiecesCountManager piecesCountManager;
    public Sprite[] pieceGradeImgs;
    public void Parse(Piece piece)
    {
        pieceImg = piece.pieceImg;
        id = piece.id;
        gold = piece.gold;
        name = piece.name;
        grade = piece.grade;

        hp = piece.hp;
        mp = piece.mp;
        originMaxHp = piece.maxHp;
        originMaxMp = piece.maxMp;
      
        originAttackDamage = piece.attackDamage;
        originAttackSpeed = piece.attackSpeed;
        originAttackRange = piece.attackRange;
        originMoveSpeed = piece.moveSpeed;

        spieces = piece.spieces;
        classes = piece.classes;
    }
    private void Awake()
    {
        pos = GetComponent<Transform>();
        items = new Item[3];

        for(int i = 0; i < items.Length; i++)
        {
            items[i] = new Item(string.Empty,0,0,0,0,0);
        }


        canvas = GetComponentInChildren<Canvas>();

        boxCollider = GetComponent<BoxCollider>();

        pieceGradeImgs = FindObjectOfType<PiecesCountManager>().GetComponent<PiecesCountManager>().piecesGradeImg;

        
    }
    public void EquipItem(ItemObject item)
    {
        for (int i=0; i < items.Length;i++)
        {
            if (items[i].itemName == string.Empty || items[i].itemName == null)
            {
                items[i] = item._item;
                buffData[0].maxHp += item._item.itemHp;
                hp = Mathf.Clamp(hp + item._item.itemHp, 0, maxHp);
                buffData[0].attackDamage += item._item.itemAttackDamage;
                buffData[0].attackSpeed += item._item.itemAttackSpeed;
                mp = Mathf.Clamp(mp + item._item.itemMp, 0, maxMp);
                for (int j = 0; j < items.Length; j++)
                {
                    if (i == j)
                    {
                        GameObject itemImg = canvas.gameObject.transform.GetChild(i).gameObject;
                        itemImg.SetActive(true);
                        itemImg.GetComponent<Image>().sprite = item._item.itemSprite;
                        return;
                    }
                }
                return;
            }
        }
    }


    public void GivingItemInfo()
    {
        for(int i=0;i < items.Length; i++)
        {
            if (items[i].itemName != string.Empty)
            {
                UIManager.instance.AddTheItem(items[i]);
            }
        } 
    }


    public void OnBoxCollider()
    {
        boxCollider.enabled = true;
    }


    public void SellPiece()
    {
        DataManager.instance.GetGold(CalculateGold());
        UIManager.instance.UIRefresh();
        GivingItemInfo();
        foreach (var tile in InvSpawnManager.instance.hexaTiles)
        {
            if (tile.piece == this)
                tile.piece = null;
        }
        foreach (var tile in InvSpawnManager.instance.invTiles)
        {
            if (tile.piece == this)
                tile.piece = null;
        }
        DestroyImmediate(gameObject);
    }

    public int CalculateGold()
    {
        if(grade == 1)
        {
            return gold = gold;
        }
        else 
        {
            return gold = gold+2*(grade-1);
        }
    }


    public void MergePeice()
    {
        grade++;
        float newsize = 1f;



        if (grade == 2)
        {
            newsize = 1.2f;
            buffData[1].maxHp = originMaxHp;
            hp = maxHp;
            buffData[1].attackDamage = originAttackDamage;
            mp = maxMp;

            canvas.gameObject.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = pieceGradeImgs[1];
            this.transform.localScale = new Vector3 (newsize, newsize, newsize);


            //for (int i = 0; i < items.Length; i++)
            //{
            //    if (items[i].itemName != string.Empty || items[i].itemName != null)
            //    {
            //        buffData[1].maxHp += items[i].itemHp;
            //        buffData[1].hp += items[i].itemHp;
            //        buffData[1].attackDamage += items[i].itemAttackDamage;
            //        buffData[1].attackSpeed += items[i].itemAttackSpeed;
            //        buffData[1].mp += items[i].itemMp;
            //    }
            //}
        }
        else if (grade == 3)
        {
            newsize = 1.4f;
            buffData[1].maxHp = 2 * originMaxHp;
            hp = maxHp;
            buffData[1].attackDamage = 2 * originAttackDamage;
            mp = maxMp;

            canvas.gameObject.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = pieceGradeImgs[2];
            this.transform.localScale = new Vector3(newsize, newsize, newsize);


            //for (int i = 0; i < items.Length; i++)
            //{
            //    if (items[i].itemName != string.Empty && items[i].itemName != null)
            //    {
            //        maxHp += items[i].itemHp;
            //        hp += items[i].itemHp;
            //        attackDamage += items[i].itemAttackDamage;
            //        attackSpeed += items[i].itemAttackSpeed;
            //        mp += items[i].itemMp;
            //    }
            //}

        }
    }
}
