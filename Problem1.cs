using System;
using System.Collections.Generic;


namespace Problem1
{
    class Program
	{
		// Simple binary tree node
		public class Node
		{
			public int Val;
			public Node? Left;
			public Node? Right;

			public Node(int val)
			{
				Val = val;
				Left = null;
				Right = null;
			}
		}

		static void Main(string[] args)
		{
			Console.Clear();
			// Build a sample tree:
			var root = BuildSampleTree();

			var inorderBefore = InorderRecursive(root);
			var preorderBefore = PreorderRecursive(root);

			Console.WriteLine("Standard recursive inorder before Morris: " + Stringify(inorderBefore));
			Console.WriteLine("Standard recursive preorder before Morris: " + Stringify(preorderBefore));

			var morris = MorrisInorder(root);
			Console.WriteLine("Morris inorder: " + Stringify(morris));

			var inorderAfter = InorderRecursive(root);
			var preorderAfter = PreorderRecursive(root);

			Console.WriteLine("Standard recursive inorder after Morris: " + Stringify(inorderAfter));
			Console.WriteLine("Standard recursive preorder after Morris: " + Stringify(preorderAfter));

			bool inorderMatch = SeqEqual(inorderBefore, morris) && SeqEqual(inorderBefore, inorderAfter);
			bool preorderMatch = SeqEqual(preorderBefore, preorderAfter);

			Console.WriteLine();
			Console.WriteLine("Verification:");
			Console.WriteLine(" - Morris produced same inorder as recursive: " + (SeqEqual(inorderBefore, morris) ? "PASS" : "FAIL"));
			Console.WriteLine(" - Tree structure reinstated (preorder before vs after): " + (preorderMatch ? "PASS" : "FAIL"));
			Console.WriteLine(" - Inorder before equals inorder after: " + (SeqEqual(inorderBefore, inorderAfter) ? "PASS" : "FAIL"));

			if (inorderMatch && preorderMatch)
				Console.WriteLine("All checks passed: Morris traversal correct and tree restored.");
			else
				Console.WriteLine("One or more checks failed. See output above.");
		}

		static Node BuildSampleTree()
		{
			var n1 = new Node(1);
			var n2 = new Node(2);
			var n3 = new Node(3);
			var n4 = new Node(4);
			var n5 = new Node(5);
			var n6 = new Node(6);

			n1.Left = n2;
			n1.Right = n3;
			n2.Left = n4;
			n2.Right = n5;
			n3.Right = n6;

			return n1;
		}

		// Standard recursive inorder
		static List<int> InorderRecursive(Node? root)
		{
			var res = new List<int>();
			void Dfs(Node? node)
			{
				if (node == null) return;
				Dfs(node.Left);
				res.Add(node.Val);
				Dfs(node.Right);
			}
			Dfs(root);
			return res;
		}

		// Standard recursive preorder (used to verify structure)
		static List<int> PreorderRecursive(Node? root)
		{
			var res = new List<int>();
			void Dfs(Node? node)
			{
				if (node == null) return;
				res.Add(node.Val);
				Dfs(node.Left);
				Dfs(node.Right);
			}
			Dfs(root);
			return res;
		}

		// Morris inorder traversal — O(1) extra space, restores tree structure
		static List<int> MorrisInorder(Node? root)
		{
			var res = new List<int>();
			var current = root;
			while (current != null)
			{
				if (current.Left == null)
				{
					res.Add(current.Val);
					current = current.Right;
				}
				else
				{
					// find the inorder predecessor of current
					var predecessor = current.Left;
					while (predecessor != null && predecessor.Right != null && predecessor.Right != current)
						predecessor = predecessor.Right;

					if (predecessor != null && predecessor.Right == null)
					{
						// make current the right child of its inorder predecessor
						predecessor.Right = current;
						current = current.Left;
					}
					else
					{
						// revert the changes (restore the tree)
						if (predecessor != null) predecessor.Right = null;
						res.Add(current.Val);
						current = current.Right;
					}
				}
			}
			return res;
		}

		static string Stringify(List<int> seq)
		{
			return "[" + string.Join(", ", seq) + "]";
		}

		static bool SeqEqual(List<int> a, List<int> b)
		{
			if (a == null && b == null) return true;
			if (a == null || b == null) return false;
			if (a.Count != b.Count) return false;
			for (int i = 0; i < a.Count; i++) if (a[i] != b[i]) return false;
			return true;
		}
	}
}

