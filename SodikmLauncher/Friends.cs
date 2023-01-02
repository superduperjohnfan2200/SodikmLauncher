using System.Collections.Generic;

namespace SodikmLauncher;

internal class Friends
{
	public class FriendRequest
	{
		public long? Inviter { get; set; }

		public long? Invitee { get; set; }

		public FriendStatus? Status { get; set; }
	}

	public enum FriendStatus
	{
		NotFriend,
		Friend
	}

	private static List<FriendRequest> FriendRequests = new List<FriendRequest>();

	private static FriendRequest? GetFriendRequest(long user, long otherUser, bool createIfNone = false)
	{
		FriendRequest friendRequest = FriendRequests.Find((FriendRequest x) => (x.Inviter == user && x.Invitee == otherUser) || (x.Inviter == otherUser && x.Invitee == user));
		if (friendRequest == null && createIfNone)
		{
			friendRequest = new FriendRequest
			{
				Inviter = user,
				Invitee = otherUser,
				Status = FriendStatus.NotFriend
			};
			FriendRequests.Add(friendRequest);
		}
		return friendRequest;
	}

	public static bool AreFriend(long user, long otherUser)
	{
		FriendRequest friendRequest = GetFriendRequest(user, otherUser);
		if (friendRequest != null)
		{
			return friendRequest.Status == FriendStatus.Friend;
		}
		return false;
	}

	public static long[] AreFriends(long user, long[] users)
	{
		List<long> list = new List<long>();
		foreach (long num in users)
		{
			if (AreFriend(user, num))
			{
				list.Add(num);
			}
		}
		return list.ToArray();
	}

	public static void CreateFriend(long inviter, long invitee)
	{
		GetFriendRequest(inviter, invitee, createIfNone: true).Status = FriendStatus.Friend;
	}

	public static void BreakFriend(long inviter, long invitee)
	{
		GetFriendRequest(inviter, invitee, createIfNone: true).Status = FriendStatus.NotFriend;
	}

	public static void Clear()
	{
		FriendRequests.Clear();
	}
}
