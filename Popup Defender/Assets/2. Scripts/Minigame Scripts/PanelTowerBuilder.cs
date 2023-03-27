using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTowerBuilder : IPanelStrategy
{
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    private GameObject buildingBlockStack;
    private GameObject activeBlock;
    private GameObject previousBlock;
    private int blockCount;
    private float blockOffset;
    float direction = 3f;
    private List<GameObject> blockList = new List<GameObject>();

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public int ObjectiveKeyTech() => 1;
    public string SetPanelBG() => "sprBG_burger";
    public string ObjectiveDesc() => "to place the block";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent;
        myDisplay = displayParent;
        sizeX = 2f;
        sizeY = 2.5f;

        if (buildingBlockStack == null)
        {
            buildingBlockStack = GameObject.Instantiate(Resources.Load("Building Block Stack"), Vector3.down * 2 + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        blockCount = 0;
        activeBlock = null;
        previousBlock = null;
        blockList.Clear();

        for (int i = 0; i < buildingBlockStack.transform.childCount; i++)
        {
            blockList.Add(buildingBlockStack.transform.GetChild(i).gameObject);
        }

        foreach (GameObject g in blockList)
        {
            g.SetActive(false);
        }

        blockList[0].SetActive(true);
        activeBlock = blockList[0];
    }

    public void OnControlDown()
    {
        if (previousBlock == null)
        {
            FirstBlock();
        }
        else
        {
            if ((activeBlock.transform.position.x + (activeBlock.transform.lossyScale.x) / 2) < previousBlock.transform.position.x - (previousBlock.transform.lossyScale.x) / 2 || (activeBlock.transform.position.x - (activeBlock.transform.lossyScale.x) / 2) > previousBlock.transform.position.x + (previousBlock.transform.lossyScale.x) / 2)//check if the blocks are touching, this is if block are not touching
            {
                MissedBlock();
            }
            else //if the blocks are touching
            {
                if (blockCount == blockList.Count - 1) // check if all the blocks are out, there are no blocks left
                {
                    LastBlock();
                }
                else // There are remaining blocks
                {
                    direction = 0; //stop the active block
                    BlockWithin();
                    //    if ((activeBlock.transform.position.x + (activeBlock.transform.lossyScale.x) / 2) < (previousBlock.transform.position.x + (previousBlock.transform.lossyScale.x) / 2) && (activeBlock.transform.position.x - (activeBlock.transform.lossyScale.x) / 2) > (previousBlock.transform.position.x - (previousBlock.transform.lossyScale.x) / 2))
                    //    {
                    //        
                    //    }
                    //    else if (activeBlock.transform.position.x < previousBlock.transform.position.x) // check the active block against the previous block to determine side of block, active block is on the left. !!left x is negative, right x is positive!!
                    //    {
                    //        BlockOverOnLeft();
                    //    }
                    //    else //active block is on the right
                    //    {
                    //        BlockOverOnRight();
                    //    }
                }
            }

        }
    }

    void FirstBlock()
    {
        direction = 0; //stops the active block
        previousBlock = activeBlock; //set current active block as previous block   
        blockCount++; // increases the block count
        blockList[blockCount].SetActive(true); //Set next block active
        activeBlock = blockList[blockCount]; // set new block as active block
        activeBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = previousBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder + 1;
        direction = 3f; //Start the active block
    }
    void MissedBlock()
    {
        myPanel.GetComponent<Panel>().ForceTimeLeft(-1, false, true);
    }
    void LastBlock()
    {
        direction = 0; //stops the active block
        myPanel.GetComponent<Panel>().SetSuccess(true);
    }
    //void BlockOverOnLeft()
    //{
    //    myPanel.GetComponent<Panel>().ForceTimeLeft(2, false, true);
    //    blockOffset = ((previousBlock.transform.position.x - previousBlock.transform.lossyScale.x / 2) - (activeBlock.transform.position.x - activeBlock.transform.lossyScale.x / 2)); //calculates blockoffset by taking left edge of active block - left edge of previous block
    //    previousBlock = activeBlock; //set current active block as previous block   
    //    blockCount++; // increases the block count
    //    blockList[blockCount].SetActive(true); //Set next block active
    //    blockList[blockCount].transform.localScale = new Vector3(previousBlock.transform.lossyScale.x - blockOffset, 0.75f, 1); //reduce the scale of the new block by whatever the offset was
    //    activeBlock = blockList[blockCount]; // set new block as active block
    //    activeBlock.GetComponent<SpriteRenderer>().sortingOrder = previousBlock.GetComponent<SpriteRenderer>().sortingOrder + 1;
    //    direction = 3f; //Start the active block
    //}
    //void BlockOverOnRight()
    //{
    //    myPanel.GetComponent<Panel>().ForceTimeLeft(2, false, true);
    //    blockOffset = ((activeBlock.transform.position.x + activeBlock.transform.lossyScale.x / 2) - (previousBlock.transform.position.x + previousBlock.transform.lossyScale.x / 2)); //calculates blockoffset by taking rigt edge of active block - right edge of previous block
    //    previousBlock = activeBlock; //set current active block as previous block   
    //    blockCount++; // increases the block count
    //    blockList[blockCount].SetActive(true); //Set next block active
    //    blockList[blockCount].transform.localScale = new Vector3(previousBlock.transform.lossyScale.x - blockOffset, 0.75f, 1); //reduce the scale of the new block by whatever the offset was
    //    activeBlock = blockList[blockCount]; // set new block as active block
    //    activeBlock.GetComponent<SpriteRenderer>().sortingOrder = previousBlock.GetComponent<SpriteRenderer>().sortingOrder + 1;
    //    direction = 3f; //Start the active block
    //}
    void BlockWithin()
    {
        //myPanel.GetComponent<Panel>().ForceTimeLeft(2, false, true);
        previousBlock = activeBlock; //set current active block as previous block   
        blockCount++; // increases the block count
        blockList[blockCount].SetActive(true); //Set next block active
        //blockList[blockCount].transform.localScale = new Vector3(previousBlock.transform.lossyScale.x, 0.75f, 1); //reduce the scale of the new block by whatever the offset was
        activeBlock = blockList[blockCount]; // set new block as active block
        activeBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = previousBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder + 1;
        direction = 3f; //Start the active block
    }

    public void OnControlHold()
    {

    }
    public void OnControlUp()
    {

    }
    public void OnTimeUp()
    {
    }
    public void MiniUpdate()
    {
        if (activeBlock.transform.localPosition.x >= 2.5 && direction > 0) direction *= -1;
        if (activeBlock.transform.localPosition.x <= -2.5 && direction < 0) direction *= -1;

        activeBlock.transform.position += new Vector3(Time.deltaTime * direction, 0, 0);
    }

}
