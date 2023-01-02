using System;
using System.Collections.Generic;

namespace SodikmLauncher;

internal class Faker
{
	private static Random Random = new Random();

	private static List<string> UsernameParts = new List<string>
	{
		"elephant", "roblox", "xxx", "gamer", "xbox", "fan", "boy", "old", "ps2", "ps3",
		"playstation", "bling", "hacker", "haxxer", "hax", "z0mg", "mine", "craft", "broke", "minecraft",
		"blockland", "diamond", "mlg", "guy", "girl", "dude", "the", "guest", "awesome", "knight",
		"fighter", "rapper", "rich", "beautiful", "bacon", "waffle", "house", "walls", "baller", "ye",
		"html", "ancient", "team", "fortress", "counter", "strike", "ricochet", "half", "life", "halflife3",
		"aim", "aol", "skype", "official", "real", "twitter", "block", "land", "lego", "cyan",
		"blue", "red", "voilet", "purple", "orange", "orangebox", "box", "pink", "green", "violent",
		"bully", "xtreme", "extreme", "epic", "sword", "skater", "soccer", "football", "skate", "hockey",
		"america", "europe", "hotline", "911", "call", "meh", "plz", "poor", "donate", "please",
		"thx", "buy", "me", "coffee", "and", "cookie", "yay", "fancy", "super", "male",
		"female", "robot", "bot", "binary", "executable", "dot", "exe", "injector", "love", "channel",
		"beep", "boop", "cole"
	};

	public static int GenerateId()
	{
		return Random.Next(1, 10000000);
	}

	public static string GenerateUsername()
	{
		string text = UsernameParts[Random.Next(UsernameParts.Count)];
		string text2 = UsernameParts[Random.Next(UsernameParts.Count)];
		string text3 = Random.Next(0, 100).ToString();
		return text + text2 + text3;
	}
}
