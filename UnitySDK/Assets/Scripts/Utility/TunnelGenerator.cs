using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour {

    public GameObject LeftSideBlock, RightSideBlock, JumpBlock, SidesBlock, OmniBlock;
    public float DistanceBetweenBlocks;
    public GameObject Tunnel;
    public int seed;
    private TunnelGenerator nextSection;
    [SerializeField]
    private int difficultyLevel;
    private float zDimension;
    public System.Random rand;
    private static List<GameObject> availableBlocks;
    private bool tunnelGenerated = false;
    public bool Initialized = false;

    // Use this for initialization
    void Start () {
        zDimension = GetComponent<MeshRenderer>().bounds.extents.z;
        if (!Initialized)
        {
            Difficulty = difficultyLevel;
            GenerateNewSection();
        }
        Initialized = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (IsOutOfBounds)
        {
            nextSection.GenerateNewSection();
            Destroy(gameObject);
        }

	}

    void GenerateNewSection()
    {
        GameObject tunnel = Instantiate(Tunnel);
        Vector3 newPosition = transform.position;
        newPosition.z = newPosition.z - zDimension*2;
        tunnel.transform.position= newPosition;
        tunnel.name = "Yet another Tunnelish Thingy!";
        nextSection = tunnel.GetComponent<TunnelGenerator>();
        nextSection.Initialized = true;
        nextSection.rand = rand;
        float zPos = tunnel.transform.position.z + zDimension-1;
        for(int i = 0; i < tunnel.transform.childCount; i++)
        {
            if (tunnel.transform.GetChild(i).name.Contains("Block"))
                Destroy(tunnel.transform.GetChild(i).gameObject, 1);
        }
        while (zPos > tunnel.transform.position.z -zDimension+2)
        {
            int selection = rand.Next(availableBlocks.Count);
            GameObject newBlock = Instantiate(availableBlocks[selection]);
            Vector3 blockPosition = tunnel.transform.position;
            blockPosition.z = zPos;
            newBlock.transform.position = blockPosition;
            zPos -= DistanceBetweenBlocks;
            newBlock.transform.parent = tunnel.transform;
        }
        tunnelGenerated = true;
    }

    private bool IsOutOfBounds
    {
        get
        {
            return transform.position.z - zDimension > Camera.main.transform.position.z;
        }
    }
    /// <summary>
    /// Difficulty of the generated tunnels, the following blocks are included at each level:
    /// 0 : JumpBlock
    /// 1 : LeftSide & RightSide Block
    /// 2 : Jump, Left, Right
    /// 3 : All
    /// </summary>
    public int Difficulty
    {
        get
        {
            return difficultyLevel;
        }

        set
        {
            if (Initialized)
                return;
            if (value < 0 || value > 3)
                throw new UnityException("Difficulty must be a value between 0 and 3 (inclusive)");
            if (seed < 0)
                rand = new System.Random((int)(System.DateTime.Now.ToFileTime() % int.MaxValue));
            else
                rand = new System.Random(seed);
            difficultyLevel = value;
            availableBlocks = new List<GameObject>();
            if (difficultyLevel != 0)
            {
                availableBlocks.Add(LeftSideBlock);
                availableBlocks.Add(RightSideBlock);
            }
            if (difficultyLevel > 2)
            {
                availableBlocks.Add(SidesBlock);
                availableBlocks.Add(OmniBlock);
            }

            if (difficultyLevel != 1)
                availableBlocks.Add(JumpBlock);
        }
    }

}
