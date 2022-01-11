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

        public static bool operator ==(Member member1, Member member2) => member1.ID == member2.ID;
        public static bool operator !=(Member member1, Member member2) => member1.ID != member2.ID;

        public static Member Server => new Member(0, "[Server]", DateTime.UtcNow, false);

        /// <summary>
        /// Compares members by ID and name.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Member member &&
                   ID == member.ID &&
                   Name == member.Name;
        }
    }
}
