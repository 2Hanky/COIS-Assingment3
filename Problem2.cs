using System;
using System.Collections.Generic;

namespace Problem2
{
	class Program
	{
		// Simple BST node
		class Node
		{
			public int Val;
			public Node? Left, Right;
			public Node(int v) { Val = v; Left = null; Right = null; }
		}

		// Index used by the O(n) construction routine
		static int idx;

		// Try to build a BST from the given preorder array in O(n) time.
		// Returns true and sets root if successful; returns false if the preorder
		// cannot represent a BST (or not all elements are consumed).
		static bool TryBuildBSTFromPreorder(int[] preorder, out Node? root)
		{
			idx = 0;
			root = Build(preorder, long.MinValue, long.MaxValue);
			// If we did not consume all values, the preorder is invalid for a BST
			if (idx != preorder.Length)
			{
				root = null;
				return false;
			}
			return true;
		}

		// Recursive helper using min/max bounds. Runs in O(n) because each element
		// is processed exactly once and idx only increases.
		static Node? Build(int[] preorder, long lowerBound, long upperBound)
		{
			if (idx >= preorder.Length)
				return null;

			int val = preorder[idx];
			if (val <= lowerBound || val >= upperBound)
				return null;

			// consume
			idx++;
			var node = new Node(val);
			node.Left = Build(preorder, lowerBound, val);
			node.Right = Build(preorder, val, upperBound);
			return node;
		}

		// Utility: get inorder traversal (should be sorted for a valid BST)
		static void InOrder(Node? root, List<int> outList)
		{
			if (root == null) return;
			InOrder(root.Left, outList);
			outList.Add(root.Val);
			InOrder(root.Right, outList);
		}

		static void Main(string[] args)
		{
			// clear console at start
			try { Console.Clear(); } catch { /* ignore if no console attached */ }
			// Two test cases: one valid preorder for a BST and one invalid.
			var tests = new List<int[]>
			{
				// Valid BST preorder
				new int[] {8, 5, 1, 7, 10, 12},

				// Invalid BST preorder: 8 -> 10 (right), then 5 appears which cannot belong
				// because it is less than 8 but appears after a right subtree element.
				new int[] {8, 10, 5}
			};

			for (int i = 0; i < tests.Count; i++)
			{
				Console.WriteLine($"Test #{i + 1} preorder: [{string.Join(", ", tests[i])}]");
				if (TryBuildBSTFromPreorder(tests[i], out var root))
				{
					var inorder = new List<int>();
					InOrder(root, inorder);
					Console.WriteLine("BST built successfully. Inorder traversal: [{0}]", string.Join(", ", inorder));
				}
				else
				{
					Console.WriteLine("Cannot build a binary search tree from the given preorder traversal.");
				}
				Console.WriteLine();
			}
		}
	}
}

