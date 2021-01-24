using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StuckLandingText : MonoBehaviour
{

    [SerializeField] GameObject rocket;
    Text text;

    void Start() {
        text = GetComponent<Text>();
        text.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        bool stuckLanding = rocket.GetComponent<Rocket>().IsStuckLanding();
        print("Stuck landing bool: " + stuckLanding);
        if (stuckLanding) {        
            text.text = "Stuck the landing!";
        }
    }
}
