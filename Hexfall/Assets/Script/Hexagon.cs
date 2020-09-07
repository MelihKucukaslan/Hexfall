using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class Hexagon : MonoBehaviour
{
    [SerializeField]
    public Color[] myColor;
    SpriteRenderer sprite;
    public int row, col, id;
    GridManager grid;
    float xOffSet = 0.609f, yOffSet = 0.35f, xPos;

    public bool isDestroy;
    GameObject explosion;
    private ParticleSystem ps;
    public bool isBomb;
    Score score;
    GameObject stepText;

    /////// Altıgene random bir renk atılır ve id değişkeninde tutulur
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        id = Random.Range(0, myColor.Length);
        sprite.color = myColor[id];
    }
    private void Start()
    {
        isDestroy = false;
        grid = GameObject.FindObjectOfType<GridManager>();
        score = FindObjectOfType<Score>();

        ////////konum bilgisi
        xPos = row;
        if (col % 2 == 1)
        {
            xPos += xOffSet;
        }
        xPos += 0.22f * row;


    }

    ////////Obje havuzundan çekilen objenin bilgileri güncellenir ve bombaysa eğer üzerinde sayı texti oluşturulur
    private void OnEnable()
    {

        isDestroy = false;
        xPos = row;
        if (col % 2 == 1)
        {
            xPos += xOffSet;
        }
        xPos += 0.22f * row;
        ChangeColor();

        if (isBomb)
        {
            stepText = new GameObject("myText");
            stepText.transform.parent = GameObject.Find("Canvas").transform;
            stepText.AddComponent<Text>();
            stepText.AddComponent<Bomb>();
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            stepText.GetComponent<Text>().font = ArialFont;
            stepText.GetComponent<Text>().material = ArialFont.material;
            stepText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            stepText.GetComponent<Text>().color = Color.black;
            stepText.GetComponent<Text>().fontSize = 70;

        }

    }
    private void FixedUpdate()
    {

        ///Eğer isDestroy değeri doğru olursa patlatılır
        if (isDestroy)
        {
            if (isBomb)
            {
                Destroy(stepText);
            }
            StartCoroutine(DestroyHexagon());

        }
        //// Eğer pozisyonu matris pozisyonuyla aynı değilse doğru konuma geçer
        if (transform.position != new Vector3(xPos, col * yOffSet, transform.position.z))
        {

            StartCoroutine(ChangePos());
            grid.state = GridManager.GameState.SPAWNING;
        }
        ////// Bombaysa eğer text objesi takip eder
        if (isBomb && stepText.gameObject != null)
        {
            Vector3 counterPos = Camera.main.WorldToScreenPoint(this.transform.position);
            stepText.transform.position = counterPos;


        }
    }

//////// Renk değişme fonksiyonu
    public void ChangeColor()
    {
        id = Random.Range(0, myColor.Length);
        sprite.color = myColor[id];
    }

////////Pozisyonunu günceller
    IEnumerator ChangePos()
    {

        xPos = row;
        if (col % 2 == 1)
        {
            xPos += xOffSet;
        }
        xPos += 0.22f * row;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(xPos, col * yOffSet, 0f), .1f);
        grid.allHex[row, col] = this.gameObject;

        yield return new WaitForSeconds(1f);

    }


//////// Obje kapatılır. 5 puan eklenir ve patlama efekti çağırılır.
    IEnumerator DestroyHexagon()
    {

        grid.state = GridManager.GameState.DESTROY;
        yield return new WaitForSeconds(.7f);

        score.SetScore(score.GetScore() + 5);
        grid.StartCoroutine("DestroyHex");
        grid.allHex[row, col] = null;


        explosion = ObjectGetter.explosion.GetObje();
        explosion.transform.position = transform.position;
        ps = explosion.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = myColor[id];
        explosion.SetActive(true);


        gameObject.SetActive(false);
    }


}
