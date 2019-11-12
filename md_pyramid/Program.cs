using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace md_pyramid
{
	class Program
	{
		static readonly List<List<int>> pyramidInput_ = new List<List<int>>() {	new List<int>() { 1 },
																				new List<int>() { 8, 9},
																				new List<int>() { 1, 5, 9},
																				new List<int>() { 4, 5, 2, 3} };

		static readonly List<List<int>> pyramidInput = new List<List<int>>() {	new List<int>() { 215 },
																				new List<int>() { 192, 124},
																				new List<int>() { 117, 269, 442},
																				new List<int>() { 218, 836, 347, 235},
																				new List<int>() { 320, 805, 522, 417, 345},
																				new List<int>() { 229, 601, 728, 835, 133, 124},
																				new List<int>() { 248, 202, 277, 433, 207, 263, 257},
																				new List<int>() { 359, 464, 504, 528, 516, 716, 871, 182},
																				new List<int>() { 461, 441, 426, 656, 863, 560, 380, 171, 923},
																				new List<int>() { 381, 348, 573, 533, 448, 632, 387, 176, 975, 449},
																				new List<int>() { 223, 711, 445, 645, 245, 543, 931, 532, 937, 541, 444,},
																				new List<int>() { 330, 131, 333, 928, 376, 733, 017, 778, 839, 168, 197, 197,},
																				new List<int>() { 131, 171, 522, 137, 217, 224, 291, 413, 528, 520, 227, 229, 928,},
																				new List<int>() { 223, 626, 034, 683, 839, 052, 627, 310, 713, 999, 629, 817, 410, 121,},
																				new List<int>() { 924, 622, 911, 233, 325, 139, 721, 218, 253, 223, 107, 233, 230, 124, 233 } };



		const string filePath = @"";
		static int maxSum = int.MinValue;
		static List<PyramidBrick> maxSumPath = new List<PyramidBrick>();

		static void Main(string[] args)
		{
			Console.WriteLine("Program start.");
			PyramidBrick root = CreatePyramid();
			SearchForTheMaxSumPath(root, root.Value, new List<PyramidBrick>() { root});
			Console.WriteLine($"Result: maximum posible sum in this pyramid is {maxSum}.");
			Console.WriteLine($"The path is {ReturnStringPath()}.");
			Console.ReadKey();
		}

		public static PyramidBrick CreatePyramid ()
		{
			#region PYRAMID_CREATION

			List<PyramidBrick> pyramidRows = new List<PyramidBrick>();
			List<PyramidBrick> pyramidRowsInCreation = new List<PyramidBrick>();

			for (int i = pyramidInput.Count() - 1; i >= 0; i--)
			{
				pyramidRowsInCreation = new List<PyramidBrick>();

				for (int j = 0; j < pyramidInput[i].Count(); j++)
				{
					PyramidBrick brick = new PyramidBrick(pyramidInput[i][j], null, null);
					if (pyramidRows.Count() > j)
					{
						brick.LeftChild = pyramidRows[j];
						brick.RightChild = pyramidRows[j + 1];
					}
					pyramidRowsInCreation.Add(brick);
				}
				pyramidRows = pyramidRowsInCreation;
			}
			#endregion

			return pyramidRows[0];
		}

		public static void SearchForTheMaxSumPath( PyramidBrick brick, int sum, List<PyramidBrick> sumPath)
		{
			PyramidBrick child = brick;
			bool childIsRight = false;

			#region PATH_SEARCH

			//path search (left priority)
			if ((brick.LeftChild.Value + brick.Value) % 2 == 1)
				child = brick.LeftChild;
			else if ((brick.RightChild.Value + brick.Value) % 2 == 1)
			{
				childIsRight = true;
				child = brick.RightChild;
			}
			else
				return;

			sum = sum + child.Value;
			sumPath.Add(child);

			if (child.LeftChild == null && child.RightChild == null)
			{
				if (sum > maxSum)
				{
					maxSum = sum;
					maxSumPath = new List<PyramidBrick>(sumPath);
				}
				sumPath.Remove(child);
				return;
			}

			#endregion

			SearchForTheMaxSumPath(child, sum, sumPath);

			#region SEARCH_FOR_ANOTHER_PATH
			//returning + searching for yet unexplored path (left -> right)
			if ((brick.RightChild.Value + brick.Value) % 2 == 1 && !childIsRight)
			{
				sum = sum - child.Value + brick.RightChild.Value;
				sumPath.Remove(child);
				child = brick.RightChild;
				sumPath.Add(child);
				if (child.LeftChild == null && child.RightChild == null)
				{
					if (sum > maxSum)
					{
						maxSum = sum;
						maxSumPath = new List<PyramidBrick>(sumPath);
					}
					sumPath.Remove(child);
					return;
				}
				SearchForTheMaxSumPath(brick.RightChild, sum, sumPath);
				//if returned here, there is no need to check for the possible right path - we already explored it
			}

			#endregion
			sumPath.Remove(child);
			return;
		}

		public static string ReturnStringPath()
		{
			string path = string.Empty;
			foreach (PyramidBrick brick in maxSumPath) path = path + $"{brick.Value} ";
			return path;
		}
	}

	public class PyramidBrick
	{
		public int Value { get; set; }
		public PyramidBrick LeftChild { get; set; }
		public PyramidBrick RightChild { get; set; }

		public PyramidBrick (int value, PyramidBrick leftChild, PyramidBrick rightChild)
		{
			Value = value;
			LeftChild = leftChild;
			RightChild = rightChild;
		}
	}
}
