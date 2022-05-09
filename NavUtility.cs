using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace NavUtility
{
	public static class Utility
	{
		public static void TestUtility()
		{
			Debug.Log("UTILITY WORKING");
		}

		public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
		{
			return list.OrderBy(arg => System.Guid.NewGuid()).Take(elementsCount).ToList();
		}

		public static Sprite ConvertToSprite(this Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}


		public static Vector3 EditX(this Vector3 _value, float newXValue)
		{
			return new Vector3(newXValue, _value.y, _value.z);
		}
		public static Vector3 EditY(this Vector3 _value, float newYValue)
		{
			return new Vector3(_value.x, newYValue, _value.z);
		}
		public static Vector3 EditZ(this Vector3 _value, float newZValue)
		{
			return new Vector3(_value.x, _value.y, newZValue);
		}
	}

	public class UtilityMonoBehavior : MonoBehaviour
	{

	}
}


/*CODE THAT PICK RANDOM ELEMENT FROM LIST*/
/*
Example usage:
var weights = new Dictionary<Animal, int>();
weights.Add(new Dog(), 90); // 90% spawn chance;
weights.Add(new Cat(), 10); // 10% spawn chance;

// Here the magic happens
Animal selected = WeightedRandomizer.From(weights).TakeOne(); 
// Note: No casting necessary. Strongly-typed Animal object is returned.

*/

/// <summary>
/// Static class to improve readability
/// Example:
///	<code>
/// var selected = WeightedRandomizer.From(weights).TakeOne();
/// </code>
/// 
/// </summary>
public static class WeightedRandomizer
{
	public static WeightedRandomizer<R> From<R>(Dictionary<R, int> spawnRate)
	{
		return new WeightedRandomizer<R>(spawnRate);
	}

	/// <summary>
	/// get random bool with percent chance of true as parameter
	/// parameter must be between 0-100 otherwise function will always return false
	/// </summary>
	public static bool GetWeightedBool(int _chance)
	{
		if (_chance >= 0 && _chance <= 100)
		{
			var weights = new Dictionary<bool, int>();
			weights.Add(true, _chance);
			weights.Add(false, (100 - _chance));
			return WeightedRandomizer.From(weights).TakeOne();
		}
		else
		{
			return false;
		}

	}
}

public class WeightedRandomizer<T>
{
	//private static Random _random = new Random();
	private Dictionary<T, int> _weights;

	/// <summary>
	/// Instead of calling this constructor directly,
	/// consider calling a static method on the WeightedRandomizer (non-generic) class
	/// for a more readable method call, i.e.:
	/// 
	/// <code>
	/// var selected = WeightedRandomizer.From(weights).TakeOne();
	/// </code>
	/// 
	/// </summary>
	/// <param name="weights"></param>
	public WeightedRandomizer(Dictionary<T, int> weights)
	{
		_weights = weights;
	}

	/// <summary>
	/// Randomizes one item
	/// </summary>
	/// <param name="spawnRate">An ordered list withe the current spawn rates. The list will be updated so that selected items will have a smaller chance of being repeated.</param>
	/// <returns>The randomized item.</returns>
	public T TakeOne()
	{
		// Sorts the spawn rate list
		var sortedSpawnRate = Sort(_weights);

		// Sums all spawn rates
		int sum = 0;
		foreach (var spawn in _weights)
		{
			sum += spawn.Value;
		}

		// Randomizes a number from Zero to Sum
		int roll = Random.Range(0, sum);

		// Finds chosen item based on spawn rate
		T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
		foreach (var spawn in sortedSpawnRate)
		{
			if (roll < spawn.Value)
			{
				selected = spawn.Key;
				break;
			}
			roll -= spawn.Value;
		}

		// Returns the selected item
		return selected;
	}

	private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
	{
		var list = new List<KeyValuePair<T, int>>(weights);

		// Sorts the Spawn Rate List for randomization later
		list.Sort(
			delegate (KeyValuePair<T, int> firstPair,
					 KeyValuePair<T, int> nextPair)
			{
				return firstPair.Value.CompareTo(nextPair.Value);
			}
		 );

		return list;
	}
}




//ADD COLOR TO STRING
public static class StringExtensions
{
	public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
	public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}