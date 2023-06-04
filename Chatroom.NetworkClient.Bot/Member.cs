using System;

namespace Chatroom.NetworkClient.Bot
{
    /// <summary>
    /// Member instance.
    /// </summary>
    public readonly struct Member
    {
        /// <summary>
        /// Whether this member is this bot.
        /// </summary>
        public readonly bool IsSelf;

        /// <summary>
        /// The ID of the user.
        /// </summary>
        public readonly byte ID;

        /// <summary>
        /// The display name of a member.
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// Returns the display name.
        /// Same as <c>DisplayName</c>.
        /// </summary>
        public readonly string Name;

        private readonly DateTime joinedAtUTC;

        /// <summary>
        /// Gets the timestamp of when the member joined.
        /// </summary>
        public readonly DateTime JoinedAt => joinedAtUTC.ToLocalTime();

        /// <summary>
        /// Initializes a new instance of the <see cref="Member"/> struct.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayName"></param>
        /// <param name="joinedAtUTC"></param>
        /// <param name="isSelf"></param>
        public Member(byte id, string displayName, DateTime joinedAtUTC, bool isSelf)
        {
            ID = id;
            DisplayName = displayName;
            Name = displayName;
            this.joinedAtUTC = joinedAtUTC;
            IsSelf = isSelf;
        }

        /// <summary>
        /// Gets the generic 'Server' member.
        /// </summary>
        public static Member Server => new Member(0, "[Server]", DateTime.UtcNow, false);

        public static bool operator ==(Member member1, Member member2) => member1.ID == member2.ID;

        public static bool operator !=(Member member1, Member member2) => member1.ID != member2.ID;

        /// <summary>
        /// Compares members by ID and name.
        /// </summary>
        /// <param name="obj">The other member.</param>
        /// <returns>Equality of members.</returns>
        public override bool Equals(object obj)
        {
            return obj is Member member &&
                   ID == member.ID &&
                   Name == member.Name;
        }
    }
}
