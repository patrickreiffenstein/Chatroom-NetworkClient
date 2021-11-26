using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client_Backend
{
	class ClientEvents
	{
		//3
		public event Action<(string user, string message, long timeStamp)> onMessageAction;
		//5
		public event Action<(string message, long timeStamp)> onLogMessageAction;
		//7
		public event Action<(int userID, string name)> onUserInfoReceivedAction;
		//9
		public event Action<int> onUserIDReceivedAction;
		//11
		public event Action<int> onUserLeftAction;

		//3
		public void MessageReceived(string user, string message, long timeStamp)
		{
			onMessageAction?.Invoke((user, message, timeStamp));
		}

		//5
		public void LogMessageReceived(string message, long timeStamp)
		{
			onLogMessageAction?.Invoke((message, timeStamp));
		}

		//7
		public void UserInfoReceived(int userID, string name)
		{
			onUserInfoReceivedAction?.Invoke((userID, name));
		}

		//9
		public void UserIDReceived(int ID)
		{
			onUserIDReceivedAction?.Invoke(ID);

		}

		//11
		public void UserLeft(int ID)
		{
			onUserLeftAction?.Invoke(ID);
		}
	}
}