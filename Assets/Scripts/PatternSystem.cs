using UnityEngine;
using System.Collections;

/*

Grid is the host of the collectableType. PatternSystem works on 2d grid map.
2d grip map is computed for each new pattern is required.
After pattern is created CollectableSpawner transforms this data to rotated positions.

Ex: Line Pattern

|------ 18 -----|

OO2OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...

This pattern shows as 0 means no collectable, 
1 as Gold
2 as Speed Boost

For the pattern maximum Width is 18. I put them and see which fits best. 18 seemed okay.
In other words 18 golds can be in a single line.



 */

public class Grid 
{
	public Grid ()
	{
		collectableType = 0 ;
	}
	public CollectableType collectableType ;
}
  
/*

Pattern types,
0: Blank
1: Gold
2: Speed Boost
3: Slow

PT_Line; this pattern creates single line pattern width = 1.
Ex: 

OO2OOOOOO...
OO1OOOOOO...
OO1OOOOOO...

PT_DoubleLine; this pattern creates 2 line pattern which is width = 2.

OO22OOOOO...
OO11OOOOO...
OO11OOOOO...

PT_TripleLine; this pattern creates 2 line pattern which is width = 2.

OO222OOOO...
OO111OOOO...
OO111OOOO...

PT_LineWithSlow; this pattern creates 1 line pattern with gold and slow debuff next to it

OO23OOOOO...
OO13OOOOO...
OO13OOOOO...

PT_FullLine; this pattern creates full line pattern with gold 

111111111...
111111111...
111111111...

PT_X; this pattern creates X line pattern with gold 

1OOOOOOO1...
O1OOOOO1O...
OO1OOO1OO...
OOO1O1OOO...
OOOO1OOOO...
OOO1O1OOO...
OO1OOO1OO...
O1OO1OO1O...
1OOO1OOO1...

PT_X; this pattern creates X line pattern with Slow 

3OOOOOOO3...
O3OOOOO3O...
OO3OOO3OO...
OOO3O3OOO...
OOOO3OOOO...
OOO3O3OOO...
OO3OOO3OO...
O3OO1OO3O...
3OOO1OOO3...
 */
public enum PatternType
{
	PT_Line = 1 ,
	PT_DoubleLine = 2,
	PT_TripleLine = 3, 
	PT_LineWithSlow = 4, 
	PT_FullLine = 5,
	PT_X = 6,
	PT_DoubleX = 7,
	PT_X_Red = 8,
}
/*

PatternSystem class is responsible for creating blended procedural and manual design patters of

*/
public class PatternSystem {

	private Grid[,] grids = null;
	float fTimeBetweenPatters = 0.0f ;
	int iMaxX = 18 ; 
	int iMaxY = 16 ; 
	int iSequenceY = 0 ;
	bool bPatternComplete ;
	int iHardness ;
		  
	public PatternSystem ()
	{
		grids = new Grid[iMaxX,iMaxY]; 

		for (int x = 0 ; x < iMaxX ; x++)
		{
			for (int y = 0; y < iMaxY; y++)			
				grids[x,y] = new Grid ();
		}

		bPatternComplete = false ;
		iHardness = 0 ;
		ClearPattern ();
	}

	public float GetTimeBetweenPatterns ()
	{
		return fTimeBetweenPatters;
	}

	public Grid[,] GetGrids ()
	{
		return grids ;
	}

	public int GetHardness ()
	{
		return iHardness ;
	}

	public void ResetHardness ()
	{
		this.iHardness = 0 ;
	}

	public void IncreaseHardness ()
	{
		iHardness++ ; 
	}

	public int GetSequenceY ()
	{
		return iSequenceY ;
	}

	public void IncrementSequenceY ()
	{
		iSequenceY++ ;
		if (iSequenceY == iMaxY)
		{
			iSequenceY = 0 ;
			ClearPattern ();
			bPatternComplete = true ;
		}
	}

	public bool GetPatternComplete ()
	{
		return bPatternComplete;
	}
	 
	void ClearPattern ()
	{
		for (int x = 0 ; x < iMaxX ; x++)
		{
			for (int y = 0; y < iMaxY; y++) 
				grids[x,y].collectableType = 0 ;
		}
	}

	public int GetMaxX ()
	{
		return iMaxX;
	}

	/*

	SetPattern method populate the 2d array grids with collectabl types within given patternType.
	iHardness parameter is increased over time.
	Higher iHardness, the game will become more difficult.
	But this difficulty is not linear, as can be checked from the below codes, 
	it increases the chance of failure, but not forcing it. This is important.
	For example; Line pattern creates minimum 14 gold to maximum 16 gold (int iMaxY = 16 )
	By iHardness increases over time, it will start to generate between (14 - iHardness) and 16.
	Which means it can be rolled as high as before but now lower values can be rolled as well.

	SetPattern method is open to fine tunning as it has configuration parameters. 
	Can be configured for best expreience.
	 */

