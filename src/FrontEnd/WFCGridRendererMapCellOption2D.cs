using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///  Clase encargada de renderizar un modulo de nuestra funcion de onda en un entorno 2D.
/// </summary>
public class WFCGridRendererMapCellOption2D : WFCGridRendererMapCellOption
{


    public GameStatusHandler gameStatusHandler;
    public Module2D module2D;
    public bool isAvailable = true;
    public Color frameNormalColor;
    public Color spriteNormalColor;
    public Color hoverColor = Color.red; 
    public bool isHover;
    public GameObject sprite;
    public GameObject frame;
    public bool inputEnable = true;
    
    // Update is called once per frame
    void Update()
    {
        
        if (!available && isAvailable && !isOnly) {

            this.BlowUp();
            isAvailable = available;
        }

        if (transform.position.y < 0) {
            Destroy(this.gameObject);
        }

        if (!isOnly) return;


        
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, this.selectedPosition, Time.deltaTime);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.selectionScale, Time.deltaTime);
        this.sprite.GetComponent<SpriteRenderer>().color = this.spriteNormalColor;


        if (this.frame != null) this.frame.SetActive(false);
        this.mapCellRenderer.DisableFrame();
        //if (this.sprite == null || this.sprite.GetComponent<SpriteRenderer>() == null || this.sprite.GetComponent<SpriteRenderer>().color.Equals(this.normalColor)) return;
        


    }

    void OnDestroy()
    {
        if (this.mapCellRenderer.wfcGridRenderer.gameStatusHandler == null) return;
        this.mapCellRenderer.wfcGridRenderer.gameStatusHandler.UnSubscribeToGameStatusEvent(OnGameStatusChanged);
    }

    private void OnMouseEnter()
    {
        if (!this.inputEnable) return;
        this.isHover = true;
        if (this.frame == null || this.frame.GetComponent<SpriteRenderer>() == null || isOnly) return;
        if (this.sprite == null || this.sprite.GetComponent<SpriteRenderer>() == null || isOnly) return;
        this.frame.GetComponent<SpriteRenderer>().color = this.hoverColor;
        this.sprite.GetComponent<SpriteRenderer>().color = this.hoverColor;
    }

    private void OnMouseExit()
    {
        if (!this.inputEnable) return;
        this.isHover = false;
        if (this.frame == null || this.frame.GetComponent<SpriteRenderer>() == null || isOnly) return;
        if (this.sprite == null || this.sprite.GetComponent<SpriteRenderer>() == null || isOnly) return;
        this.frame.GetComponent<SpriteRenderer>().color = this.frameNormalColor;
        this.sprite.GetComponent<SpriteRenderer>().color = this.spriteNormalColor;

    }

    private void OnMouseOver()
    {

        if (isOnly) return;
        if (!this.inputEnable) return;

        if (this.GetComponent<BoxCollider2D>() == null || !this.GetComponent<BoxCollider2D>().enabled) return;

        if (Input.GetMouseButtonDown(1)) //Click Derecho
        {
            this.mapCellRenderer.wfcGridRenderer.RemoveModule(this.mapCellRenderer.mapCell,this.module2D);
            
        }
        else if (Input.GetMouseButtonDown(0)) // Click Izquierdo 
        {
            this.mapCellRenderer.wfcGridRenderer.SelectionModule(this.mapCellRenderer.mapCell, this.module2D);
      
        }


    }

    public void BlowUp() {

        this.isHover = false;
         
        if (this.GetComponent<BoxCollider2D>() == null || !this.GetComponent<BoxCollider2D>().enabled) return;
        if (this.frame != null) this.frame.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        Rigidbody2D body = this.GetComponent<Rigidbody2D>() == null ? gameObject.AddComponent<Rigidbody2D>() : this.GetComponent<Rigidbody2D>();
        body.isKinematic = false;
        body.AddForce(
               new Vector2(UnityEngine.Random.Range(1, 3) * (UnityEngine.Random.value > .5f ? 1 : -1),
                   UnityEngine.Random.Range(4, 5)), ForceMode2D.Impulse);
        body.AddTorque(UnityEngine.Random.Range(1, 4) * (UnityEngine.Random.value > .5f ? 1 : -1),
            ForceMode2D.Impulse);


    }

    public override void OnCreated(Module module,WFCGridRendererMapCell mapCellRenderer)
    {
        this.mapCellRenderer = mapCellRenderer;
        Module2D module2D = module as Module2D;
  
        if (module2D == null) return;
        if (this.frame == null || this.frame.GetComponent<SpriteRenderer>() == null) return;
        if (this.sprite == null || this.sprite.GetComponent<SpriteRenderer>() == null) return;
        if (this.GetComponent<BoxCollider2D>() == null) return;

        this.module2D = module2D;
        this.sprite.GetComponent<SpriteRenderer>().sprite = this.module2D.sprite;
        this.spriteNormalColor = this.sprite.GetComponent<SpriteRenderer>().color;
        this.frameNormalColor = this.frame.GetComponent<SpriteRenderer>().color;
        this.startPosition = this.transform.localPosition;
        this.startScale = this.transform.localScale;
        this.selectedPosition = new Vector3(0.5f,0.5f, 0);
        this.selectionScale = new Vector3(1, 1, 1);
        this.isAvailable = true;
        if (this.mapCellRenderer.wfcGridRenderer.gameStatusHandler == null) return;
        this.mapCellRenderer.wfcGridRenderer.gameStatusHandler.SubscribeToGameStatusEvent(OnGameStatusChanged);


    }

    public void OnGameStatusChanged(GameStatus newGameStatus)
    {
        this.inputEnable = newGameStatus.Equals(GameStatus.Gameplay) ? true : false;
    }


}
