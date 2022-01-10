using System;

namespace ChatroomBot
{
    public readonly struct Member
    {
        public readonly bool IsSelf;
        public readonly byte ID;
        public readonly string DisplayName;
        /// <summary>
        /// Returns the display name.
        /// Same as <c>DisplayName</c>.
        /// </summary>
        public readonly string Name;

        public readonly DateTime JoinedAt => joinedAtUTC.ToLocalTime();
        private readonly DateTime joinedAtUTC;

        public Member(byte id, string displayName, DateTime joinedAtUTC, bool isSelf)
        {
            ID = id;
            DisplayName = displayName;
            Name = displayName;
            this.joinedAtUTC = joinedAtUTC;
            IsSelf = isSelf;
        }
    }
}