	public void SetPattern (PatternType patternType)
	{
		bPatternComplete = false ;

		switch (patternType) 
		{
		case PatternType.PT_Line:
		{ 
			int iRandX = Random.Range (0, iMaxX); 
			int iLowY = 14 - iHardness ;
			if (iLowY < 0)
				iLowY = 0 ;
			int iRandYLen = Random.Range (iLowY, iMaxY );
 
			for (int i = 0; i < iRandYLen; i++) 
			{
				grids[iRandX, i].collectableType = CollectableType.CT_GOLD ;
			}

			int iNofSlow = iHardness / 2 ;
			if (iNofSlow > 10)
				iNofSlow = 10 ;
			int iRandSlow = Random.Range (0, iMaxY - iNofSlow );
			
			for (int i = iRandSlow; i < iRandSlow + iNofSlow; i++) 
			{ 
				grids[iRandX, i].collectableType = CollectableType.CT_SLOW ;
			}

			int iRand = Random.Range (1, 2);
			if (iRand == 1)
				if (iRandSlow + iNofSlow-1 > 0 && iRandSlow + iNofSlow-1 < iMaxY)
					grids[iRandX, iRandSlow + iNofSlow-1].collectableType = CollectableType.CT_SPEEDBOOST ;
		}
		break;

		case PatternType.PT_DoubleLine:
		{ 
			int iRandX = Random.Range (0, iMaxX-1); 
			int iLowY = 14 - iHardness ;
			if (iLowY < 0)
				iLowY = 0 ;
			int iRandYLen = Random.Range (iLowY, iMaxY );

			for (int i = 0; i < iRandYLen; i++) 
			{
				grids[iRandX, i].collectableType = CollectableType.CT_GOLD ;
				grids[iRandX+1, i].collectableType = CollectableType.CT_GOLD ;
			}

			int iNofSlow = iHardness / 2 ;
			if (iNofSlow > 10)
				iNofSlow = 10 ;
			int iRandSlow = Random.Range (0, iMaxY - iNofSlow );

			int iRandVal = Random.Range (0,3);

			for (int i = iRandSlow; i < iRandSlow + iNofSlow; i++) 
			{ 					
				grids[iRandX + iRandVal, i].collectableType = CollectableType.CT_SLOW ;
				if (iRandVal == 2)
				{
					grids[iRandX, i].collectableType = CollectableType.CT_SLOW ;
					grids[iRandX+1, i].collectableType = CollectableType.CT_SLOW ;
				}

			}

			int iRand = Random.Range (1, 2);
			if (iRand == 1)
				if (iRandSlow + iNofSlow-1 > 0 && iRandSlow + iNofSlow-1 < iMaxY)
				grids[iRandX, iRandSlow + iNofSlow-1].collectableType = CollectableType.CT_SPEEDBOOST ;
		}
		break;

		case PatternType.PT_TripleLine:
		{
			int iRandX = Random.Range (0, iMaxX-2); 
			int iLowY = 14 - iHardness ;
			if (iLowY < 0)
				iLowY = 0 ;

			int iRandYLen = Random.Range (iLowY, iMaxY );
			for (int i = 0; i < iRandYLen; i++) 
			{
				grids[iRandX, i].collectableType = CollectableType.CT_GOLD ;
				grids[iRandX+1, i].collectableType = CollectableType.CT_GOLD ;
				grids[iRandX+2, i].collectableType = CollectableType.CT_GOLD ;
			}

			int iNofSlow = iHardness / 2 ;
			if (iNofSlow > 10)
				iNofSlow = 10 ;
			int iRandSlow = Random.Range (0, iMaxY - iNofSlow );

			int iRandVal = Random.Range (0,3);

			for (int i = iRandSlow; i < iRandSlow + iNofSlow; i++) 
			{ 

				grids[iRandX + iRandVal, i].collectableType = CollectableType.CT_SLOW ;
				if (iRandVal == 2)
				{
					grids[iRandX + iRandVal, i].collectableType = CollectableType.CT_SLOW ;
					grids[iRandX+1, i].collectableType = CollectableType.CT_SLOW ;
					grids[iRandX+2, i].collectableType = CollectableType.CT_SLOW ;
				}

			}

			int iRand = Random.Range (1, 2);
			if (iRand == 1)
				if (iRandSlow + iNofSlow-1 > 0 && iRandSlow + iNofSlow-1 < iMaxY)
				grids[iRandX, iRandSlow + iNofSlow-1].collectableType = CollectableType.CT_SPEEDBOOST ;

		}
		break;

		case PatternType.PT_LineWithSlow:
		{
			int iRandX = Random.Range (0, iMaxX-2);
			int iLowY = 10 - iHardness ;
			if (iLowY < 0)
				iLowY = 0 ;
			int iRandYLen = Random.Range (iLowY, iMaxY );

			for (int i = 0; i < iRandYLen; i++) 
			{
				grids[iRandX, i].collectableType = CollectableType.CT_GOLD ;

			}

			int iNof = Random.Range (0, iHardness);
			int iStart = Random.Range (0, iRandYLen);

			for (int i = iStart; i < iStart + iNof; i++) 
			{
				grids[iRandX + 2, i].collectableType = CollectableType.CT_SLOW ;
			}
		}
		break;

		case PatternType.PT_X:
		{
			int iLowLen = 10 - iHardness ;
			if (iLowLen < 0)
				iLowLen = 0 ;

			int iLen = Random.Range (iLowLen, 10 );

 
			int iRandX = Random.Range (0, iMaxX-1);
			int iRandY = Random.Range (0, iMaxY-1);

			int iWidth = Random.Range (0, 3);

			for (int i = -iLen; i < iLen; i++) 
			{
				if (iRandX + i > 0 && iRandX + i < iMaxX)
					if (iRandY + i > 0 && iRandY + i < iMaxY)
				{
					grids[iRandX + i, iRandY + i].collectableType = CollectableType.CT_GOLD ;
					for (int x = 0; x < iWidth; x++)
					{
						if (iRandX + i + x < iMaxX)
							grids[iRandX + i + x, iRandY + i].collectableType = CollectableType.CT_GOLD ;

					}
				}	

				if (iRandX - i > 0 && iRandX - i < iMaxX)
					if (iRandY + i > 0 && iRandY + i < iMaxY)
				{
					grids[iRandX - i, iRandY + i].collectableType = CollectableType.CT_GOLD ;

					for (int x = 0; x < iWidth; x++)
					{
						if (iRandX - i -x > 0)
							grids[iRandX - i - x, iRandY + i].collectableType = CollectableType.CT_GOLD ;
					}
				}			
			}

			if (iHardness > 5)
			{
				int iRand = Random.Range (0, 3);
				if (iRand == 1)
					grids[iRandX, iRandY].collectableType = CollectableType.CT_SLOW ;	
				if (iRand == 2)
					grids[iRandX, iRandY].collectableType = CollectableType.CT_SPEEDBOOST ;
			}
		}
		break;

		case PatternType.PT_FullLine:
		{
			int iLowLen = 10 - iHardness ;
			if (iLowLen < 0)
				iLowLen = 0 ;

			int iRandY = Random.Range (iLowLen, iMaxY - 5);
			int iStartRandX = Random.Range (2, 10);
			int iNofX = Random.Range (2, 8);

			for (int x = iStartRandX; x < iStartRandX + iNofX; x++) 
			{
				for (int y = 0; y < iRandY; y++) 
				{
					grids[x, y].collectableType = CollectableType.CT_GOLD ;	
				}
			}

			int iNofSlow = iHardness / 2 ;
			if (iNofSlow > 10)
				iNofSlow = 10 ;
			 
			for (int x = 0; x < iNofSlow; x++) 
			{
				int iRx = Random.Range (0, iMaxX);
				int iRy = Random.Range (0, iRandY);
				
				grids[iRx, iRy].collectableType = CollectableType.CT_SLOW ;						
			}
		}
			break;

		case PatternType.PT_X_Red:
		{  			
			int iLen = Random.Range (1, iHardness + 1 );

			int iRandX = Random.Range (0, iMaxX-1);
			int iRandY = Random.Range (0, iMaxY-1);

			int iRand = Random.Range (0,2);
						
			for (int i = -iLen; i < iLen; i++) 
			{
				if (iRandX + i > 0 && iRandX + i < iMaxX)
					if (iRandY + i > 0 && iRandY + i < iMaxY)
				{
					CollectableType ct = CollectableType.CT_SLOW ;
					if (iRand == 0)
						ct = CollectableType.CT_GOLD;
					grids[iRandX + i, iRandY + i].collectableType = ct ;	
				}	
				
				if (iRandX - i > 0 && iRandX - i < iMaxX)
					if (iRandY + i > 0 && iRandY + i < iMaxY)
				{
					CollectableType ct = CollectableType.CT_GOLD ;
					if (iRand == 0)
						ct = CollectableType.CT_SLOW;
					grids[iRandX - i, iRandY + i].collectableType = ct ;					
				}			
			} 

			if (iRandX + iLen-1 < iMaxX && iRandY + iLen-1 < iMaxY)
				grids[iRandX + iLen-1, iRandY + iLen-1].collectableType = CollectableType.CT_SPEEDBOOST ;
			if (iRandX - iLen-1 > 0 && iRandY + iLen-1 < iMaxY)
				grids[iRandX - iLen-1, iRandY + iLen-1].collectableType = CollectableType.CT_SPEEDBOOST ;
		}
			break;
			
		default:
				break;
		}
	}
}
