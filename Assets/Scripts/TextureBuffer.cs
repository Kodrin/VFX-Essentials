using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;


[System.Serializable]
public struct ParticleData
{
    public Vector3 position;
    public Vector3 velocity;
    public float size;
}

public class TextureBuffer : MonoBehaviour
{
    private const int TEX_RESOLUTION = 32;
    private const int PARTICLE_COUNT = TEX_RESOLUTION * TEX_RESOLUTION;
    public Vector3 bounds = new Vector3(5,5,5);
    public Renderer visualizePositions;
    // public int particleCount = 1024;

    [SerializeField] protected VisualEffect graph;
    [SerializeField] protected RenderTexture particlePosTex;
    [SerializeField] protected ComputeShader particleCS;

    protected const string ENCODE_POSITION = "EncodePosition";
    protected const string PARTICLE_MOVE = "ParticleMove";
    protected int encodePosKernelIndex;
    protected int particleMoveKernelIndex;
    protected ComputeBuffer particleBuffer;
    
    // Start is called before the first frame update
    protected void Start()
    {
        InitializeCompute();
        InitializeParticles();
    }

    // Update is called once per frame
    protected void Update()
    {
        SetConstants();
        DispatchEncodePos();
        DispatchParticleMove();
        
        VisualizePositions();
        SetBufferToGraph();
        
        if(Input.GetKeyDown(KeyCode.A)) DebugParticleData();
    }

    protected ParticleData CreateParticle()
    {
        ParticleData p = new ParticleData();
        
        //random inside the bounds
        p.position = (Random.insideUnitSphere);
        // p.position = (Random.insideUnitSphere * 2 - Vector3.one);
        // p.position.x *= bounds.x/2;
        // p.position.y *= bounds.y/2;
        // p.position.z *= bounds.z/2;
        // p.position += this.transform.position; //add gameobject position
        
        //random direction
        p.velocity = Random.insideUnitSphere * 2 - Vector3.one;
        
        //random size
        p.size = Random.value;
        
        return p;
    }
    
    protected RenderTexture CreateRT(int resolution, FilterMode filterMode)
    {
        RenderTexture texture = new RenderTexture(resolution,resolution,1, RenderTextureFormat.ARGBFloat);

        texture.name = "Particles";
        texture.enableRandomWrite = true;
        texture.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
        texture.volumeDepth = 1;
        texture.filterMode = filterMode;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.autoGenerateMips = false;
        texture.useMipMap = false;
        texture.Create();

        return texture;

    }

    protected void InitializeCompute()
    {
        //find the function in compute shader
        encodePosKernelIndex = particleCS.FindKernel(ENCODE_POSITION);
        particleMoveKernelIndex = particleCS.FindKernel(PARTICLE_MOVE);
        
        //initiliaze the texture
        particlePosTex = CreateRT(TEX_RESOLUTION, FilterMode.Point);
        
    }

    protected void InitializeParticles()
    {
        //initialize the buffer
        particleBuffer = new ComputeBuffer(PARTICLE_COUNT, Marshal.SizeOf(typeof(ParticleData)));
        
        //temp array to initialize particles
        ParticleData[] arr = new ParticleData[PARTICLE_COUNT];
        
        //fill that array with particle
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = CreateParticle();
            
        }
        
        //put that array into a compute buffer
        particleBuffer.SetData(arr);
    }

    protected void SetConstants()
    {
        particleCS.SetInt("_Resolution", TEX_RESOLUTION);
        particleCS.SetVector("_Bounds", bounds);
        particleCS.SetVector("_BoundsPosition", this.transform.position);
    }

    protected void DispatchEncodePos()
    {
        //set texture and buffer
        particleCS.SetTexture(encodePosKernelIndex,"_ParticlePositions", particlePosTex);
        particleCS.SetBuffer(encodePosKernelIndex,"_ParticleBuffer", particleBuffer);
        
        //call the function in the compute shader
        particleCS.Dispatch(encodePosKernelIndex, 16,16,1);
    }
    
    protected void DispatchParticleMove()
    {
        //set texture and buffer
        particleCS.SetBuffer(particleMoveKernelIndex,"_ParticleBuffer", particleBuffer);
        
        //call the function in the compute shader
        particleCS.Dispatch(particleMoveKernelIndex, (int)(PARTICLE_COUNT / 64),1,1);
    }

    protected void VisualizePositions()
    {
        visualizePositions.material.SetTexture("_UnlitColorMap", particlePosTex);
    }

    protected void SetBufferToGraph()
    {
        graph.SetTexture("PositionsBufferTex", particlePosTex);
    }
    
    protected void DebugParticleData()
    {
        ParticleData[] particleArr = new ParticleData[PARTICLE_COUNT];
        particleBuffer.GetData(particleArr);

        for (int i = 0; i < 15; i++)
        {
            int index = Random.Range(0, particleArr.Length);
            Vector3 position = particleArr[i].position;
            Vector3 direction = particleArr[i].velocity;
            float size = particleArr[i].size;
            Debug.Log($"Agent #{index}: Position {position}, Vel {direction}");
        }
            
    }

    protected void OnDestroy()
    {
        //flush out the textures
        if (particlePosTex != null)
        {
            particlePosTex.Release();
        }
        
        //flush out the buffer on the GPU
        if (particleBuffer != null)
        {
            particleBuffer.Release();
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(this.transform.position, bounds);
    }
}
