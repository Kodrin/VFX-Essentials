using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace CodrinMihail.VFXWorkshop
{

    public class GraphEventSample : MonoBehaviour
    {
        public VisualEffect graph;
        
        // Start is called before the first frame update
        protected void Start()
        {

        }

        // Update is called once per frame
        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SpawnParticlesEvent();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                StopParticlesEvent();
            }
        }

        protected void SpawnParticlesEvent()
        {
            graph.SendEvent("SpawnParticles");
        }

        protected void StopParticlesEvent()
        {
            graph.SendEvent("StopParticles");
        }
    }

}
