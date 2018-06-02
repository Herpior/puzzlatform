using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {

        public bool onFire = false;
        
        public Transform waterCheck;           // A position marking the center of circle where to check if the object is in water or fire.
        public float waterCheckRadius = 0.1f;   // Radius of water check circle
        public LayerMask waterLayer;
        public LayerMask fireLayer;
        private Renderer rend;

	// Use this for initialization
	void Start () {
            //Fetch the Renderer from the GameObject
            rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
            if(onFire) onFire = !Physics2D.OverlapCircle(waterCheck.position, waterCheckRadius, waterLayer);
            else onFire = Physics2D.OverlapCircle(waterCheck.position, waterCheckRadius, fireLayer);
            
            if(onFire) {
                //Set the main Color of the Material to green
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.red);
            }
            else {
                //Set the main Color of the Material to green
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.grey);
            }
            //Find the Specular shader and change its Color to red
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
	}
}
